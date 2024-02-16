using Microsoft.AspNetCore.Authorization;
using Pobytne.Shared.Struct;
using System.Security.Claims;

namespace Pobytne.Client.Authentication
{
    internal class PermitionRequirementHandler : AuthorizationHandler<PermitionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermitionRequirement requirement)
        {
            var Resource = context.Resource as (EPermition, EAccess,string)?;
            if(Resource is not null)  
            {
                Claim? permitions;
                if (Resource.Value.Item3 == "-1")
                    permitions = context.User.Claims.First();
                else
                    permitions = context.User.Claims.FirstOrDefault(c => c.Type == Resource.Value.Item3);//podle modulu

                if (permitions == null)
                {
                    context.Fail(new(this, "Access denied"));
                    return Task.CompletedTask;
                }

                string permitionString = permitions.Value;


                if (requirement.GetAccess((Resource.Value.Item1,Resource.Value.Item2), permitionString))
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail(new(this, "Access denied"));
                }
            }

            return Task.CompletedTask;
        }
    }
}
