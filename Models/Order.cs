namespace Ctrl_Save.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Address { get; set; } = "";
        public string City { get; set; } = "";
        public string Region { get; set; } = "";
        public string PaymentMethod { get; set; } = "";
        public decimal OrderTotal { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public List<OrderItem> Items { get; set; } = new();
    }

    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        public string ProductId { get; set; } = "";
        public string ProductName { get; set; } = "";
        public string Price { get; set; } = "";
        public string Category { get; set; } = "";
    }
}
