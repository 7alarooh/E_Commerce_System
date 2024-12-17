using System.Collections.Generic;
using E_CommerceSystem.Models;

namespace E_CommerceSystem.Services
{
    /// <summary>
    /// Interface for Product Service to define business logic for the Product table.
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Retrieves all products.
        /// </summary>
        /// <returns>List of all products</returns>
        IEnumerable<Product> GetAllProducts();

        /// <summary>
        /// Retrieves a single product by ID.
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>Product if found, otherwise null</returns>
        Product GetProductById(int id);

        /// <summary>
        /// Adds a new product after validation.
        /// </summary>
        /// <param name="product">Product entity</param>
        /// <returns>True if product is added successfully</returns>
        bool AddProduct(Product product);

        /// <summary>
        /// Updates an existing product after validation.
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <param name="updatedProduct">Updated product data</param>
        /// <returns>True if product is updated successfully</returns>
        bool UpdateProduct(int id, Product updatedProduct);

        /// <summary>
        /// Deletes a product by ID.
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>True if product is deleted successfully</returns>
        bool DeleteProduct(int id);
    }
}
