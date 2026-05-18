namespace MVCAdo.Models
{
    public class OrderDto
    {
        public int Id { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public int UserId { get; set; }
    }
}
