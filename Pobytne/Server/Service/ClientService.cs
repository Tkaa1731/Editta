using DocumentFormat.OpenXml.Office2010.Excel;
using Pobytne.Data.Tables;


namespace Pobytne.Server.Service
{
	public class ClientService(ClientTable clientTable)
    {
        
        private readonly ClientTable _clientTable = clientTable;

        public async Task<IEnumerable<Shared.Procedural.DTO.Client>> GetClientsByModule(int moduleNumber)
        {
            return await _clientTable.GetAll(new { ModuleNumber = moduleNumber});
        }
        //---------------------------- InsUpDel-------------------------------
        public async Task<int> Update(Shared.Procedural.DTO.Client updateLicense)
        {
            return await _clientTable.Update(updateLicense);
        }
        public async Task<int?> Insert(Shared.Procedural.DTO.Client insertLicense)
        {
            return await _clientTable.Insert(insertLicense);
        }
        public async Task<int> Delete(int id)
        {
			//Kontrola na existenci navazujících tabulek
			var errors = await _clientTable.IsDeletable(id);//TODO: DEPENDECIE GRAPH FORM DB
			if (errors.Any())
				throw new Exception($"Pro typ platby ID:{id},který se pokoušíte smazat existuje platný záznam v tabulce {errors}");
			return await _clientTable.Delete(id);
        }
    }
}
