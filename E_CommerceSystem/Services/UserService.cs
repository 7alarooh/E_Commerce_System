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
        /// Add a new user to the system after validating business rules.
        /// </summary>
        /// <param name="user">User object</param>
        /// <returns>True if the user is added successfully, otherwise false</returns>
        public bool AddUser(User user)
        {
            // Validate email format
            if (!IsValidEmail(user.Email))
                throw new ArgumentException("Invalid email format.");

            // Check if email is unique
            var existingUser = _userRepository.GetUser(user.Email, string.Empty);
            if (existingUser != null)
                throw new ArgumentException("Email must be unique.");

            // Hash the password
            user.Password = HashPassword(user.Password);

            // Delegate to repository to add the user
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

        /// <summary>
        /// Hash a password using SHA256.
        /// </summary>
        /// <param name="password">Plain-text password</param>
        /// <returns>Hashed password</returns>
        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                var builder = new System.Text.StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
