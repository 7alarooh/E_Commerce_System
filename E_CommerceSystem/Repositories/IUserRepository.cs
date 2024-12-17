using E_CommerceSystem.Models;

namespace E_CommerceSystem.Repositories
{
    public interface IUserRepository
    {
        /// <summary>
        /// Retrieve user by ID.
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>User object</returns>
        User GetUserById(int id);
        bool AddUser(User user);
        User GetUser(string email, string password);
        bool UpdateUser(int id, User updatedUser);
    }
}