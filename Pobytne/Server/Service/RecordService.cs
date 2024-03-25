using Pobytne.Client.Pages.ModulePages;
using Pobytne.Data;
using Pobytne.Data.Tables;
using Pobytne.Shared.Procedural;
using Pobytne.Shared.Struct;
using System.Data;

namespace Pobytne.Server.Service
{
    public class RecordService(RecordTable recordTable,RecordAttributeTable attributeTable,RecordStockTable stockTable)
    {
        private readonly RecordTable _recordTable = recordTable;
        private readonly RecordAttributeTable _attributeTable = attributeTable;
        private readonly RecordStockTable _stockTable = stockTable;

        public async Task<IEnumerable<Record>> GetBranch(int parentId)
        {
            List<int> parameter = [parentId];
            return await _recordTable.GetSubRecords(new { IDParent = parameter});
        }
        public async Task<IEnumerable<Record>> GetRoot(int moduleId)
        {
            return await _recordTable.GetRoot(new { ModuleID = moduleId});
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
        public async Task<IEnumerable<Record>> GetSeasonTickets(int moduleId, DateTime validTo)
        {
            return await _recordTable.GetSeasonTicketRecords(new {ModuleId = moduleId, ValidTo =  validTo});
        }
        public async Task<int> GetMaxDepth(int moduleId)
        {
            return await _recordTable.GetMaxDepth(new { ModuleID = moduleId });
        }

 
        public async Task<IEnumerable<RecordStock>> GetStockByRecord(int recordId)
        {
            return await _stockTable.GetStockByRecord(recordId);
        }
        //---------------------------- InsUpDel-------------------------------
        public async Task<RecordStock?> Insert(RecordStock insertStock)
        {
			//SET Server time
			insertStock.CreationDate = DateTime.Now;

            int stockId;

            using IDbConnection cnn = Database.CreateConnection();
			cnn.Open();
			using ( var tran = cnn.BeginTransaction() )
            {
                try
                {
                    var insertedStock = await _stockTable.Insert(insertStock, tran);
                    if (!insertedStock.HasValue)
                        throw new Exception("Nastala chyba při vložení záznamu.");

                    stockId = insertedStock.Value;
                    var rows = await _recordTable.UpdateStock(insertStock.RecordId, insertStock.Quantity, tran);
                    if(rows != 1)
						throw new Exception("Nastala chyba při aktualizaci zůstatku.");

					tran.Commit();
                }
                catch ( Exception ex )
                {
                    tran.Rollback();
					throw;
				}
            }
            cnn.Close();
            return await _stockTable.GetById(stockId);
        }
		public async Task UpdateStock(int recordId, int stock, IDbTransaction tran)
		{
			var rows = await _recordTable.UpdateStock(recordId, stock, tran);
			if (rows != 1)
				throw new Exception("Nastala chyba při aktualizaci zůstatku.");
		}
		public async Task<Record?> Update(Record updateRecord)
        {
            //SET Server time
            updateRecord.CreationDate = DateTime.Now;

            //kontrola pouziti zaznamu mimo rozsah platnosti
            var dateRange = await _recordTable.GetUsedDateRange(updateRecord.Id);
            if(dateRange.Item1.HasValue && dateRange.Item2.HasValue)
                if (dateRange.Item1.Value < updateRecord.ValidFrom || dateRange.Item2.Value > updateRecord.ValidTo)
                    throw new Exception("Záznam byl použit mimo období plastnosti. Opravte platnost záznamu!");

            var rows = await _recordTable.Update(updateRecord);
            if(rows > 0)
                return await _recordTable.GetById(updateRecord.Id);
            return null;
        }
        public async Task<Record?> Insert(Record insertRecord)
        {
            //SET Server time
            insertRecord.CreationDate = DateTime.Now;

            var id = await _recordTable.Insert(insertRecord);
            if(id.HasValue)
                return await _recordTable.GetById(id.Value);
            return null;
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
                if (isDeletable.Any())
                    throw new Exception("Záznam byl již použit v systému, a proto jej není možno smazat!");


                // if WARE and kontrolaNaZustatek and zustate>0
                if (record.RecordType == ERecordType.Ware && record.IsBalanceCheck && record.Stock > 0)
                    throw new Exception("Nelze smazat naskladněný záznam.");
            }
            return await _recordTable.Delete(id);
        }
    }
}
