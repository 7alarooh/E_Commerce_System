using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_CommerceSystem.Models
{
    public class Order
    {
        [Key] // Specifies the primary key for the table.
        public int Id { get; set; }

        [Required(ErrorMessage = "UserId is required.")] // Ensures the UserId field is mandatory.
        [ForeignKey("User")] // Specifies that this property is a foreign key referencing the User table.
        public int UserId { get; set; }

        public User User { get; set; } // Navigation property for the User relationship.

        [Required(ErrorMessage = "OrderDate is required.")] // Ensures that the OrderDate is mandatory.
        public DateTime OrderDate { get; set; } = DateTime.Now; // Automatically sets the order date to the current date and time.


    }
}
