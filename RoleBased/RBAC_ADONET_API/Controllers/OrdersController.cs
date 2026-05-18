using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RBAC_ADONET_API.Models;
using RBAC_ADONET_API.Repositories;
using System.Security.Claims;

namespace RBAC_ADONET_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IUserRepository repo;

        public OrdersController(IUserRepository repo)
        {
            this.repo = repo;
        }
        [Authorize]
        [HttpGet("get-orders")]
        public IActionResult GetOrders(
            int pageNumber = 1,
            int pageSize = 5)
        {
            return Ok(
                repo.GetOrdersPaged(
                    pageNumber,
                    pageSize));
        }


        [Authorize]
        [HttpPost("create-order")]
        public IActionResult CreateOrder(Order order)
        {
            repo.CreateOrder(order);

            return Ok("Order Created");
        }

        [Authorize]
        [HttpPut("update-order")]
        public IActionResult UpdateOrder(Order order)
        {
            repo.UpdateOrder(order);

            return Ok("Order Updated");
        }

        [Authorize]
        [HttpDelete("delete-order/{id}")]
        public IActionResult DeleteOrder(int id)
        {
            repo.DeleteOrder(id);

            return Ok("Order Deleted");
        }

        [Authorize]
        [HttpGet("search-orders")]
        public IActionResult SearchOrders( string? search,decimal? minPrice, decimal? maxPrice)
        {
            var orders =
                repo.SearchOrders(
                    search,
                    minPrice,
                    maxPrice);

            return Ok(orders);
        }
    }
}