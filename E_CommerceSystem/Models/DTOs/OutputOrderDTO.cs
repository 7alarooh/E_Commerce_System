using System;
using System.Collections.Generic;

namespace E_CommerceSystem.Models.DTOs
{
    public class OutputOrderDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OutputOrderItemDTO> OrderItems { get; set; }
    }

    public class OutputOrderItemDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
