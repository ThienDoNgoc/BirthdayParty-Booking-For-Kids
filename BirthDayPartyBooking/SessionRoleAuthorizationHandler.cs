using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Linq;
using System;
using BusinessObject.Enum;

namespace BirthDayPartyBooking
{
    public class SessionRoleAuthorizationHandler : AuthorizationHandler<RolesAuthorizationRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionRoleAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RolesAuthorizationRequirement requirement)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var role = httpContext.Session.GetInt32("Role");

            if (role != null)
            {
                if (requirement.AllowedRoles.Contains(UserRole.Role[role.Value]))
                    context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
