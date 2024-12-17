using E_CommerceSystem.Models;

namespace E_CommerceSystem.Repositories
{
    public interface IProductRepository
    {
        bool AddProduct(Product product);
        bool DeleteProduct(int id);
        IEnumerable<Product> GetAllProducts();
        Product GetProductById(int id);
        bool UpdateProduct(int id, Product updatedProduct);
    }
}