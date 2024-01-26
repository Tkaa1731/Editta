using Pobytne.Shared.Procedural;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace Pobytne.Data.Tables
{
    public class ClientTable
    {
        public async Task<IEnumerable<Client>> GetAll(object conditions)
        {
            using (IDbConnection cnn = new SqlConnection(Tools.GetConnectionString())) // TODO: odstranit omezeni TOP 15
            {
                string sql = @"select TOP 15 cl.*, cr.JmenoUser AS CreationClientName
                               from S_Uzivatele cl
                               JOIN S_LoginUser cr ON cl.Kdo = cr.IDLogin
                               where cl.IDModulu = @ModuleNumber;";

                return await cnn.QueryAsync<Client>(sql, conditions);
            }
        }
        public async Task<int> GetCount(object conditions)
        {
            using (IDbConnection cnn = new SqlConnection(Tools.GetConnectionString()))
            {
                return await cnn.RecordCountAsync<Client>(conditions);
            }
        }
        public async Task<int?> Insert(Client user)
        {
            using IDbConnection cnn = new SqlConnection(Tools.GetConnectionString());
            return await cnn.InsertAsync(user);
        }
        public async Task<int> Update(Client user)
        {
            using IDbConnection cnn = new SqlConnection(Tools.GetConnectionString());
            return await cnn.UpdateAsync(user);
        }
        public async Task<int> Delete(int id)
        {
            using IDbConnection cnn = new SqlConnection(Tools.GetConnectionString());
            return await cnn.DeleteAsync(id);
        }
    }
}
