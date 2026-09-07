using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;

namespace Ctrl_Save.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Login(string returnUrl = "/")
        {
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = returnUrl
            }, OpenIdConnectDefaults.AuthenticationScheme);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return SignOut(
                new AuthenticationProperties { RedirectUri = "/Auth/PostLogout" },
                CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectDefaults.AuthenticationScheme
            );
        }

        public IActionResult PostLogout()
        {
            // After Keycloak redirects back, clear the cart cookie here
            Response.Cookies.Append("CtrlSaveCart", "", new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(-1),
                Path = "/",
                HttpOnly = false,
                IsEssential = true,
                SameSite = SameSiteMode.Lax
            });
            return Redirect("/");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}