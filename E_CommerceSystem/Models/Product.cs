using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_CommerceSystem.Models
{
    public class Product
    {
        [Key] // Specifies the primary key for the table.
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")] // Ensures the Name field is not null or empty.
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")] // Limits the length of the Name field.
        public string Name { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")] // Optional description with a max length.
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required.")] // Ensures the Price field is mandatory.
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")] // Validates that the price is greater than 0.
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stock is required.")] // Ensures the Stock field is mandatory.
        [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative.")] // Validates that stock is non-negative.
        public int Stock { get; set; }
        //Navigation property for Reviews (One-to-Many)
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
       
        //Calculated property for Overall Rating
        [NotMapped] // Not stored in the database; calculated at runtime.
        public decimal OverallRating
        {
            get
            {
                // Calculate the average rating if reviews exist, otherwise return 0.
                if (Reviews != null && Reviews.Count > 0)
                {
                    decimal totalRating = 0;
                    foreach (var review in Reviews)
                    {
                        totalRating += review.Rating;
                    }
                    return Math.Round(totalRating / Reviews.Count, 2); // Average rating rounded to 2 decimal places.
                }
                return 0; // Default value if no reviews.
            }
        }

    }
}
