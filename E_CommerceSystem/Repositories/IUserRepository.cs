using E_CommerceSystem.Models;

namespace E_CommerceSystem.Repositories
{
    public interface IUserRepository
    {
        bool AddUser(User user);
        User GetUser(string email, string password);
        bool UpdateUser(int id, User updatedUser);
    }
}