using E_CommerceSystem.Models;

namespace E_CommerceSystem.Services
{
    public interface IUserService
    {
        bool AddUser(User user);
        User GetUserByEmailAndPassword(string email, string password);
        User GetUserById(int id);
    }
}