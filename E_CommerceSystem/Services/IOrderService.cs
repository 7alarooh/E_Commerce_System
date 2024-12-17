using E_CommerceSystem.Models;

namespace E_CommerceSystem.Services
{
    public interface IOrderService
    {
        bool PlaceOrder(Order order, List<(int ProductId, int Quantity)> orderItems);
    }
}