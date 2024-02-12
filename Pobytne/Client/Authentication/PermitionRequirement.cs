using Microsoft.AspNetCore.Authorization;
using Pobytne.Shared.Struct;

namespace Pobytne.Client.Authentication
{
	internal class PermitionRequirement : IAuthorizationRequirement
    {
        public bool GetAccess((EPermition,EAccess) acces, string permition)
        {
            int permitionIndex = ((int)acces.Item1);
            char accessValue = ((char)acces.Item2);
            return permition[permitionIndex] >= accessValue;
        }
    }
}
