using Microsoft.AspNetCore.Mvc;
using MVCview.Models;
using System.Text.Json;

namespace MVCview.Controllers
{
    public class AccountController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }


            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync(
                    "https://localhost:7050/api/auth/login", model);

                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.Error = "Invalid Email or Password";

                    return View();
                }

                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();


                HttpContext.Session.SetString("token", result.AccessToken);
                HttpContext.Session.SetString("refreshToken", result.RefreshToken);
                HttpContext.Session.SetString("role", result.role.ToString());
                HttpContext.Session.SetInt32("UserId", result.UserId);
                HttpContext.Session.SetString("permissions",JsonSerializer.Serialize(result.permissions));

                return RedirectToAction("Index", "Home");
            }
        }



        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }




        public IActionResult Logout()
        {
           HttpContext.Session.Clear();
           return RedirectToAction("Login", "Account");
        }




        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync(
                    "https://localhost:7050/api/auth/register",
                    model);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Login");
                }

                ViewBag.Error = "Registration failed";
            }

            return View(model);
        }
    }
}
