using Pobytne.Data.Tables;
using Pobytne.Shared.Procedural;
using Pobytne.Shared.Struct;

namespace Pobytne.Server.Service
{
    public class RecordService(RecordTable recordTable)
    {
        private readonly RecordTable _recordTable = recordTable;

        public async Task<IEnumerable<Record>> GetBranch(int parentId)
        {
            List<int> parameter = [parentId];

            var records = await _recordTable.GetSubRecords(new { ParentID = parameter});
            return records;
        }
        public async Task<IEnumerable<Record>> GetRoot(int moduleId)
        {
            var records = await _recordTable.GetRoot(new { ModuleID = moduleId});
            return records;
        }
        public async Task<int> GetMaxDepth(int moduleId)
        {
            var depth = await _recordTable.GetMaxDepth(new { ModuleID = moduleId });
            return depth;
        }
        public async Task<IEnumerable<int>> GetAllSubRecords(List<int> pivot)
        {
            List<int> result = [];
            while (true)
            {
                if(pivot.Count <= 0)
                    break;
                var records = await _recordTable.GetSubRecords(new { ParentID = pivot });
                result.AddRange(records.Where(w => w.RecordType != ERecordType.Folder).Select(s => s.Id));

                pivot.Clear();
                pivot.AddRange(records.Where(w => w.RecordType == ERecordType.Folder).Select(s => s.Id));
            }
            return result;
		}
        //---------------------------- InsUpDel-------------------------------
        public async Task<int> Update(Record updateRecord)
        {
            return await _recordTable.Update(updateRecord);
        }
        public async Task<int?> Insert(Record insertRecord)
        {
            return await _recordTable.Insert(insertRecord);
        }
        public async Task<int> Delete(int it)
        {
            return await _recordTable.Delete(it);
        }
    }
}
