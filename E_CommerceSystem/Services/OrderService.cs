using System;
using System.Collections.Generic;
using System.Linq;
using E_CommerceSystem.Models;
using E_CommerceSystem.Repositories;

namespace E_CommerceSystem.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderProductsRepository _orderProductsRepository;
        private readonly IProductRepository _productRepository;

        public OrderService(IOrderRepository orderRepository, IOrderProductsRepository orderProductsRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _orderProductsRepository = orderProductsRepository;
            _productRepository = productRepository;
        }

        public bool PlaceOrder(Order order, List<(int ProductId, int Quantity)> orderItems)
        {
            decimal totalAmount = 0;

            foreach (var item in orderItems)
            {
                var product = _productRepository.GetProductById(item.ProductId);
                if (product == null || product.Stock < item.Quantity)
                {
                    throw new InvalidOperationException($"Insufficient stock for Product ID: {item.ProductId}");
                }

                totalAmount += product.Price * item.Quantity;

                product.Stock -= item.Quantity;
                _productRepository.UpdateProduct(product.Id, product);
            }

            order.OrderDate = DateTime.Now;
            order.TotalAmount = totalAmount; // Set TotalAmount
            _orderRepository.AddOrder(order);

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

        public IEnumerable<Order> GetAllOrders()
        {
            return _orderRepository.GetAllOrders();
        }
        public IEnumerable<Order> GetOrdersByUserId(int userId)
        {
            return _orderRepository.GetAllOrders().Where(o => o.UserId == userId);
        }
        public Order GetOrderById(int id)
        {
            return _orderRepository.GetOrderById(id);
        }
    }
}
