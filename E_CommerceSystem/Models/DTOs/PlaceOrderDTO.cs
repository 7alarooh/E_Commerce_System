namespace E_CommerceSystem.Models.DTOs
{
    public class PlaceOrderDTO
    {
        public int UserId { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }
    }
}
