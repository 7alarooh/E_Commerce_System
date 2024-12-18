using System;
using System.Text.RegularExpressions;
using E_CommerceSystem.Models;
using E_CommerceSystem.Repositories;

namespace E_CommerceSystem.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Retrieve user details by ID.
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>User object if found, otherwise null</returns>
        public User GetUserById(int id)
        {
            var user = _userRepository.GetUserById(id);
            if (user == null)
                throw new ArgumentException($"User with ID {id} does not exist.");
            return user;
        }

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
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
            if (!IsValidEmail(email))
                throw new ArgumentException("Invalid email format.");

            return _userRepository.GetUser(email, password);
        }

        /// <summary>
        /// Add a new user to the system.
        /// </summary>
        public bool AddUser(User user)
        {
            if (!IsValidEmail(user.Email))
                throw new ArgumentException("Invalid email format.");

            if (_userRepository.GetUser(user.Email, string.Empty) != null)
                throw new ArgumentException("Email already exists.");

            // No need to hash here since User.Password setter already hashes
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
