using E_CommerceSystem.Models;

namespace E_CommerceSystem.Services
{
    public interface IOrderProductsService
    {
        IEnumerable<OrderProducts> GetOrderProductsByOrderId(int orderId);
    }
}