using Microsoft.AspNetCore.Components;
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
        public async Task<Module?> GetModuleById(int id)
        {
            return await _moduleTable.GetById(id);
        }
        //---------------------------- InsUpDel-------------------------------
        public async Task<Module?> Update(Module updateModule)
        {
            //SET Server time
            updateModule.CreationDate = DateTime.Now;

            var rows = await _moduleTable.Update(updateModule);
            if(rows > 0)
                return await GetModuleById(updateModule.Id);
            return null;
        }
        public async Task<Module?> Insert(Module insertModule)
        {
            //SET Server time
            insertModule.CreationDate = DateTime.Now;

            var id = await _moduleTable.Insert(insertModule);
            if (id.HasValue)
                return await GetModuleById(id.Value);
            return null;
        }
        public async Task<int> Delete(int id)
        {
			//Kontrola na existenci navazujících tabulek
			var errors = await _moduleTable.IsDeletable(id);
            if (errors.Any())
                throw new Exception($"Pro modul {id},který se pokoušíte smazat existuje platný záznam v tabulce {errors.First().Error}");

            return await _moduleTable.Delete(id);
        }
    }
}
