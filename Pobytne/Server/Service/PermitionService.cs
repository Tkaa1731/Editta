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
        public async Task<Permition> GetPermitionById(int id)
        {
            return await _permitionTable.GetById(id);
        }
        //---------------------------- InsUpDel-------------------------------
        public async Task<Permition?> Update(Permition updatePermition)
        {
            //SET Server time
            updatePermition.CreationDate = DateTime.Now;

            var rows = await _permitionTable.Update(updatePermition);
            if (rows > 0)
                return await GetPermitionById(updatePermition.Id);
            return null;
        }
        public async Task<Permition?> Insert(Permition insertPermition)
        {
            //SET Server time
            insertPermition.CreationDate = DateTime.Now;

            var id = await _permitionTable.Insert(insertPermition);
            if (id.HasValue)
                return await GetPermitionById(id.Value);
            return null;
        }
		public async Task<int> Delete(int id)
		{
            return await _permitionTable.Delete(id);
		}
	}
}
