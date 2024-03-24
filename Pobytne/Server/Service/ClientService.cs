using Pobytne.Data.Tables;
using Pobytne.Shared.Struct;
using ClientDTO = Pobytne.Shared.Procedural.DTO.Client;


namespace Pobytne.Server.Service
{
	public class ClientService(ClientTable clientTable)
    {
        
        private readonly ClientTable _clientTable = clientTable;

        public async Task<IEnumerable<ClientDTO>> Get(int ModuleId, LazyList filter)
        {
            var conditions = new {
                ModuleId,
                filter.StartIndex,
                filter.Count,
                Valid = filter.Active ? 0 : -1,
                filter.Subfix
            };

            return await _clientTable.GetList(conditions);
        }
        public async Task<int> GetCount(int ModuleId, LazyList filter)
        {
            var conditions = new
            {
                ModuleId,
                Valid = filter.Active ? 0 : -1,
                filter.Subfix
            };
            return await _clientTable.GetCount(conditions);
        }
        public async Task<ClientDTO> GetClientById(int id)
        {
            return await _clientTable.GetById(id);
        }
        //---------------------------- InsUpDel-------------------------------
        public async Task<ClientDTO?> Update(ClientDTO updateClient)
        {
            //SET Server time
            updateClient.CreationDate = DateTime.Now;

            var rows = await _clientTable.Update(updateClient);
            if(rows > 0)
                return await GetClientById(updateClient.Id);
            return null;
        }
        public async Task<ClientDTO?> Insert(ClientDTO insertLicense)
        {
            //SET Server time
            insertLicense.CreationDate = DateTime.Now;

            var id = await _clientTable.Insert(insertLicense);
            if (id.HasValue)
                return await GetClientById(id.Value);
            return null;
        }
        public async Task<int> Delete(int id)
        {
			//Kontrola na existenci navazujících tabulek
			var errors = await _clientTable.IsDeletable(id);//TODO: DEPENDECIE GRAPH FORM DB
			if (errors.Any())
				throw new Exception($"Pro typ platby ID:{id},který se pokoušíte smazat existuje platný záznam v tabulce {errors.First().Error}");
			return await _clientTable.Delete(id);
        }
    }
}
