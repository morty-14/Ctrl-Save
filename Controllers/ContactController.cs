using Ctrl_Save.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ctrl_Save.Controllers
{
    public class ContactController : Controller
    {
        private readonly Ctrl_SaveContext _context;
        private readonly string email = "contact@ctrlsave.com";
        private readonly string address = "Windhoek, Namibia";

        public ContactController(Ctrl_SaveContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewData["EmailAddress"] = email;
            ViewBag.Address = address;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ContactDto model)
        {
            ViewData["EmailAddress"] = email;
            ViewBag.Address = address;

            // Validate contact detail based on chosen method
            if (model.ContactMethod == "email" && string.IsNullOrWhiteSpace(model.Email))
                ModelState.AddModelError("Email", "Email address is required");
            if (model.ContactMethod == "phone" && string.IsNullOrWhiteSpace(model.Phone))
                ModelState.AddModelError("Phone", "Phone number is required");

            if (!ModelState.IsValid)
                return View(model);

            var message = new ContactMessage
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                ContactMethod = model.ContactMethod,
                ContactDetail = model.ContactMethod == "email" ? model.Email ?? "" : model.Phone ?? "",
                Message = model.Message,
                SentAt = DateTime.Now,
                IsRead = false
            };

            _context.ContactMessages.Add(message);
            await _context.SaveChangesAsync();

            ViewBag.SuccessMessage = "Your message has been received successfully!";
            ModelState.Clear();
            return View(new ContactDto());
        }
    }
}
