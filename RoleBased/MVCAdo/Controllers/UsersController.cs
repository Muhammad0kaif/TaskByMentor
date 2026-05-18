using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net.Http.Headers;
using System.Text.Json;
using PocoClasses;
using MVCAdo.Models;

namespace MVCAdo.Controllers
{
    public class UsersController : Controller
    {
        public async Task<IActionResult> Users()
        {
            var permissionsJson = HttpContext.Session.GetString("permissions");

            if (string.IsNullOrEmpty(permissionsJson))
            {
                return RedirectToAction("Login", "Account");
            }

            var permissions = JsonSerializer.Deserialize<List<PermissionDto>>(permissionsJson);

            var pagePermission = permissions.FirstOrDefault(x => x.PageName.ToLower() == "users");

            if (pagePermission == null || pagePermission.CanWrite == false)
            {
                return RedirectToAction("AccessDenied", "Home");
            }
            ViewBag.Permissions = permissions;

            var token = HttpContext.Session.GetString("token");

            List<UserDto> users = new();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync(
                    "https://localhost:7140/api/auth/get-users");

                if (response.IsSuccessStatusCode)
                {
                    users = await response.Content
                        .ReadFromJsonAsync<List<UserDto>>();
                }
            }

            return View(users);
        }


        [HttpGet]
        public IActionResult Create()
        {
       
            var permissionsJson = HttpContext.Session.GetString("permissions");

            if (string.IsNullOrEmpty(permissionsJson))
                return RedirectToAction("Login", "Account");

            var permissions = JsonSerializer.Deserialize<List<PermissionDto>>(permissionsJson);

            var pagePermission = permissions.FirstOrDefault(x => x.PageName.ToLower() == "users");

            if (pagePermission == null || pagePermission.CanWrite == false)
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            List<Role> roles = new();

            using (var client = new HttpClient())
            {
                var response = client.GetAsync("https://localhost:7140/api/auth/roles").Result;

                if (response.IsSuccessStatusCode)
                {
                    roles = response.Content
                        .ReadFromJsonAsync<List<Role>>().Result;
                }
            }

            ViewBag.Roles = roles;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserDto model)
        {
            var token = HttpContext.Session.GetString("token");

            if (model.Password != model.ConfirmPassword)
            {
                ViewBag.Error = "Password and Confirm Password do not match";
                return View(model);
            }

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await client.PostAsJsonAsync(
                    "https://localhost:7140/api/auth/create-user",
                    model);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Users");
                }
            }

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var token = HttpContext.Session.GetString("token");

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await client.DeleteAsync(
                    $"https://localhost:7140/api/auth/delete-user/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Users");
                }
            }

            return RedirectToAction("Users");
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var token = HttpContext.Session.GetString("token");

            UserDto user = new();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync(
                    "https://localhost:7140/api/auth/get-users");

                if (response.IsSuccessStatusCode)
                {
                    var users = await response.Content
                        .ReadFromJsonAsync<List<UserDto>>();

                    user = users.FirstOrDefault(x => x.Id == id);
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
                    $"https://localhost:7140/api/auth/update-user/{model.Id}",
                    model);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Users");
                }

                ViewBag.Error = "Update Failed";
            }

            return View(model);
        }

    }
}
