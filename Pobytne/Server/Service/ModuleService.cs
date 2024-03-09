using Pobytne.Data;
using Pobytne.Data.Tables;
using Pobytne.Shared.Procedural.DTO;
using System.ComponentModel;

namespace Pobytne.Server
{
    public class ModuleService(ModuleTable module)
	{
        private readonly ModuleTable _moduleTable = module;

		public async Task<IEnumerable<Module>> GetModulesByLicense(int licenseNumber)
		{
			return await _moduleTable.GetByLicense(licenseNumber);
		}
		public async Task<IEnumerable<Module>> GetModulesByUser(int userId)
		{
			return await _moduleTable.GetByUser(userId);
		}
        //---------------------------- InsUpDel-------------------------------
        public async Task<int> Update(Module updateModule)
        {
            return await _moduleTable.Update(updateModule);
        }
        public async Task<int?> Insert(Module insertModule)
        {
            return await _moduleTable.Insert(insertModule);
        }
        public async Task<int> Delete(int id)
        {
			//Kontrola na existenci navazujících tabulek
			var errors = await _moduleTable.IsDeletable(id);
            if (errors.Any())
                throw new Exception($"Pro modul {id},který se pokoušíte smazat existuje platný záznam v tabulce {errors}");

            return await _moduleTable.Delete(id);
        }
    }
}
