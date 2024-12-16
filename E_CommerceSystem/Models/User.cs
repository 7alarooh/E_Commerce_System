using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;

namespace E_CommerceSystem.Models
{
    public class User
    {
        [Key] // Primary Key// Specifies this property as the Primary Key in the database table.
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]// Ensures that the Name field cannot be null or empty.
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")] // Limits the maximum length of the Name field.
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")] // Ensures that the Email field is mandatory.
        [EmailAddress(ErrorMessage = "Invalid email format.")] // Validates the format of the email using a predefined pattern.
        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters.")] // Limits the maximum length of the Email field.
        public string Email { get; set; }

        private string _password;

        [Required(ErrorMessage = "Password is required.")] // Ensures that the Password field cannot be null or empty.
        public string Password
        {
            get => _password;
            set
            {
                // Validates the password against a regex pattern for complexity requirements.
                if (IsValidPassword(value))
                {
                    _password = HashPassword(value); // Hashes the password using SHA256 before storing it.
                }
                else
                {
                    // Throws an error if the password doesn't meet the complexity requirements.
                    throw new ArgumentException("Password must be at least 8 characters, contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");
                }
            }
        }

        [Required(ErrorMessage = "Phone is required.")]// Ensures that the Phone field is mandatory.
        [Phone(ErrorMessage = "Invalid phone number format.")] // Validates the phone number format.
        public string Phone { get; set; }


        [Required(ErrorMessage = "Role is required.")]// Ensures that the Role field cannot be null or empty.
        [StringLength(50, ErrorMessage = "Role cannot exceed 50 characters.")] // Limits the length of the Role field.
        public string Role { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;// Automatically sets the current date and time when a user is created.

        // Navigation property for orders (One-to-Many)
        public ICollection<Order> Orders { get; set; } = new List<Order>();

        // Navigation property for reviews (One-to-Many)
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        // Validates the password based on a regular expression (Regex).
        // Validate Password with Regex
        private bool IsValidPassword(string password)
        { 
            // Regex pattern ensures the password includes:
            // - At least one lowercase letter.
            // - At least one uppercase letter.
            // - At least one digit.
            // - At least one special character.
            // - Minimum length of 8 characters.
            const string passwordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
            return Regex.IsMatch(password, passwordRegex);
        }

        // Hash Password using SHA256 // Hashes the password using the SHA256 cryptographic algorithm.
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {// Converts the password string to a byte array and computes the hash.
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Converts the byte array to a hexadecimal string for storage.
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2")); // Converts each byte to a hexadecimal representation.
                }
                return builder.ToString(); // Returns the hashed password as a string.
            }
        }
    }
}
