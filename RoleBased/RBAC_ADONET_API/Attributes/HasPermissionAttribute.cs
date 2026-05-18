using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RBAC_ADONET_API.Repositories;
using System.Security.Claims;

namespace RBAC_ADONET_API.Attributes
{
    public class HasPermissionAttribute
        : Attribute, IAuthorizationFilter
    {
        private readonly string permission;

        public HasPermissionAttribute(string permission)
        {
            this.permission = permission;
        }

        public void OnAuthorization(
            AuthorizationFilterContext context)
        {
            var repo =
                context.HttpContext.RequestServices
                .GetService<IUserRepository>();

            var user =
                context.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                context.Result =
                    new UnauthorizedResult();

                return;
            }

            var userId =
                int.Parse(
                    user.FindFirst(
                        ClaimTypes.NameIdentifier)?.Value);

            var permissions =
                repo.GetPermissions(userId);

            var hasPermission =
                permissions.Any(x =>
                    x.PermissionName == permission);

            if (!hasPermission)
            {
                context.Result =
                    new ForbidResult();
            }
        }
    }
}