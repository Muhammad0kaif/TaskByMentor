using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PocoClasses;
using PocoClasses.Dto;
using RBAC.Data.Data;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly JwtService jwt;

        public AuthController(AppDbContext context, JwtService jwt)
        {
            this.context = context;
            this.jwt = jwt;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            var user = context.Users
                .FirstOrDefault(x => x.Email == dto.Email && x.Password == dto.Password);

            if (user == null)
                return Unauthorized("Invalid credentials");

            var role = context.Roles.FirstOrDefault(r => r.Id == user.RoleId);

            if (role == null)
                return Unauthorized("Role not found");

            var token = jwt.GenerateToken(user.Id, role.RoleName);

            return Ok(new
            {
                token,
                role = role.RoleName,
                userId = user.Id
            });
        }


        [Authorize]
        [HttpGet("get-users")]
        public IActionResult GetUsers()
        {
            return Ok(context.Users.Select(x => new UserDto
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                RoleId = x.RoleId
            }).ToList());
        }

        [Authorize(Roles = "Admin")] 
        [HttpGet("admin-data")]
        public IActionResult AdminOnly()
        {
            return Ok(context.Users.Select(x => new UserDto
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                RoleId = x.RoleId
            }).ToList());
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("create-user")]
        public IActionResult CreateUser(UserDto dto)
        {
            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = dto.Password,
                RoleId = dto.RoleId
            };

            context.Users.Add(user);
            context.SaveChanges();

            return Ok("User Created");
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("update-user/{id}")]
        public IActionResult UpdateUser(int id, UserDto model)
        {
            var user = context.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            user.Email = model.Email;
            user.Password = model.Password;
            user.RoleId = model.RoleId;

            context.SaveChanges();

            return Ok("User Updated");
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-user/{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = context.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            context.Users.Remove(user);

            context.SaveChanges();

            return Ok("User Deleted");
        }



        [HttpPost("register")]
        public IActionResult Register(RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var exists = context.Users.Any(x => x.Email == model.Email);

            if (exists)
                return BadRequest("User already exists");

            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password,
                RoleId = 2 //  user
            };

            context.Users.Add(user);
            context.SaveChanges();

            return Ok("User Registered");
        }

        [HttpGet("roles")]
        public IActionResult GetRoles()
        {
            return Ok(context.Roles.ToList());
        }


        [Authorize]
        [HttpPut("update-profile/{id}")]
        public IActionResult UpdateProfile(int id, UserDto dto)
        {
            var user = context.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
                return NotFound();

            user.Name = dto.Name;
            user.Email = dto.Email;
            user.Password = dto.Password;

            context.SaveChanges();

            return Ok("Profile Updated");
        }

        [Authorize]
        [HttpPost("change-password/{id}")]
        public IActionResult ChangePassword(int id, ChangePasswordDto dto)
        {
            var user = context.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
                return NotFound();

            if (user.Password != dto.OldPassword)
                return BadRequest("Old password is incorrect");

            user.Password = dto.NewPassword;

            context.SaveChanges();

            return Ok("Password changed successfully");
        }
    }
}
