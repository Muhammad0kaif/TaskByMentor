using RBAC.Data.Data;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Middleware
{
    public class PermissionMiddleware
    {
        private readonly RequestDelegate next;

        public PermissionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context, AppDbContext db)
        {
            var endpoint = context.Request.Path.Value?.ToLower();

            if (endpoint.Contains("auth") || endpoint.Contains("permissions"))
            {
                await next(context);
                return;
            }

            var roleClaim = context.User.FindFirst(ClaimTypes.Role);

            if (roleClaim == null)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }


            var roleName = roleClaim.Value;

            var role = await db.Roles
                .FirstOrDefaultAsync(r => r.RoleName == roleName);

            if (role == null)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Invalid Role");
                return;
            }


            var segments = endpoint.Split('/');

            var pageName = segments.Length >= 3
                ? segments[2]
                : "";


            var hasPermission = await db.Permissions
                .AnyAsync(p =>
                    p.RoleId == role.Id &&
                    p.PageName.ToLower() == pageName &&
                    p.CanRead == true);

            if (!hasPermission)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Access Denied");
                return;
            }

            await next(context);
        }
    }
}
