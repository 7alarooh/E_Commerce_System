using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;

namespace E_CommerceSystem.Models
{
    public class User
    {
        [Key] // Primary Key
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters.")]
        public string Email { get; set; }

        private string _password;

        [Required(ErrorMessage = "Password is required.")]
        public string Password
        {
            get => _password;
            set
            {
                if (IsValidPassword(value))
                {
                    _password = HashPassword(value);
                }
                else
                {
                    throw new ArgumentException("Password must be at least 8 characters, contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");
                }
            }
        }

        [Required(ErrorMessage = "Phone is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        [StringLength(50, ErrorMessage = "Role cannot exceed 50 characters.")]
        public string Role { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Validate Password with Regex
        private bool IsValidPassword(string password)
        {
            const string passwordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
            return Regex.IsMatch(password, passwordRegex);
        }

        // Hash Password using SHA256
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
