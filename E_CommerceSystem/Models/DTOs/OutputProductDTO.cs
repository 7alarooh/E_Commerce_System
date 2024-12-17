namespace E_CommerceSystem.Models.DTOs
{
    public class OutputProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public decimal OverallRating { get; set; }
    }
}
