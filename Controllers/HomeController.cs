using Ctrl_Save.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Ctrl_Save.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [Authorize]
        public IActionResult Claims()
        {
            return Json(User.Claims.Select(c => new { c.Type, c.Value }));
        }

        public IActionResult AdminTest()
        {
            return Json(new { isAuth = User.Identity?.IsAuthenticated, name = User.Identity?.Name });
        }
    }
}
