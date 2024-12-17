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
        public bool AddReview(int userId, int productId, int rating, string comment)
        {
            // 1. Check if the user has purchased the product.
            var orders = _orderRepository.GetAllOrders()
                        .Where(o => o.UserId == userId && o.OrderProducts.Any(op => op.ProductId == productId));

            if (!orders.Any())
                throw new InvalidOperationException("You can only review a product you have purchased.");

            // 2. Check if the user has already reviewed this product.
            var existingReview = _reviewRepository.GetReviewsByProductId(productId)
                                .FirstOrDefault(r => r.UserId == userId);

            if (existingReview != null)
                throw new InvalidOperationException("You have already reviewed this product.");

            // 3. Validate the rating.
            if (rating < 1 || rating > 5)
                throw new ArgumentException("Rating must be between 1 and 5.");

            // 4. Create and save the new review.
            var review = new Review
            {
                UserId = userId,
                ProductId = productId,
                Rating = rating,
                Comment = comment,
                ReviewDate = DateTime.Now
            };

            var success = _reviewRepository.AddReview(review);

            if (!success) return false;

            // 5. Recalculate the product's overall rating.
            RecalculateProductRating(productId);

            return true;
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
