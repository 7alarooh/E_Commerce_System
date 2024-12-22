using System.Linq;
using Microsoft.EntityFrameworkCore;
using E_CommerceSystem.Models;

namespace E_CommerceSystem.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public User GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public User GetUser(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public bool AddUser(User user)
        {
            if (_context.Users.Any(u => u.Email == user.Email))
                throw new ArgumentException("Email already exists.");

            _context.Users.Add(user);
            _context.SaveChanges();
            return true;
        }

        public bool UpdateUser(int id, User updatedUser)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return false;

            user.Name = updatedUser.Name;
            user.Phone = updatedUser.Phone;
            user.Role = updatedUser.Role;
            user.Email = updatedUser.Email;

            if (!string.IsNullOrWhiteSpace(updatedUser.Password))
                user.Password = updatedUser.Password;

            _context.Users.Update(user);
            _context.SaveChanges();
            return true;
        }
    }
}
