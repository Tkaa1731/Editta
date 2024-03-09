using Pobytne.Data.Tables;
using Pobytne.Shared.Procedural.DTO;

namespace Pobytne.Server
{
    public class LicenseService(LicenseTable licenseTable)
    {
        private readonly LicenseTable _licenseTable = licenseTable;
        public async Task<IEnumerable<License>> GetLicenses()
		{
			return await _licenseTable.GetAll();
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
        public async Task<int> Delete(int id)
        {
            //Kontrola na existenci navazujících tabulek
            var errors = await _licenseTable.IsDeletable(id);
            if (errors.Any())
                throw new Exception($"Pro licenci ID:{id},kterou se pokoušíte smazat existuje platný záznam v tabulce {errors}");

			return await _licenseTable.Delete(id);
        }
    }
}
