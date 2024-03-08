using Pobytne.Data;
using Pobytne.Data.Tables;
using Pobytne.Shared.Procedural;
using Pobytne.Shared.Struct;
using System.Data;

namespace Pobytne.Server.Service
{
    public class RecordService(RecordTable recordTable)
    {
        private readonly RecordTable _recordTable = recordTable;

        public async Task<IEnumerable<Record>> GetBranch(int parentId)
        {
            List<int> parameter = [parentId];
            try
            {
                var records = await _recordTable.GetSubRecords(new { IDParent = parameter});
                return records.OrderBy(r => r.Order);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return [];
            }
        }
        public async Task<IEnumerable<Record>> GetRoot(int moduleId)
        {
            var records = await _recordTable.GetRoot(new { ModuleID = moduleId});
            return records.OrderBy(r => r.Order);
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
        public async Task<IEnumerable<RecordAttribute>> GetAttibutesByModule(int moduleId)
        {
            try
            {
                var attributes = await _recordTable.GetAttributesByModule(moduleId);
                return attributes;

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return [];
            }
        }
        //---------------------------- InsUpDel-------------------------------
        public async Task<int> Update(Record updateRecord)//TODO: DB PROCEDURA
        {
            //SET Server time
            updateRecord.CreationDate = DateTime.Now;

            //kontrola pouziti zaznamu mimo rozsah platnosti
            var dateRange = await _recordTable.GetUsedDateRange(updateRecord.Id);
            if(dateRange.Item1.HasValue && dateRange.Item2.HasValue)
                if (dateRange.Item1.Value < updateRecord.ValidFrom || dateRange.Item2.Value > updateRecord.ValidTo)
                    throw new Exception("Záznam byl použit mimo období plastnosti. Opravte platnost záznamu!");

            return await _recordTable.UpdateRecord(updateRecord);
        }
        public async Task<int> Insert(Record insertRecord)
        {
            //SET Server time
            insertRecord.CreationDate = DateTime.Now;

            using IDbConnection cnn = Database.CreateConnection();
            cnn.Open();
            using (var tran = cnn.BeginTransaction())
            {
                try
                {
                    var insertId = await _recordTable.InsertRecord(insertRecord, tran);
                    if (insertId.HasValue)
                    {
                        insertRecord.Id = insertId.Value;
                        if(insertRecord.RootId == 0)
                            insertRecord.RootId = insertId.Value;

                        if (await _recordTable.InsertRecordStructure(insertRecord, tran) == 1)
                        {
                            tran.Commit();
                            return 2;
                        }
                    }
                    tran.Rollback();
                    throw new Exception("Nepodarilo se vlozit zaznam");
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }
        public async Task<int> Delete(int id)
        {
            var record = await _recordTable.GetById(id);
            // if FOLDER kontrola na podzaznamy
            if(record.RecordType == ERecordType.Folder)
            {
                if (await _recordTable.GetSubRecordCount(id) > 0)
                    throw new Exception("Nelze smazat záznam, který obsahuje další záznamy!");
            }
            // else kontrola na evidenci zaznamu
            else 
            {
                var isDeletable = await _recordTable.IsDeletable(record.Id);
                if (isDeletable.Any()) //Kam predavat data od tabulkach obsahujici zazmamy
                {
                    foreach (var e in isDeletable)
                    {
                        Console.WriteLine($"ERROR: While Delete {record.Name} => ID:{e.Id}, TABLE:{e.Error}");
                    }
                    throw new Exception("Záznam byl již použit v systému, a proto jej není možno smazat!");
                }

                // if WARE and kontrolaNaZustatek and zustate>0
                if (record.RecordType == ERecordType.Ware && record.IsBalanceCheck && record.Stock > 0)
                    throw new Exception("Zůstatek pro položku je větší než nula.");
            }
            return await _recordTable.Delete(id);
        }
    }
}
