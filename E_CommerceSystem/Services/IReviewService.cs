using E_CommerceSystem.Models;

namespace E_CommerceSystem.Services
{
    public interface IReviewService
    {
        void AddReview(int userId, int productId, int rating, string comment);
        Review GetReviewById(int id);
        IEnumerable<Review> GetReviewsByProductId(int productId);
    }
}