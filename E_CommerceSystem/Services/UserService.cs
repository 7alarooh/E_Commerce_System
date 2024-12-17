using System;
using System.Text.RegularExpressions;
using E_CommerceSystem.Models;
using E_CommerceSystem.Repositories;

namespace E_CommerceSystem.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        /// <summary>
        /// Validate user login credentials.
        /// </summary>
        /// <param name="email">User's email</param>
        /// <param name="password">User's plain-text password</param>
        /// <returns>User if credentials are valid, otherwise null</returns>
        public User GetUserByEmailAndPassword(string email, string password)
        {
            // Delegate to repository to validate user credentials
            return _userRepository.GetUser(email, password);
        }

        /// <summary>
        /// Add a new user to the system.
        /// </summary>
        public bool AddUser(User user)
        {
            if (_userRepository.GetUser(user.Email, string.Empty) != null)
                throw new ArgumentException("Email already exists.");

            user.Password = HashPassword(user.Password);
            return _userRepository.AddUser(user);
        }

        /// <summary>
        /// Validate an email format using regular expressions.
        /// </summary>
        /// <param name="email">Email to validate</param>
        /// <returns>True if email is valid, otherwise false</returns>
        private bool IsValidEmail(string email)
        {
            var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailRegex);
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
