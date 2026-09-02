using Microsoft.AspNetCore.Mvc;

namespace Ctrl_Save.Controllers
{
    public class SupportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Faqs()
        {
            return View();
        }

        public IActionResult BuyingHelp()
        {
            return View();
        }

        public IActionResult Orders()
        {
            return View();
        }

        public IActionResult Warranty()
        {
            return View();
        }
    }
}
