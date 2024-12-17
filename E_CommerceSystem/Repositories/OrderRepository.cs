using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using E_CommerceSystem.Models;

namespace E_CommerceSystem.Repositories
{
    /// <summary>
    /// Repository for managing Order operations.
    /// </summary>
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _context.Orders
                           .Include(o => o.User)
                           .Include(o => o.OrderProducts)
                           .ThenInclude(op => op.Product)
                           .ToList();
        }

        public Order GetOrderById(int id)
        {
            return _context.Orders
                           .Include(o => o.User)
                           .Include(o => o.OrderProducts)
                           .ThenInclude(op => op.Product)
                           .FirstOrDefault(o => o.Id == id);
        }

        public bool AddOrder(Order order)
        {
            if (order == null) return false;

            _context.Orders.Add(order);
            _context.SaveChanges();
            return true;
        }

        public bool UpdateOrder(int id, Order updatedOrder)
        {
            var existingOrder = _context.Orders.Find(id);
            if (existingOrder == null) return false;

            existingOrder.OrderDate = updatedOrder.OrderDate;
            existingOrder.UserId = updatedOrder.UserId;

            _context.Orders.Update(existingOrder);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteOrder(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null) return false;

            _context.Orders.Remove(order);
            _context.SaveChanges();
            return true;
        }
    }
}
