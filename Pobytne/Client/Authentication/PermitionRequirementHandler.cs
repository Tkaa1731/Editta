using Microsoft.AspNetCore.Authorization;
using Pobytne.Client.Extensions;
using System.Net.Http;
using System.Security.Claims;

namespace Pobytne.Client.Authentication
{
    public class PermitionRequirementHandler : AuthorizationHandler<PermitionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermitionRequirement requirement)
        {
            var permition = context.User.Claims.FirstOrDefault(c => c.Type == "Permitions");
            if (permition == null)
            {
				Console.WriteLine("Permition is null");
				context.Fail(new(this, "Access denied"));
                Console.WriteLine(context.User.Claims.Count());
                return Task.CompletedTask;
            }

            string permitionString = permition.Value;


            var Resource = context.Resource as (PermitionEnum, AccessEnum)?;
            if (Resource is not null && requirement.GetAccess(Resource.Value, permitionString))
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
