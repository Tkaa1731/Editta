using Pobytne.Shared.Procedural;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Pobytne.Shared.Extensions;
using Dapper.Transaction;

namespace Pobytne.Data.Tables
{
    public class RecordTable
    {
		public async Task<IEnumerable<Record>> GetSubRecords(object conditions)
		{
			using (IDbConnection cnn = Database.CreateConnection())
			{
				string sql = @"SELECT z.*, sz.*, u.JmenoUser AS CreationUserName, zv.Nazev AS RecordAttributeName
                                FROM S_Zaznamy z
                                JOIN S_StrukturaZaznamu sz ON z.IDZaznamu = sz.IDZaznamu
								LEFT JOIN S_ZaznamyVlastnosti zv ON z.IDZaznamuVlastnosti = zv.IDZaznamuVlastnosti
                                JOIN S_LoginUser u ON z.Kdo = u.IDLogin
                                WHERE sz.IDParent IN @IDParent;";

				return await cnn.QueryAsync<Record>(sql, conditions);
			}
		}
		public async Task<IEnumerable<Record>> GetRoot(object conditions)
        {
            using (IDbConnection cnn = Database.CreateConnection())
            {
                string sql = @"SELECT z.*, sz.*, u.JmenoUser AS CreationUserName, zv.Nazev AS RecordAttributeName
                                FROM S_Zaznamy z
                                JOIN S_StrukturaZaznamu sz ON z.IDZaznamu = sz.IDZaznamu
								LEFT JOIN S_ZaznamyVlastnosti zv ON z.IDZaznamuVlastnosti = zv.IDZaznamuVlastnosti
                                JOIN S_LoginUser u ON z.Kdo = u.IDLogin
                                WHERE sz.IDModulu = @ModuleID  AND sz.IDParent = 0;";

                return await cnn.QueryAsync<Record>(sql, conditions);
            }
        }
		public async Task<IEnumerable<RecordAttribute>> GetAttributesByModule(int moduleId)
		{
			using (IDbConnection cnn = Database.CreateConnection())
			    {
				string sql = @"SELECT zv.*, lu.JmenoUser AS CreationUserName
                                FROM S_ZaznamyVlastnosti zv
                                JOIN S_LoginUser lu ON lu.IDLogin = zv.Kdo
                                WHERE zv.IDModulu = @IDModulu;";

				return await cnn.QueryAsync<RecordAttribute>(sql, new{ IDModulu = moduleId });
			}
		}
		public async Task<int> GetSubRecordCount(int parentId)
        {
            using (IDbConnection cnn = Database.CreateConnection())
            {
                string sql = @"SELECT COUNT(*)
                                FROM S_Zaznamy z
                                JOIN S_StrukturaZaznamu sz ON z.IDZaznamu = sz.IDZaznamu
                                WHERE sz.IDParent = @IDParent;";
                var conditions = new { IDParent = parentId };
                return await cnn.ExecuteScalarAsync<int>(sql,conditions);
            }
        }
        public async Task<int> GetMaxDepth(object conditions)
        {
            using (IDbConnection cnn = Database.CreateConnection())
            {
                string sql = @"SELECT MAX(Uroven)
                                FROM S_StrukturaZaznamu
                                WHERE IDModulu = @ModuleID;";

                return await cnn.ExecuteScalarAsync<int>(sql,conditions);
            }
        }
        public async Task<Record> GetById(int id)
        {
            using IDbConnection cnn = Database.CreateConnection();
            return await cnn.GetAsync<Record>(id);
        }
        public async Task<int> Update(RecordAttribute attribute){
			using IDbConnection cnn = Database.CreateConnection();
			return await cnn.UpdateAsync(attribute);
		}
        public async Task<int> Update(Record record)
        {
            using IDbConnection cnn = Database.CreateConnection();
            return await cnn.UpdateAsync(record);   
        }
        public async Task<int?> Insert(RecordAttribute attribute)
        {
            using IDbConnection cnn = Database.CreateConnection(); 
            return await cnn.InsertAsync(attribute);
        }
        public async Task<int?> Insert(Record record)
        {

			using IDbConnection cnn = Database.CreateConnection();
			cnn.Open();
			using (var tran = cnn.BeginTransaction())
			{
				try
				{
					var insertId = await cnn.InsertAsync(record, tran);
					if (insertId.HasValue)
					{
                        //Set values for structure
						record.Id = insertId.Value;
						if (record.RootId == 0)
							record.RootId = insertId.Value;


                        var sql = @"INSERT INTO S_StrukturaZaznamu (IDZaznamu, IDModulu, IDParent, IDRoot, Uroven)
                        VALUES  (@IDZaznamu, @IDModulu, @IDParent, @IDRoot, @Uroven)";
						var conditions = new { IDZaznamu = record.Id, IDModulu = record.ModuleId, IDParent = record.ParentId, IDRoot = record.RootId, Uroven = record.StructDepth };

							
						if (await tran.ExecuteAsync(sql, conditions) == 1)
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
        public async Task<(DateTime?,DateTime?)> GetUsedDateRange(int id)
        {
            using IDbConnection cnn = Database.CreateConnection();
            var sql = @"SELECT MIN(MinDate) as MinDate, MAX(MaxDate) as MaxDate
                        FROM (SELECT MIN(a.Datum) as MinDate, MAX(a.Datum) as MaxDate
                                FROM P_Interakce a
                                JOIN P_Pokladna b on b.IDInterakce = a.IDInterakce
                                WHERE b.IDZaznamu = @IDZaznamu 
                                UNION
                              SELECT MIN(a.Datum) as MinDate, MAX(a.Datum) as MaxDate
                                FROM P_Interakce a
                                JOIN P_Evidence b on b.IDInterakce = a.IDInterakce
                                WHERE b.IDZaznamu = @IDZaznamu 
                                UNION
                              SELECT MIN(Datum) as MinDate, MAX(Datum) as MaxDate
                                FROM P_PohybyEvidence
                                WHERE IDZaznamu = @IDZaznamu ) as DateRange;";
            var conditions = new { IDZaznamu = id };
            var dateRange = await cnn.QueryFirstAsync(sql, conditions);
            return (dateRange.MinDate, dateRange.MaxDate);
        }
        public async Task<IEnumerable<DeleteError>> IsDeletable(int id)
        {
            using IDbConnection cnn = Database.CreateConnection();
            var sql = @"SELECT * FROM (
                        SELECT 1 as Id, 'StrukturaZaznamů' as Error FROM S_StrukturaZaznamu WHERE IDZaznamu = @IDZaznamu UNION  
                        SELECT 2 as Id, 'Pokladna' as Error FROM P_Pokladna WHERE IDZaznamu = @IDZaznamu UNION 
                        SELECT 3 as Id, 'Evidence' as Error FROM P_Evidence WHERE IDZaznamu = @IDZaznamu UNION
                        SELECT 4 as Id, 'PohybyEvidence' as Error FROM P_PohybyEvidence WHERE IDZaznamu = @IDZaznamu UNION
                        SELECT 5 as Id, 'Permanentky' as Error FROM P_Permanentka WHERE IDZaznamu = @IDZaznamu) as ByloPouzito;";
            var conditions = new {IDZaznamu = id};
            return await cnn.QueryAsync<DeleteError>(sql, conditions);
        } 
        public async Task<int> Delete(int id)
        {
            using IDbConnection cnn = Database.CreateConnection();
            cnn.Open();
            using (var tran = cnn.BeginTransaction())
            {
                var deleted = 0;
                deleted += await tran.ExecuteAsync("DELETE S_StrukturaZaznamu WHERE IDZaznamu = @IDZaznamu;", new { IDZaznamu = id });
                deleted += await cnn.DeleteAsync<Record>(id,tran);
                if(deleted != 2)
                    tran.Rollback();
                else
                    tran.Commit();
                return deleted;
            }
        }
    }
}
