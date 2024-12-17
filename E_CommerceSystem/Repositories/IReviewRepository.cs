using E_CommerceSystem.Models;

namespace E_CommerceSystem.Repositories
{
    public interface IReviewRepository
    {
        bool AddReview(Review review);
        bool DeleteReview(int id);
        IEnumerable<Review> GetAllReviews();
        Review GetReviewById(int id);
        IEnumerable<Review> GetReviewsByProductId(int productId);
    }
}