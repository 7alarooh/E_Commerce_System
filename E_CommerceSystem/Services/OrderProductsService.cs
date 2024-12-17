using System.Collections.Generic;
using E_CommerceSystem.Models;
using E_CommerceSystem.Repositories;

namespace E_CommerceSystem.Services
{
    /// <summary>
    /// Service layer for handling business logic related to OrderProducts.
    /// Implements IOrderProductsService.
    /// </summary>
    public class OrderProductsService : IOrderProductsService
    {
        private readonly IOrderProductsRepository _orderProductsRepository;

        public OrderProductsService(IOrderProductsRepository orderProductsRepository)
        {
            _orderProductsRepository = orderProductsRepository;
        }

        /// <summary>
        /// Retrieve order products for a specific order.
        /// </summary>
        public IEnumerable<OrderProducts> GetOrderProductsByOrderId(int orderId)
        {
            return _orderProductsRepository.GetOrderProductsByOrderId(orderId);
        }
    }
}
