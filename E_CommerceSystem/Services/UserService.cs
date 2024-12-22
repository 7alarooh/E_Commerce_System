using System.Text.RegularExpressions;
using E_CommerceSystem.Models;
using E_CommerceSystem.Repositories;

namespace E_CommerceSystem.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User GetUserById(int id)
        {
            var user = _userRepository.GetUserById(id);
            if (user == null)
                throw new ArgumentException($"User with ID {id} does not exist.");
            return user;
        }

        public bool AddUser(User user)
        {
            if (!IsValidEmail(user.Email))
                throw new ArgumentException("Invalid email format.");

            if (!IsValidPassword(user.Password))
                throw new ArgumentException("Password must meet complexity requirements.");

            user.Password = HashPassword(user.Password);
            return _userRepository.AddUser(user);
        }

        public User GetUserByEmailAndPassword(string email, string password)
        {
            if (!IsValidEmail(email))
                throw new ArgumentException("Invalid email format.");

            var user = _userRepository.GetUser(email);
            if (user == null || user.Password != HashPassword(password))
                throw new ArgumentException("Invalid email or password.");

            return user;
        }

        private bool IsValidEmail(string email)
        {
            var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailRegex);
        }

        private bool IsValidPassword(string password)
        {
            const string passwordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@#$%^&*!])[A-Za-z\d@#$%^&*!]{8,}$";
            return Regex.IsMatch(password, passwordRegex);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return string.Concat(bytes.Select(b => b.ToString("x2")));
            }
        }
    }
}
