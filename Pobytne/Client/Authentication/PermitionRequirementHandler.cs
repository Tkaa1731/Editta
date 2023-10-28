using Microsoft.AspNetCore.Authorization;
using Pobytne.Client.Extensions;

namespace Pobytne.Client.Authentication
{
    public class PermitionRequirementHandler : AuthorizationHandler<PermitionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermitionRequirement requirement)
        {
            var permition = context.User.Claims.FirstOrDefault(c => c.Type == "Permitions");
            if (permition == null)
            {
                context.Fail(new(this, "Access denied"));
                Console.WriteLine(context.User.Claims.Count());
                return Task.CompletedTask;
            }

            string permitionString = permition.Value;

            Console.WriteLine(permition);
            var Resource = context.Resource as Tuple<PermitionEnum, AccessEnum>;
            if (Resource is not null && requirement.GetAccess(Resource, permitionString))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail(new(this, "Access denied"));
            }

            return Task.CompletedTask;
        }
    }
}
