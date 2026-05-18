using Microsoft.AspNetCore.Mvc;
using MVCview.Models;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MVCview.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var role = HttpContext.Session.GetString("role");
            var token = HttpContext.Session.GetString("token");

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync(
                    $"https://localhost:7050/api/permissions/{role}");


                if (response.IsSuccessStatusCode)
                {
                    var permissions = await response.Content
                        .ReadFromJsonAsync<List<PermissionDto>>();

                    HttpContext.Session.SetString(
                        "permissions",
                        JsonSerializer.Serialize(permissions));
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();

                    ViewBag.Error = error;

                    return RedirectToAction("Login", "Account");
                }
            }

            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
