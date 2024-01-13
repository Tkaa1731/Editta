using Pobytne.Data.Tables;
using Pobytne.Shared;

namespace Pobytne.Server.Service
{
    public class ClientService
    {
        
        private readonly ClientTable _clientTable;

        public ClientService(ClientTable clientTable)
        {
            _clientTable = clientTable;
        }

        public async Task<IEnumerable<Shared.Procedural.Client>> GetClientsByModule(int moduleNumber)
        {
            var users = await _clientTable.GetAll(new { ModuleNumber = moduleNumber});
            return users;
        }
    }
}
