using System.ComponentModel.DataAnnotations;

namespace Ctrl_Save.Models.DTOs
{
    public class ProductDto
    {
        public string ProductId { get; set; } = "";
        public string Name { get; set; } = "";
        public string Price { get; set; } = "";
        public string Category { get; set; } = "";
        public string Condition { get; set; } = "";
        public string Image { get; set; } = "";
        public string Includes { get; set; } = "";
        public bool IsAvailable { get; set; }
        public string Listed { get; set; } = "";
    }

    public class ProductCreateDto
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Price is required")]
        public string Price { get; set; } = "";

        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; } = "";

        [Required(ErrorMessage = "Condition is required")]
        public string Condition { get; set; } = "";

        [Required(ErrorMessage = "Image is required")]
        public string Image { get; set; } = "";

        public string Includes { get; set; } = "";
    }
}
