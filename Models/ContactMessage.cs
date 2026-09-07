namespace Ctrl_Save.Models
{
    public class ContactMessage
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string ContactMethod { get; set; } = "email";
        public string ContactDetail { get; set; } = "";
        public string Message { get; set; } = "";
        public DateTime SentAt { get; set; } = DateTime.Now;
        public bool IsRead { get; set; } = false;
        public string? Reply { get; set; }
        public DateTime? RepliedAt { get; set; }
    }
}
