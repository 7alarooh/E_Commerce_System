using E_CommerceSystem.Models;

namespace E_CommerceSystem.Repositories
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetAllOrders();
        IEnumerable<Order> GetOrdersByUserId(int userId);
        Order GetOrderById(int id);
        bool AddOrder(Order order);
        bool UpdateOrder(int id, Order updatedOrder);
        bool DeleteOrder(int id);
    }
}