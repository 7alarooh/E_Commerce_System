using System.Linq;
using System.Security.Cryptography;
using System.Text;
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

        /// <summary>
        /// Retrieve user by ID.
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>User object</returns>
        public User GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        /// <summary>
        /// Get a user by email and password (hashed).
        /// </summary>
        /// <param name="email">User's email</param>
        /// <param name="password">User's plain-text password</param>
        /// <returns>User if email and password match, otherwise null</returns>
        public User GetUser(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return null;

            return _context.Users.FirstOrDefault(u => u.Email == email && u.Password == HashPassword(password));
        }

        /// <summary>
        /// Add a new user to the database.
        /// </summary>
        /// <param name="user">User object</param>
        /// <returns>True if user is added successfully, otherwise false</returns>
        public bool AddUser(User user)
        {
            try
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding user: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Update an existing user's details.
        /// </summary>
        /// <param name="id">User's ID</param>
        /// <param name="updatedUser">Updated user details</param>
        /// <returns>True if update is successful, otherwise false</returns>
        public bool UpdateUser(int id, User updatedUser)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return false;

            // Update user details
            user.Name = updatedUser.Name;
            user.Phone = updatedUser.Phone;
            user.Role = updatedUser.Role;
            user.Email = updatedUser.Email;

            // Update password (if provided)
            if (!string.IsNullOrWhiteSpace(updatedUser.Password))
            {
                user.Password = HashPassword(updatedUser.Password);
            }

            _context.Users.Update(user);
            _context.SaveChanges();
            return true;
        }

        /// <summary>
        /// Hash a password using SHA256.
        /// </summary>
        /// <param name="password">Plain-text password</param>
        /// <returns>Hashed password</returns>
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
