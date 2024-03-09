using Pobytne.Data.Tables;
using Pobytne.Shared.Procedural.DTO;

namespace Pobytne.Server.Service
{
    public class PermitionService(PermitionTable permitionTable)
    {
        private readonly PermitionTable _permitionTable = permitionTable;

        public async Task<IEnumerable<Permition>> GetAllOfModule(int idModule)
        {
            return await _permitionTable.GetAllOfModule(idModule);
        }
        public async Task<int> Update(Permition updatePermition)
        {
            return await _permitionTable.Update(updatePermition);
        }
        public async Task<int?> Insert(Permition insertPermition)
        {
            return await _permitionTable.Insert(insertPermition);
        }
		public async Task<int> Delete(int it)
		{
			//Kontrola na existenci navazujících tabulek
			return await _permitionTable.Delete(it);
		}
	}
}
