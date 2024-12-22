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
                // Fetch the product from the repository
                var product = _productRepository.GetProductById(item.ProductId);
                if (product == null)
                {
                    throw new ArgumentException($"Product with ID {item.ProductId} does not exist.");
                }

                // Check if stock is sufficient
                if (product.Stock < item.Quantity)
                {
                    throw new InvalidOperationException($"Insufficient stock for Product ID {item.ProductId}. Available stock: {product.Stock}.");
                }

                // Reduce stock and calculate total amount
                product.Stock -= item.Quantity;
                totalAmount += product.Price * item.Quantity;

                // Update the product in the database
                _productRepository.UpdateProduct(product.Id, product);
            }

            // Set order details
            order.OrderDate = DateTime.Now;
            order.TotalAmount = totalAmount;

            // Save the order in the repository
            _orderRepository.AddOrder(order);

            // Save order items in the repository
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
