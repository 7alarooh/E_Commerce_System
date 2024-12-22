using System;
using System.Collections.Generic;
using System.Linq;
using E_CommerceSystem.Models;
using E_CommerceSystem.Repositories;

namespace E_CommerceSystem.Services
{
    /// <summary>
    /// Service layer for handling business logic related to reviews.
    /// Implements IReviewService.
    /// </summary>
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public ReviewService(
            IReviewRepository reviewRepository,
            IOrderRepository orderRepository,
            IProductRepository productRepository)
        {
            _reviewRepository = reviewRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        /// <summary>
        /// Adds a review for a product after validating business rules.
        /// </summary>
        public void AddReview(int userId, int productId, int rating, string comment)
        {
            // Validate product existence
            var product = _productRepository.GetProductById(productId);
            if (product == null)
            {
                throw new ArgumentException("Product does not exist.");
            }

            // Validate if the user can review (e.g., purchased the product)
            var orders = _orderRepository.GetOrdersByUserId(userId);
            var hasPurchasedProduct = orders
                .SelectMany(order => order.OrderProducts)
                .Any(op => op.ProductId == productId);

            if (!hasPurchasedProduct)
            {
                throw new InvalidOperationException("You can only review products you have purchased.");
            }

            // Add the review
            var review = new Review
            {
                UserId = userId,
                ProductId = productId,
                Rating = rating,
                Comment = comment,
                ReviewDate = DateTime.Now
            };

            _reviewRepository.AddReview(review);

            // Recalculate OverallRating
            UpdateProductOverallRating(productId);
        }

        private void UpdateProductOverallRating(int productId)
        {
            // Get all reviews for the product
            var reviews = _reviewRepository.GetReviewsByProductId(productId);
            if (!reviews.Any())
            {
                return;
            }

            // Calculate the average rating
            var overallRating = (decimal)reviews.Average(r => r.Rating);

            // Update the product's OverallRating
            var product = _productRepository.GetProductById(productId);
            if (product != null)
            {
                product.OverallRating = Math.Round(overallRating, 2); // Round to 2 decimal places
                _productRepository.UpdateProduct(product.Id, product);
            }
        }

        /// <summary>
        /// Retrieves reviews for a specific product.
        /// </summary>
        public IEnumerable<Review> GetReviewsByProductId(int productId)
        {
            return _reviewRepository.GetReviewsByProductId(productId);
        }

        /// <summary>
        /// Retrieves a review by its ID.
        /// </summary>
        public Review GetReviewById(int id)
        {
            return _reviewRepository.GetReviewById(id);
        }


        /// <summary>
        /// Recalculates the overall rating of a product.
        /// </summary>
        private void RecalculateProductRating(int productId)
        {
            var reviews = _reviewRepository.GetReviewsByProductId(productId);

            if (reviews.Any())
            {
                var overallRating = (decimal)reviews.Average(r => r.Rating);

                var product = _productRepository.GetProductById(productId);
                if (product != null)
                {
                    product.OverallRating = Math.Round(overallRating, 2); // Update to two decimal places
                    _productRepository.UpdateProduct(product.Id, product);
                }
            }
        }
    }
}
