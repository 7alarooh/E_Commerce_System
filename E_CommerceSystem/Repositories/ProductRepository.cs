using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using E_CommerceSystem.Models;

namespace E_CommerceSystem.Repositories
{
    /// <summary>
    /// Repository for managing Product table operations.
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes the repository with the database context.
        /// </summary>
        /// <param name="context">Application database context</param>
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieve all products.
        /// </summary>
        /// <returns>List of all products</returns>
        public IEnumerable<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }

        /// <summary>
        /// Retrieve a single product by ID.
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>Product if found, otherwise null</returns>
        public Product GetProductById(int id)
        {
            return _context.Products
                .Include(p => p.Reviews) // Include reviews for overall rating
                .FirstOrDefault(p => p.Id == id);
        }

        /// <summary>
        /// Add a new product to the database.
        /// </summary>
        /// <param name="product">Product entity</param>
        /// <returns>True if successful, otherwise false</returns>
        public bool AddProduct(Product product)
        {
            if (product == null) return false;

            _context.Products.Add(product);
            _context.SaveChanges();
            return true;
        }

        /// <summary>
        /// Update an existing product.
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <param name="updatedProduct">Updated product data</param>
        /// <returns>True if updated successfully, otherwise false</returns>
        public bool UpdateProduct(int id, Product updatedProduct)
        {
            var existingProduct = _context.Products.Find(id);
            if (existingProduct == null) return false;

            // Update product fields
            existingProduct.Name = updatedProduct.Name;
            existingProduct.Description = updatedProduct.Description;
            existingProduct.Price = updatedProduct.Price;
            existingProduct.Stock = updatedProduct.Stock;
            existingProduct.OverallRating = updatedProduct.OverallRating;

            _context.Products.Update(existingProduct);
            _context.SaveChanges();
            return true;
        }

        /// <summary>
        /// Delete a product by ID.
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>True if deleted successfully, otherwise false</returns>
        public bool DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null) return false;

            _context.Products.Remove(product);
            _context.SaveChanges();
            return true;
        }
    }
}
