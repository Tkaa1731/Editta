using Microsoft.AspNetCore.Authorization;
using Pobytne.Client.Extensions;

namespace Pobytne.Client.Authentication
{
    public class PermitionRequirementHandler : AuthorizationHandler<PermitionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermitionRequirement requirement)
        {
            var Resource = context.Resource as (PermitionEnum, AccessEnum,string)?;
            if(Resource is not null)
            {//TODO: possilkat v JWT vsechny pristupy... Pak vyhledat pristup v zavislosti na idModulu
                var permitions = context.User.Claims.FirstOrDefault(c => c.Type == "Permitions");// pole mermici

                if (permitions == null)
                {
				    context.Fail(new(this, "Access denied"));
                    return Task.CompletedTask;
                }

                string permitionString = permitions.Value;


                if (Resource is not null && requirement.GetAccess((Resource.Value.Item1,Resource.Value.Item2), permitionString))
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
