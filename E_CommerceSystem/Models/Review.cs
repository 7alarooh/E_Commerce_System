using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_CommerceSystem.Models
{
    public class Review
    {
        [Key] // Specifies the primary key for the table.
        public int Id { get; set; }

        [Required(ErrorMessage = "UserId is required.")] // Ensures UserId is mandatory.
        [ForeignKey("User")] // Establishes a foreign key relationship with the User table.
        public int UserId { get; set; }

        public User User { get; set; } // Navigation property for the User relationship.

        [Required(ErrorMessage = "ProductId is required.")] // Ensures ProductId is mandatory.
        [ForeignKey("Product")] // Establishes a foreign key relationship with the Product table.
        public int ProductId { get; set; }

        public Product Product { get; set; } // Navigation property for the Product relationship.

        [Required(ErrorMessage = "Rating is required.")] // Ensures the Rating field is mandatory.
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")] // Validates that the rating is between 1 and 5.
        public int Rating { get; set; }

        [StringLength(1000, ErrorMessage = "Comment cannot exceed 1000 characters.")] // Limits the length of the optional Comment field.
        public string Comment { get; set; }

        [Required(ErrorMessage = "ReviewDate is required.")] // Ensures the ReviewDate field is mandatory.
        public DateTime ReviewDate { get; set; } = DateTime.Now; // Automatically sets the review date to the current date and time.
    }
}
