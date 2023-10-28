using Microsoft.AspNetCore.Authorization;
using Pobytne.Client.Extensions;
using System.Collections;

namespace Pobytne.Client.Authentication
{
    public class PermitionRequirement : IAuthorizationRequirement
    {
        public bool GetAccess(Tuple<PermitionEnum,AccessEnum> acces, string permition)
        {
            int permitionIndex = ((int)acces.Item1);
            char accessValue = ((char)acces.Item2);
            return permition[permitionIndex] >= accessValue;
        }
    }
}
