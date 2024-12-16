using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_CommerceSystem.Models
{
    public class OrderProducts
    {
        [Required(ErrorMessage = "OrderId is required.")] // Ensures OrderId is mandatory.
        [ForeignKey("Order")] // Establishes a foreign key relationship with the Order table.
        public int OrderId { get; set; }

        public Order Order { get; set; } // Navigation property for the Order relationship.

        [Required(ErrorMessage = "ProductId is required.")] // Ensures ProductId is mandatory.
        [ForeignKey("Product")] // Establishes a foreign key relationship with the Product table.
        public int ProductId { get; set; }

        public Product Product { get; set; } // Navigation property for the Product relationship.

        [Required(ErrorMessage = "Quantity is required.")] // Ensures the Quantity field is mandatory.
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")] // Validates that the Quantity is at least 1.
        public int Quantity { get; set; }
    }
}
