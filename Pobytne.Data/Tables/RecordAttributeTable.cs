using Dapper;
using Pobytne.Shared.Extensions;
using Pobytne.Shared.Procedural;
using Pobytne.Shared.Procedural.DTO;
using System.Data;

namespace Pobytne.Data.Tables
{
    public class RecordAttributeTable
    {
        public async Task<IEnumerable<RecordAttribute>> GetAttributesByModule(int moduleId)
        {
            using (IDbConnection cnn = Database.CreateConnection())
            {
                string sql = @"SELECT zv.*, lu.JmenoUser AS CreationUserName
                                FROM S_ZaznamyVlastnosti zv
                                JOIN S_LoginUser lu ON lu.IDLogin = zv.Kdo
                                WHERE zv.IDModulu = @IDModulu;";

                return await cnn.QueryAsync<RecordAttribute>(sql, new { IDModulu = moduleId });
            }
        }
        public async Task<RecordAttribute> GetById(int id)
        {
            using IDbConnection cnn = Database.CreateConnection();
            string sql = @"SELECT zv.*, lu.JmenoUser AS CreationUserName
                            FROM S_ZaznamyVlastnosti zv
                            JOIN S_LoginUser lu ON lu.IDLogin = zv.Kdo
                            WHERE zv.IDZaznamuVlastnosti = @IDZaznamuVlastnosti;";

            var conditions = new { IDZaznamuVlastnosti = id };

            return await cnn.QueryFirstAsync<RecordAttribute>(sql, conditions);
        }
		public async Task<int> GetCount(object conditions)
		{
			using IDbConnection cnn = Database.CreateConnection();
			string sql = @"SELECT COUNT(IDZaznamuVlastnosti)
                               FROM S_ZaznamyVlastnosti
                               WHERE IDModulu = @ModuleId;";

			return await cnn.ExecuteScalarAsync<int>(sql, conditions);
		}
		// ------------------ InsUp ---------------------
		public async Task<int> Update(RecordAttribute attribute)
        {
            using IDbConnection cnn = Database.CreateConnection();
            return await cnn.UpdateAsync(attribute);
        }
        public async Task<int?> Insert(RecordAttribute attribute)
        {
            using IDbConnection cnn = Database.CreateConnection();
            return await cnn.InsertAsync(attribute);
        }

        public async Task<IEnumerable<DeleteError>> IsDeletable(int id)
        {
            using IDbConnection cnn = Database.CreateConnection();//TODO: oprav sql
            var sql = @"SELECT * FROM (
                        SELECT 10 as Id, 'Zaznamy' as Error FROM S_Zaznamy WHERE IDZaznamuVlastnosti = @ID 
						) as ByloPouzito;";
            var conditions = new { ID = id };
            return await cnn.QueryAsync<DeleteError>(sql, conditions);
        }

        public async Task<int> Delete(int id)
        {
            using IDbConnection cnn = Database.CreateConnection();
            return await cnn.DeleteAsync<RecordAttribute>(id);
        }
    }
}
