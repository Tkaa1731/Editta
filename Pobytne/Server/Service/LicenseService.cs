using Pobytne.Data.Tables;
using Pobytne.Shared.Procedural.DTO;

namespace Pobytne.Server
{
    public class LicenseService(LicenseTable license)
    {
        private readonly LicenseTable _licenseTable = license;

        public async Task<IEnumerable<License>> GetLicenses()
		{
			return await _licenseTable.GetAll(new {});
		}
        //---------------------------- InsUpDel-------------------------------
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
