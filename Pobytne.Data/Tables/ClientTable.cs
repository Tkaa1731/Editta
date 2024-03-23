using System.Data;
using Dapper;
using Pobytne.Shared.Extensions;
using Pobytne.Shared.Procedural.DTO;

namespace Pobytne.Data.Tables
{
	public class ClientTable
    {
        public async Task<IEnumerable<Client>> GetAll(object conditions)
        {
            using (IDbConnection cnn = Database.CreateConnection())
            {
                string sql = @"SELECT TOP 30 cl.*, cr.JmenoUser AS CreationClientName
                               FROM S_Uzivatele cl
                               JOIN S_LoginUser cr ON cl.Kdo = cr.IDLogin
                               WHERE cl.IDModulu = @ModuleNumber
                               ORDER BY cl.IDUzivatele DESC;";

                return await cnn.QueryAsync<Client>(sql, conditions);
            }
        }
        public async Task<IEnumerable<Client>> GetAllRange(object conditions)
        {
            using (IDbConnection cnn = Database.CreateConnection())
            {
                string sql = @"select TOP 30 cl.*, cr.JmenoUser AS CreationClientName
                               from S_Uzivatele cl
                               JOIN S_LoginUser cr ON cl.Kdo = cr.IDLogin
                               WHERE cl.IDModulu = @ModuleId AND cl.JmenoUzivatele LIKE '@Name%' AND cl.Valid != @Valid 
                               ORDER BY cl.IDUzivatele
                               OFFSET @StartIndex ROWS FETCH NEXT @Count ROWS ONLY;";

                return await cnn.QueryAsync<Client>(sql, conditions);
            }
        }
        public async Task<Client> GetById(int clientId)
        {
            using (IDbConnection cnn = Database.CreateConnection())
            {
                string sql = @"SELECT cl.*, cr.JmenoUser AS CreationClientName
                               FROM S_Uzivatele cl
                               JOIN S_LoginUser cr ON cl.Kdo = cr.IDLogin
                               WHERE cl.IDUzivatele = @IDUzivatele;";

                var conditions = new { IDUzivatele = clientId };

                return await cnn.QueryFirstAsync<Client>(sql, conditions);
            }
        }
        public async Task<int> GetCount(object conditions)
        {
            using (IDbConnection cnn = Database.CreateConnection())
            {
                return await cnn.RecordCountAsync<Client>(conditions);
            }
        }
        //---------------------------- InsUpDel-------------------------------

        public async Task<int?> Insert(Client client)
        {
            using IDbConnection cnn = Database.CreateConnection();
            return await cnn.InsertAsync(client);
        }
        public async Task<int> Update(Client client)
        {
            using IDbConnection cnn = Database.CreateConnection();
            return await cnn.UpdateAsync(client);
        }
		public async Task<IEnumerable<DeleteError>> IsDeletable(int clientId)
		{
			using IDbConnection cnn = Database.CreateConnection();
			var sql = @"SELECT * FROM (
                        SELECT 12 as Id, 'Dohody osoby' as Error FROM P_DohodaOsoba WHERE IDUzivatele = @ID UNION  
                        SELECT 15 as Id, 'Permanentky' as Error FROM P_Permanentka WHERE IDUzivatele = @ID UNION  
                        SELECT 14 as Id, 'Interakce' as Error FROM P_Interakce  WHERE IDUzivatele = @ID
						) as ByloPouzito;";

			var conditions = new { ID = clientId };
			return await cnn.QueryAsync<DeleteError>(sql, conditions);
		}
		public async Task<int> Delete(int id)
        {
            using IDbConnection cnn = Database.CreateConnection();
            return await cnn.DeleteAsync<Client>(id);
        }
    }
}
