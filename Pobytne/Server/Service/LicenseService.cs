using Pobytne.Data;
using Pobytne.Data.Tables;
using Pobytne.Shared.Procedural;
using System.Collections;

namespace Pobytne.Server
{
	public class LicenseService
	{
        private readonly UserTable _userTable;
        private readonly LicenseTable _licenseTable;
        private readonly PermitionTable _permitionTable;
        private readonly ModuleTable _moduleTable;
        public LicenseService(UserTable user, LicenseTable license, PermitionTable permition, ModuleTable module)
        {
            _userTable = user;
            _licenseTable = license;
            _permitionTable = permition;
            _moduleTable = module;
        }
        public async Task<IEnumerable<License>> GetLicenses()
		{
			return await _licenseTable.GetAll(new {});
		}
	}
}
