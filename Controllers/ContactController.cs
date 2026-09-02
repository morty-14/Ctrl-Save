using Ctrl_Save.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Ctrl_Save.Controllers
{
    public class ContactController : Controller
    {
        private readonly string email = "contact@ctrlsave.com";
        private readonly string address = "Windhoek West, Windhoek, Namibia";
        public IActionResult Index()
        {
            ViewData["EmailAddress"] = email;
            ViewBag.Address = address;

            return View();
        }

        
        [HttpPost]
        public IActionResult Index(ContactDto model)
        {
            ViewData["EmailAddress"] = email;
            ViewBag.Address = address;

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            ViewBag.SuccessMEssage = "Your message has been sent succesfully.";
            ModelState.Clear();

            return View(model);
        }
    }
}
