using Microsoft.AspNetCore.Mvc;
using MVCview.Models;
using System.Net.Http.Headers;


namespace MVCview.Controllers
{
    public class OrdersController : Controller
    {
        public async Task<IActionResult> Orders(int page = 1)
        {
            var token = HttpContext.Session.GetString("token");

            PagedResult<OrderDto> result = new();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync(
                    $"https://localhost:7050/api/orders/get-orders?page={page}&pageSize=5");

                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content
                        .ReadFromJsonAsync<PagedResult<OrderDto>>();
                }
            }

            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling(result.TotalCount / 5.0);
            if (result == null)
            {
                result = new PagedResult<OrderDto>
                {
                    Items = new List<OrderDto>(),
                    TotalCount = 0
                };
            }
            return View(result?.Items ?? new List<OrderDto>());
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Create(OrderDto model)
        {
            model.UserId = HttpContext.Session.GetInt32("UserId") ?? 0;

            var token = HttpContext.Session.GetString("token");

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await client.PostAsJsonAsync(
                    "https://localhost:7050/api/orders/create-order",
                    model);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Orders");
                }
            }

            ViewBag.Error = "Order Create Failed";

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var token = HttpContext.Session.GetString("token");

            List<OrderDto> orders = new();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync(
                    "https://localhost:7050/api/orders/get-orders");

                if (response.IsSuccessStatusCode)
                {
                    orders = await response.Content
                        .ReadFromJsonAsync<List<OrderDto>>();
                }
            }

            var order = orders.FirstOrDefault(x => x.Id == id);

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(OrderDto model)
        {
            var token = HttpContext.Session.GetString("token");

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await client.PutAsJsonAsync(
                    $"https://localhost:7050/api/orders/update-order/{model.Id}",
                    model);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Orders");
                }
            }

            ViewBag.Error = "Update Failed";

            return View(model);
        }
    }
}
