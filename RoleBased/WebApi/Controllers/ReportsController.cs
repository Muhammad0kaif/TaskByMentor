using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PocoClasses.Dto;
using RBAC.Data.Data;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext context = context;

        [Authorize]
        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            var report = new ReportDto
            {
                TotalOrders = context.Orders.Count(),

                TotalSales = context.Orders
                    .Sum(x => x.Price * x.Quantity)
            };

            return Ok(report);
        }
    }
}