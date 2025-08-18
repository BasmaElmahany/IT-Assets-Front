using ItAssetsFront.Models;
using ItAssetsFront.Services.AuthService;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ItAssetsFront.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginService _authService;
        public LoginController()
        {
            _authService = new LoginService();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var token = await _authService.LoginAsync(model.email, model.password);

            if (string.IsNullOrEmpty(token))
            {
                ViewBag.Error = "Login failed. Please check your credentials.";
                return View(model);
            }

            var claims = _authService.DecodeToken(token);

            // ✅ Store in session
            HttpContext.Session.SetString("JWT", token);

            if (claims.TryGetValue("name", out var name))
                HttpContext.Session.SetString("UserName", name);

            if (claims.TryGetValue("email", out var email))
                HttpContext.Session.SetString("UserEmail", email);

            // Optional: store entire claims as JSON
            HttpContext.Session.SetString("UserClaims", JsonSerializer.Serialize(claims));
            ViewBag.Token = token;
            ViewBag.Claims = claims;
            return RedirectToAction("Index", "Home");
        }
    }
}
