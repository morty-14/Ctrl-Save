namespace Ctrl_Save.Models
{
    public class ErrorLog
    {
        public int Id { get; set; }
        public string Message { get; set; } = "";
        public string? StackTrace { get; set; }
        public string? Path { get; set; }
        public string? Method { get; set; }
        public int StatusCode { get; set; } = 500;
        public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
    }
}