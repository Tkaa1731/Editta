using Dapper;
using Pobytne.Shared.Procedural;
using System.Data;
using System.Data.SqlClient;

namespace Pobytne.Data.Tables
{
    public class PermitionTable : IDataTable<Permition>
    {
        public async Task<IEnumerable<Permition>> GetAll(int idLogin)
        {
            using (IDbConnection cnn = new SqlConnection(Tools.GetConnectionString()))
            {
                string sql = @"SELECT m.Nazev AS ModuleName,o.*,u.JmenoUser AS CreationUserName
                                from S_Opravneni o
                                JOIN S_Moduly m ON m.IDModulu = o.IDModulu
                                JOIN S_LoginUser u ON u.IDLogin = o.IDLogin 
                                WHERE o.IDLogin = @IDLogin;";
                var condition = new { IDLogin = idLogin };
                return await cnn.QueryAsync<Permition>(sql, condition);
            }
        }
        public Task<IEnumerable<Permition>> Select()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Insert(Permition item, int editorId)
        {
            throw new NotImplementedException();
        }


        public Task<Permition?> Update(Permition item, int editorId)
        {
            throw new NotImplementedException();
        }
        public Task<int> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
