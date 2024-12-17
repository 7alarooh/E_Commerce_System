using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using E_CommerceSystem.Models;

namespace E_CommerceSystem.Repositories
{
    /// <summary>
    /// Repository for managing Review operations.
    /// Implements IReviewRepository.
    /// </summary>
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes the repository with the database context.
        /// </summary>
        public ReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieve all reviews.
        /// </summary>
        public IEnumerable<Review> GetAllReviews()
        {
            return _context.Reviews
                           .Include(r => r.User)
                           .Include(r => r.Product)
                           .ToList();
        }

        /// <summary>
        /// Retrieve a review by its ID.
        /// </summary>
        public Review GetReviewById(int id)
        {
            return _context.Reviews
                           .Include(r => r.User)
                           .Include(r => r.Product)
                           .FirstOrDefault(r => r.Id == id);
        }

        /// <summary>
        /// Retrieve reviews for a specific product.
        /// </summary>
        public IEnumerable<Review> GetReviewsByProductId(int productId)
        {
            return _context.Reviews
                           .Include(r => r.User)
                           .Where(r => r.ProductId == productId)
                           .ToList();
        }

        /// <summary>
        /// Add a new review to the database.
        /// </summary>
        public bool AddReview(Review review)
        {
            if (review == null) return false;

            _context.Reviews.Add(review);
            _context.SaveChanges();
            return true;
        }

        /// <summary>
        /// Delete a review by its ID.
        /// </summary>
        public bool DeleteReview(int id)
        {
            var review = _context.Reviews.Find(id);
            if (review == null) return false;

            _context.Reviews.Remove(review);
            _context.SaveChanges();
            return true;
        }
    }
}
