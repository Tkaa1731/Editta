using Pobytne.Shared.Procedural;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace Pobytne.Data.Tables
{
    public class ModuleTable : IDataTable<Module>
    {

        public async Task<IEnumerable<Module>> GetAll(object conditions)
        {
            using (IDbConnection cnn = new SqlConnection(Tools.GetConnectionString()))
            {
                string sql = @"select m.*, c.JmenoUser AS CreationUserName
                               from S_Moduly m
                               JOIN S_LoginUser c ON m.Kdo = c.IDLogin;";

                return await cnn.QueryAsync<Module>(sql);
            }
        }
        public Task<IEnumerable<Module>> Select()
        {
            throw new NotImplementedException();
        }


        public Task<bool> Insert(Module item, int editorId)
        {
            throw new NotImplementedException();
        }

        public Task<Module?> Update(Module item, int editorId)
        {
            throw new NotImplementedException();
        }
        public Task<int> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
