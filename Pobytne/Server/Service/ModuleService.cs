using Pobytne.Data;
using Pobytne.Data.Tables;
using Pobytne.Shared.Procedural;

namespace Pobytne.Server
{
	public class ModuleService
	{
        private readonly UserTable _userTable;
        private readonly LicenseTable _licenseTable;
        private readonly PermitionTable _permitionTable;
        private readonly ModuleTable _moduleTable;
        public ModuleService(UserTable user, LicenseTable license, PermitionTable permition, ModuleTable module)
        {
            _userTable = user;
            _licenseTable = license;
            _permitionTable = permition;
            _moduleTable = module;
        }
        public async Task<IEnumerable<Module>> GetModulesByLicense(int licenseNumber)
		{
			return await _moduleTable.GetAll(new{ CisloLicence = licenseNumber});
		}
	}
}
