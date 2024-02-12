using AuthRequirementsData.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Primitives;
using Pobytne.Shared.Struct;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Pobytne.Server.Extensions
{
	internal partial class PermitionRequirementHandler : AuthorizationHandler<PermissionAuthorizeAttribute>
    {
		private readonly IHttpContextAccessor _httpContextAccessor;

		public PermitionRequirementHandler(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAuthorizeAttribute requirement)
        {
            var module = _httpContextAccessor.HttpContext?.Request.Headers["module-id"];
            if(module is null)
            {
				context.Fail(new(this, "Access denied"));
				return Task.CompletedTask;
			}
            Claim? permitions;
            if (module.Value == "-1")
                permitions = context.User.Claims.FirstOrDefault(c => MyRegex().IsMatch(c.Type));
            else
                permitions = context.User.Claims.FirstOrDefault(c => c.Type == module.Value);
            
            if (permitions is null)
            {
                context.Fail(new(this, "Access denied"));
                return Task.CompletedTask;
            }
            
            string permitionString = permitions.Value;
            
            
            if (GetAccess((requirement.Permition,requirement.Access), permitionString))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail(new(this, "Access denied"));
            }


            return Task.CompletedTask;
        }
        private static bool GetAccess((EPermition,EAccess) policy, string permition)
        {
			int permitionIndex = ((int)policy.Item1);
			char accessValue = ((char)policy.Item2);
			return permition[permitionIndex] >= accessValue;
		}

        [GeneratedRegex(@"^[1-9]\d*$")]
        private static partial Regex MyRegex();
    }
}
