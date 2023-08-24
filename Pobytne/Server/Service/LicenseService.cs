using Pobytne.Data;
using Pobytne.Shared.Procedural;

namespace Pobytne.Server
{
	public class LicenseService
	{
		public async Task<List<License>> GetLicenses()
		{
			var licenses = LicenseTable.GetAll().Result;
			return licenses;
		}
	}
}
