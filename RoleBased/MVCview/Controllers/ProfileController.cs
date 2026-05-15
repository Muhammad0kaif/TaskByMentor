using Microsoft.AspNetCore.Mvc;

using PocoClasses.Dto;

using System.Net.Http.Headers;

namespace MVCview.Controllers
{
    public class ProfileController : Controller
    {
        public async Task<IActionResult> Profile()
        {
            var token = HttpContext.Session.GetString("token");
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            UserDto user = null;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync("https://localhost:7050/api/auth/get-users");

                if (response.IsSuccessStatusCode)
                {
                    var users = await response.Content
                        .ReadFromJsonAsync<List<UserDto>>();

                    user = users.FirstOrDefault(x => x.Id == userId);
                }
            }

            if (user == null)
                return RedirectToAction("Login", "Account");

            return View(user);
        }

        public async Task<IActionResult> Edit()
        {
            var token = HttpContext.Session.GetString("token");
            var userId = HttpContext.Session.GetInt32("UserId");

            UserDto user = new();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync(
                    "https://localhost:7050/api/auth/get-users");

                if (response.IsSuccessStatusCode)
                {
                    var users = await response.Content
                        .ReadFromJsonAsync<List<UserDto>>();

                    user = users.FirstOrDefault(x => x.Id == userId);
                }
            }

            return View(user);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(UserDto model)
        {
            var token = HttpContext.Session.GetString("token");

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await client.PutAsJsonAsync(
                    $"https://localhost:7050/api/auth/update-profile/{model.Id}",
                    model);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Edit");
                }

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Profile updated successfully!";
                    return RedirectToAction("Edit");
                }
            }


            ViewBag.Error = "Update Failed";
            return View(model);
        }


        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.NewPassword != model.ConfirmPassword)
            {
                ViewBag.Error = "Password and Confirm Password do not match";
                return View();
            }

            var token = HttpContext.Session.GetString("token");
            var userId = HttpContext.Session.GetInt32("UserId");

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await client.PostAsJsonAsync(
                    $"https://localhost:7050/api/auth/change-password/{userId}",
                    model);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Password changed successfully!";
                    return RedirectToAction("ChangePassword");
                }

                ViewBag.Error = "Failed to change password";
            }

            return View(model);
        }
    }
}
