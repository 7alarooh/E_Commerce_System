using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using E_CommerceSystem.Models.DTOs;
using E_CommerceSystem.Services;
using E_CommerceSystem.Models;
using AutoMapper;
using System.Security.Claims;

namespace E_CommerceSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Secures endpoints to authenticated users only
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        /// <summary>
        /// Place a new order (Authenticated users only).
        /// </summary>
        [HttpPost("place")]
        public IActionResult PlaceOrder([FromBody] InputOrderDTO inputOrder)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var order = new Order
                {
                    UserId = inputOrder.UserId
                };

                var orderItems = new List<(int ProductId, int Quantity)>();
                foreach (var item in inputOrder.OrderItems)
                {
                    orderItems.Add((item.ProductId, item.Quantity));
                }

                _orderService.PlaceOrder(order, orderItems);
                return Ok(new { Message = "Order placed successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        /// <summary>
        /// Get all orders for an authenticated user.
        /// </summary>
        [HttpGet("GetUserOrders")]
        public IActionResult GetUserOrders()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized(new { message = "User ID not found in token." });
            }

            if (!int.TryParse(userIdString, out int userId))
            {
                return BadRequest(new { message = "Invalid User ID format." });
            }

            var orders = _orderService.GetOrdersByUserId(userId);
            if (!orders.Any())
            {
                return NotFound(new { message = "No orders found for the user." });
            }

            var outputOrders = orders.Select(o => new OutputOrderDTO
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                OrderProducts = o.OrderProducts.Select(op => new OrderProductDTO
                {
                    ProductId = op.ProductId,
                    Quantity = op.Quantity
                }).ToList()
            }).ToList();

            return Ok(outputOrders);
        }


        /// <summary>
        /// Get order details by ID (Authenticated users only).
        /// </summary>
        [HttpGet("{id}")]
        public IActionResult GetOrderById(int id)
        {
            var order = _orderService.GetOrderById(id);
            if (order == null)
                return NotFound(new { Error = "Order not found." });

            var outputOrder = _mapper.Map<OutputOrderDTO>(order);
            return Ok(outputOrder);
        }
    }
}
