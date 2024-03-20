using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Components;
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
        public async Task<License> GetLicenseById(int id)
        {
            return await _licenseTable.GetById(id);
        }
        //---------------------------- InsUpDel-------------------------------
        public async Task<License?> Update(License updateLicense)
        {
            //SET Server time
            updateLicense.CreationDate = DateTime.Now;

            var rows = await _licenseTable.Update(updateLicense);
            if (rows > 0)
                return await GetLicenseById(updateLicense.Id);
            return null;
        }
        public async Task<License?> Insert(License insertLicense)
        {
            //SET Server time
            insertLicense.CreationDate = DateTime.Now;

            var id = await _licenseTable.Insert(insertLicense);
            if (id.HasValue)
                return await GetLicenseById(id.Value);
            return null;
        }
        public async Task<int> Delete(int id)
        {
            //Kontrola na existenci navazujících tabulek
            var errors = await _licenseTable.IsDeletable(id);
            if (errors.Any())
                throw new Exception($"Pro licenci ID:{id},kterou se pokoušíte smazat existuje platný záznam v tabulce {errors.First().Error}");

			return await _licenseTable.Delete(id);
        }
    }
}
