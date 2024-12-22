using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace E_CommerceSystem.Models.DTOs
{
    public class InputOrderDTO
    {
        [Required(ErrorMessage = "UserId is required.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Order items are required.")]
        public List<OrderItemDTO> OrderItems { get; set; }
    }

    public class OrderItemDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
