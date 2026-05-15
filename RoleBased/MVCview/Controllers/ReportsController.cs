using Microsoft.AspNetCore.Mvc;
using MVCview.Models;
using System.Net.Http.Headers;

namespace MVCview.Controllers
{
    public class ReportsController : Controller
    {
        public async Task<IActionResult> Reports()
        {
            var token = HttpContext.Session.GetString("token");

            ReportDto report = new();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync(
                    "https://localhost:7050/api/reports/dashboard");

                if (response.IsSuccessStatusCode)
                {
                    report = await response.Content
                        .ReadFromJsonAsync<ReportDto>();
                }
            }

            return View(report);
        }
    }
}