using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PocoClasses;
using PocoClasses.Dto;
using RBAC.Data.Data;
using System.Security.Claims;

namespace WebApi.Controllers
{
        [Route("api/[controller]")]
        [ApiController]
        public class OrdersController(AppDbContext context) : ControllerBase
        {
            private readonly AppDbContext context = context;

            

            [Authorize(Roles = "Admin,User")]
            [HttpPost("create-order")]
            public IActionResult CreateOrder(OrderDto dto)
            {
                var order = new Order
                {
                    ProductName = dto.ProductName,
                    Quantity = dto.Quantity,
                    Price = dto.Price,
                    UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                };

                context.Orders.Add(order);

                context.SaveChanges();

                return Ok("Order Created");
            }

           


            [Authorize]
            [HttpPut("update-order/{id}")]
            public IActionResult UpdateOrder(int id, OrderDto dto)
            {
                var order = context.Orders.FirstOrDefault(x => x.Id == id);

                if (order == null)
                {
                    return NotFound();
                }

                order.ProductName = dto.ProductName;
                order.Quantity = dto.Quantity;
                order.Price = dto.Price;

                context.SaveChanges();

                return Ok("Order Updated");
            }


            [Authorize]
            [HttpGet("get-orders")]
            public IActionResult GetOrders(int page = 1, int pageSize = 5)
            {
                var role = User.FindFirst(ClaimTypes.Role)?.Value;

                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var query = context.Orders.AsQueryable();

                if (role != "Admin")
                {
                    query = query.Where(x => x.UserId == userId);
                }

                var totalCount = query.Count();

                var orders = query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return Ok(new PagedResult<Order>
                {
                    TotalCount = totalCount,
                    Items = orders
                });
            }

        }
}

