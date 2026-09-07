using System.ComponentModel.DataAnnotations;

namespace Ctrl_Save.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Order number is required")]
        public string OrderNumber { get; set; } = "";

        [Required(ErrorMessage = "First name is required")]
        [MaxLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        public string FirstName { get; set; } = "";

        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        public string LastName { get; set; } = "";

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        public string Phone { get; set; } = "";

        [Required(ErrorMessage = "Address is required")]
        [MaxLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
        public string Address { get; set; } = "";

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; } = "";

        [Required(ErrorMessage = "Region is required")]
        public string Region { get; set; } = "";

        [Required(ErrorMessage = "Payment method is required")]
        public string PaymentMethod { get; set; } = "";

        [Range(0, double.MaxValue, ErrorMessage = "Order total must be a positive value")]
        public decimal OrderTotal { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public List<OrderItem> Items { get; set; } = new();
    }
}
