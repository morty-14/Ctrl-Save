using System.ComponentModel.DataAnnotations;

namespace Ctrl_Save.Models
{
    public class ContactDto
    {
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; } = "";

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; } = "";

        // "email" or "phone"
        [Required]
        public string ContactMethod { get; set; } = "email";

        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Please enter a valid phone number")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Message is required")]
        public string Message { get; set; } = "";
    }
}
