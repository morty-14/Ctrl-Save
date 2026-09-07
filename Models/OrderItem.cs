using System.ComponentModel.DataAnnotations;

namespace Ctrl_Save.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Product ID is required")]
        public string ProductId { get; set; } = "";

        [Required(ErrorMessage = "Product name is required")]
        public string ProductName { get; set; } = "";

        [Required(ErrorMessage = "Price is required")]
        public string Price { get; set; } = "";

        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; } = "";
    }
}
