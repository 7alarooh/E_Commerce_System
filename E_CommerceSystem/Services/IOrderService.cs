using E_CommerceSystem.Models;

namespace E_CommerceSystem.Services
{
    public interface IOrderService
    {
        bool PlaceOrder(Order order, List<(int ProductId, int Quantity)> orderItems);
        IEnumerable<Order> GetAllOrders(); // <-- Add this method declaration
        Order GetOrderById(int id);
    }
}