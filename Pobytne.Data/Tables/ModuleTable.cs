using System.Data.SqlClient;
using System.Data;
using Dapper;
using Pobytne.Shared.Procedural.DTO;
using Pobytne.Shared.Extensions;

namespace Pobytne.Data.Tables
{
    public class ModuleTable
    {

        public async Task<IEnumerable<Module>> GetByLicense(int license)
        {
            using (IDbConnection cnn = Database.CreateConnection())
            {
                string sql = @"select m.*, c.JmenoUser AS CreationUserName
                               from S_Moduly m
                               JOIN S_LoginUser c ON m.Kdo = c.IDLogin
                               WHERE m.CisloLicence = @License;";

				var conditions = new { License = license };
				return await cnn.QueryAsync<Module>(sql,conditions);
            }
		}
		public async Task<IEnumerable<Module>> GetByUser(int userId)
		{
			using (IDbConnection cnn = Database.CreateConnection())
			{
				string sql = @"SELECT m.*
                                FROM S_LoginUser l
                                JOIN S_Opravneni o ON o.IDLogin = l.IDLogin
                                JOIN S_Moduly m ON m.IDModulu = o.IDModulu
                                WHERE l.IDLogin  = @IDLogin;";
				var condition = new { IDLogin = userId };
				return await cnn.QueryAsync<Module>(sql, condition);
			}
		}
		public async Task<Module?> GetById(int id)
        {
            using (IDbConnection cnn = Database.CreateConnection())
            {
                string sql = @"select m.*, c.JmenoUser AS CreationUserName
                               from S_Moduly m
                               JOIN S_LoginUser c ON m.Kdo = c.IDLogin
                               WHERE m.IDModulu = @IDModulu;";
                var condition = new { IDModulu = id };
                var result = await cnn.QueryAsync<Module>(sql, condition);
                return result.FirstOrDefault();
            }
        }
		public async Task<int?> Insert(Module item)
        {
            using IDbConnection cnn = Database.CreateConnection();
            return await cnn.InsertAsync(item);
        }

        public async Task<int> Update(Module item)
        {
            using IDbConnection cnn = Database.CreateConnection();
            return await cnn.UpdateAsync(item);
        }
		public async Task<IEnumerable<DeleteError>> IsDeletable(int id)
		{
			using IDbConnection cnn = Database.CreateConnection();
			var sql = @"SELECT * FROM (
                        --SELECT 4 as Id, 'PohybyPokladna' as Error FROM P_PohybyPokladna p WHERE p.M = @ID UNION
                        SELECT 5 as Id, 'Opravneni' as Error FROM S_Opravneni WHERE IDModulu = @ID UNION
                        SELECT 9 as Id, 'Klienti' as Error FROM S_Uzivatele WHERE IDModulu = @ID UNION  
                        SELECT 10 as Id, 'Zaznamy' as Error FROM S_Zaznamy  WHERE IDModulu = @ID UNION
                        SELECT 14 as Id, 'Interakce' as Error FROM P_Interakce  WHERE IDModulu = @ID
						) as ByloPouzito;";
			var conditions = new { ID = id };
			return await cnn.QueryAsync<DeleteError>(sql, conditions);
		}
		public async Task<int> Delete(int id)
        {
            using IDbConnection cnn = Database.CreateConnection();
            return await cnn.DeleteAsync<Module>(id);
        }
    }
}
