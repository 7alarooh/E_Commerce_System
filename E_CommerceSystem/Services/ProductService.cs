using System.Collections.Generic;
using System;
using E_CommerceSystem.Models;
using E_CommerceSystem.Repositories;

namespace E_CommerceSystem.Services
{
    /// <summary>
    /// Service layer for handling business logic related to products.
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        /// <summary>
        /// Initializes the ProductService with a repository.
        /// </summary>
        /// <param name="productRepository">Product repository dependency</param>
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        /// Retrieves all products.
        /// </summary>
        /// <returns>List of all products</returns>
        public IEnumerable<Product> GetAllProducts()
        {
            return _productRepository.GetAllProducts();
        }

        /// <summary>
        /// Retrieves a product by ID.
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>Product if found, otherwise null</returns>
        public Product GetProductById(int id)
        {
            return _productRepository.GetProductById(id);
        }

        /// <summary>
        /// Adds a new product after validating business rules.
        /// </summary>
        /// <param name="product">Product entity</param>
        /// <returns>True if successful, otherwise false</returns>
        public bool AddProduct(Product product)
        {
            // Validate Price
            if (product.Price <= 0)
                throw new ArgumentException("Product price must be greater than zero.");

            // Validate Stock
            if (product.Stock < 0)
                throw new ArgumentException("Product stock cannot be negative.");

            // Delegate to repository
            return _productRepository.AddProduct(product);
        }

        /// <summary>
        /// Updates an existing product after validating business rules.
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <param name="updatedProduct">Updated product data</param>
        /// <returns>True if updated successfully, otherwise false</returns>
        public bool UpdateProduct(int id, Product updatedProduct)
        {
            // Validate Price
            if (updatedProduct.Price <= 0)
                throw new ArgumentException("Product price must be greater than zero.");

            // Validate Stock
            if (updatedProduct.Stock < 0)
                throw new ArgumentException("Product stock cannot be negative.");

            // Delegate to repository
            return _productRepository.UpdateProduct(id, updatedProduct);
        }

        /// <summary>
        /// Deletes a product by ID.
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>True if deleted successfully, otherwise false</returns>
        public bool DeleteProduct(int id)
        {
            return _productRepository.DeleteProduct(id);
        }

        public IEnumerable<Product> GetFilteredProducts(string name, decimal? minPrice, decimal? maxPrice, int pageNumber, int pageSize)
        {
            var query = _productRepository.GetAllProducts().AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(p => p.Name.Contains(name));

            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);

            return query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

    }
}
