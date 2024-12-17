using E_CommerceSystem.Models;

namespace E_CommerceSystem.Repositories
{
    public interface IOrderProductsRepository
    {
        IEnumerable<OrderProducts> GetAllOrderProducts();
        IEnumerable<OrderProducts> GetOrderProductsByOrderId(int orderId);
        bool AddOrderProduct(OrderProducts orderProduct);
        bool DeleteOrderProduct(int orderId, int productId);
    }
}