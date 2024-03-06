using Pobytne.Data.Tables;
using Pobytne.Shared.Procedural.DTO;
using System.Reflection;


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
        public async Task<int> Delete(int it)
        {
            return await _clientTable.Delete(it);
        }
    }
}
