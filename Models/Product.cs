using System.ComponentModel.DataAnnotations;

namespace Ctrl_Save.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string ProductId { get; set; } = "";

        [Required(ErrorMessage = "Product name is required")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Price is required")]
        public string Price { get; set; } = "";

        [Required(ErrorMessage = "Image is required")]
        public string Image { get; set; } = "";

        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; } = "";

        [Required(ErrorMessage = "Condition is required")]
        public string Condition { get; set; } = "";

        public string Listed { get; set; } = "";
        public string Description { get; set; } = "";

        [Required(ErrorMessage = "Includes is required")]
        public string Includes { get; set; } = "";

        public bool IsAvailable { get; set; } = true;
    }
}
