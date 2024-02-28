using Pobytne.Data.Tables;
using Pobytne.Shared.Procedural;
using Pobytne.Shared.Struct;

namespace Pobytne.Server.Service
{
    public class RecordService
    {
        private readonly RecordTable _table;
        public RecordService(RecordTable table)
        {
            _table = table;
        }
        public async Task<IEnumerable<Record>> GetBranch(int parentId)
        {
            List<int> parameter = [parentId];

            var records = await _table.GetSubRecords(new { ParentID = parameter});
            return records;
        }
        public async Task<IEnumerable<Record>> GetRoot(int moduleId)
        {
            var records = await _table.GetRoot(new { ModuleID = moduleId});
            return records;
        }
        public async Task<int> GetMaxDepth(int moduleId)
        {
            var depth = await _table.GetMaxDepth(new { ModuleID = moduleId });
            return depth;
        }
        public async Task<IEnumerable<int>> GetAllSubRecords(List<int> pivot)
        {
            List<int> result = [];
            while (true)
            {
                if(pivot.Count <= 0)
                    break;
                var records = await _table.GetSubRecords(new { ParentID = pivot });
                result.AddRange(records.Where(w => w.RecordType != ERecordType.Folder).Select(s => s.Id));

                pivot.Clear();
                pivot.AddRange(records.Where(w => w.RecordType == ERecordType.Folder).Select(s => s.Id));
            }
            return result;
		}
    }
}
