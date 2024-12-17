using E_CommerceSystem.Models;

namespace E_CommerceSystem.Repositories
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetAllOrders();
        Order GetOrderById(int id);
        bool AddOrder(Order order);
        bool UpdateOrder(int id, Order updatedOrder);
        bool DeleteOrder(int id);
    }
}