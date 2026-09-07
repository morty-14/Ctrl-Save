namespace Ctrl_Save.Models.DTOs
{
    public class OrderItemDto
    {
        public string ProductName { get; set; } = "";
        public string Price { get; set; } = "";
        public string Category { get; set; } = "";
    }

    public class OrderDto
    {
        public string OrderNumber { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string City { get; set; } = "";
        public string Region { get; set; } = "";
        public string PaymentMethod { get; set; } = "";
        public decimal OrderTotal { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }
}
