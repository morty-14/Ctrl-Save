using Microsoft.AspNetCore.Mvc;

namespace Ctrl_Save.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult AccessDenied()
        {
            return RedirectToAction("AccessDenied", "Auth");
        }
    }
}
