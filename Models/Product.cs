namespace Ctrl_Save.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductId { get; set; } = "";
        public string Name { get; set; } = "";
        public string Price { get; set; } = "";
        public string Image { get; set; } = "";
        public string Category { get; set; } = "";
        public string Condition { get; set; } = "";
        public string Listed { get; set; } = "";
        public string Description { get; set; } = "";
        public string Includes { get; set; } = "";
        public bool IsAvailable { get; set; } = true;
    }
}
