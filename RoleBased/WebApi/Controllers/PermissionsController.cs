using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RBAC.Data.Data;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly AppDbContext context;

        public PermissionsController(AppDbContext context)
        {
            this.context = context;
        }



        [HttpGet("{roleName}")]
        public IActionResult GetPermissions(string roleName)
        {
            var role = context.Roles
                .FirstOrDefault(r => r.RoleName.ToLower() == roleName.ToLower());

            if (role == null)
                return NotFound();

            var permissions = context.Permissions
                .Where(x => x.RoleId == role.Id)
                .Select(x => new
                {
                    x.PageName,
                    x.CanRead,
                    x.CanWrite
                })
                .ToList();

            return Ok(permissions);
        }
    }
}
