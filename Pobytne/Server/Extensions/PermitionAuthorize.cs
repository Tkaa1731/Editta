using Microsoft.AspNetCore.Authorization;
using Pobytne.Shared.Struct;

namespace AuthRequirementsData.Authorization;


class PermissionAuthorizeAttribute : AuthorizeAttribute,IAuthorizationRequirement, IAuthorizationRequirementData
{
	public EPermition Permition { get; }
	public EAccess Access { get; }

	public PermissionAuthorizeAttribute(EPermition permition, EAccess access)
	{
		Permition = permition;
		Access = access;
	}

	public IEnumerable<IAuthorizationRequirement> GetRequirements()
	{
		yield return this;
	}
}
