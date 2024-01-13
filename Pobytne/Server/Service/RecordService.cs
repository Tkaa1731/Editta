using Pobytne.Data.Tables;
using Pobytne.Shared.Procedural;

namespace Pobytne.Server.Service
{
    public class RecordService
    {
        private readonly RecordTable _table;
        public RecordService(RecordTable table)
        {
            _table = table;
        }
        public async Task<IEnumerable<Record>> GetRecords(int parentId)
        {
            var records = await _table.GetBranch(new { ParentID = parentId});
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
    }
}
