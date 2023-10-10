using Pobytne.Data.Tables;
using Pobytne.Shared.Procedural;

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
        public async Task<int> Update(License updateLicense)
        {
            return await _licenseTable.Update(updateLicense);
        }
        public async Task<int?> Insert(License insertLicense)
        {
            return await _licenseTable.Insert(insertLicense);
        }
        public async Task<int> Delete(int it)
        {
            return await _licenseTable.Delete(it);
        }
    }
}
