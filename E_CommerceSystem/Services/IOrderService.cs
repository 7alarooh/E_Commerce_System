using E_CommerceSystem.Models;

namespace E_CommerceSystem.Services
{
    public interface IOrderService
    {
        IEnumerable<Order> GetOrdersByUserId(int userId);
        Order GetOrderById(int id);
        bool PlaceOrder(Order order, List<(int ProductId, int Quantity)> orderItems);
        IEnumerable<Order> GetAllOrders();
    }
}