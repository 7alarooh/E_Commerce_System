using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using E_CommerceSystem.Models;

namespace E_CommerceSystem.Repositories
{
    /// <summary>
    /// Repository for managing OrderProducts operations.
    /// </summary>
    public class OrderProductsRepository : IOrderProductsRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderProductsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<OrderProducts> GetAllOrderProducts()
        {
            return _context.OrderProducts
                           .Include(op => op.Order)
                           .Include(op => op.Product)
                           .ToList();
        }

        public IEnumerable<OrderProducts> GetOrderProductsByOrderId(int orderId)
        {
            return _context.OrderProducts
                           .Where(op => op.OrderId == orderId)
                           .Include(op => op.Product)
                           .ToList();
        }

        public bool AddOrderProduct(OrderProducts orderProduct)
        {
            if (orderProduct == null) return false;

            _context.OrderProducts.Add(orderProduct);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteOrderProduct(int orderId, int productId)
        {
            var orderProduct = _context.OrderProducts
                                       .FirstOrDefault(op => op.OrderId == orderId && op.ProductId == productId);
            if (orderProduct == null) return false;

            _context.OrderProducts.Remove(orderProduct);
            _context.SaveChanges();
            return true;
        }
    }
}
