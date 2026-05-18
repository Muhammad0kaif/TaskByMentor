using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RBAC_ADONET_API.Attributes;
using RBAC_ADONET_API.DTOs;
using RBAC_ADONET_API.Models;
using RBAC_ADONET_API.Repositories;
using RBAC_ADONET_API.Services;
using System.Security.Claims;

namespace RBAC_ADONET_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository repo;
        private readonly JwtService jwt;

        public AuthController(IUserRepository repo,JwtService jwt)
        {
            this.repo = repo;

            this.jwt = jwt;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            var user =
                repo.Login(dto.Email, dto.Password);

            if (user == null)
            {
                return Unauthorized("Invalid Credentials");
            }

            var role =user.RoleId == 1 ? "Admin": "User";

            var token = jwt.GenerateToken(user.Id, role);

            return Ok(new
            {
                token = token,

                role = role,

                userId = user.Id
            });
        }



        [Authorize]
        [HttpGet("get-users")]
        public IActionResult GetUsers()
        {
            var users = repo.GetUsers();

            return Ok(users);
        }

        [Authorize]
        [HttpPost("create-user")]
        public IActionResult CreateUser(User user)
        {
            repo.CreateUser(user);

            return Ok("User Created");
        }


        [HasPermission("DeleteUser")]
        [HttpGet("admin-data")]
        public IActionResult AdminOnly()
        {
            return Ok("Welcome Admin");
        }


        [Authorize]
        [HttpGet("my-data")]
        public IActionResult MyData()
        {
            var userId =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var role =
                User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new
            {
                UserId = userId,
                Role = role
            });
        }


        [Authorize]
        [HttpGet("my-permissions")]
        public IActionResult MyPermissions()
        {
            var userId =
                int.Parse(
                    User.FindFirst(
                        ClaimTypes.NameIdentifier)?.Value);

            var permissions =
                repo.GetPermissions(userId);

            return Ok(permissions);
        }


        [Authorize]
        [HttpGet("dashboard-report")]
        public IActionResult DashboardReport()
        {
            return Ok(
                repo.GetDashboardReport());
        }
    }
}