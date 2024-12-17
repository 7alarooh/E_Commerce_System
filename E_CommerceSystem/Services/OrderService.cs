using System.Collections.Generic;
using System.Linq;
using E_CommerceSystem.Models;
using E_CommerceSystem.Repositories;

namespace E_CommerceSystem.Services
{
    /// <summary>
    /// Service layer for handling business logic related to Orders.
    /// Implements IOrderService.
    /// </summary>
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderProductsRepository _orderProductsRepository;
        private readonly IProductRepository _productRepository;

        public OrderService(
            IOrderRepository orderRepository,
            IOrderProductsRepository orderProductsRepository,
            IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _orderProductsRepository = orderProductsRepository;
            _productRepository = productRepository;
        }

        /// <summary>
        /// Places an order after verifying sufficient stock and calculating the total amount.
        /// </summary>
        /// <param name="order">Order details</param>
        /// <param name="orderItems">List of product IDs and quantities</param>
        /// <returns>True if the order is placed successfully, otherwise false</returns>
        public bool PlaceOrder(Order order, List<(int ProductId, int Quantity)> orderItems)
        {
            decimal totalAmount = 0;

            // Check stock availability
            foreach (var item in orderItems)
            {
                var product = _productRepository.GetProductById(item.ProductId);
                if (product == null || product.Stock < item.Quantity)
                {
                    throw new InvalidOperationException($"Insufficient stock for Product ID: {item.ProductId}");
                }

                totalAmount += product.Price * item.Quantity;
            }

            // Reduce stock
            foreach (var item in orderItems)
            {
                var product = _productRepository.GetProductById(item.ProductId);
                product.Stock -= item.Quantity;
                _productRepository.UpdateProduct(product.Id, product);
            }

            // Add order to database
            order.OrderDate = DateTime.Now;
            _orderRepository.AddOrder(order);

            // Add order items to OrderProducts
            foreach (var item in orderItems)
            {
                var orderProduct = new OrderProducts
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                _orderProductsRepository.AddOrderProduct(orderProduct);
            }

            return true;
        }

        /// <summary>
        /// Retrieve all orders.
        /// </summary>
        public IEnumerable<Order> GetAllOrders()
        {
            return _orderRepository.GetAllOrders(); // Fetch all orders from repository
        }

        /// <summary>
        /// Retrieve order by ID.
        /// </summary>
        public Order GetOrderById(int id)
        {
            return _orderRepository.GetOrderById(id);
        }
    }
}
