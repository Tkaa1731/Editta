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
                               JOIN S_LoginUser c ON m.Kdo = c.IDLogin
                               WHERE m.CisloLicence = @CisloLicence;";

                return await cnn.QueryAsync<Module>(sql,conditions);
            }
        }
        public async Task<IEnumerable<Module>> Select(int id)
        {
            using (IDbConnection cnn = new SqlConnection(Tools.GetConnectionString()))
            {
                string sql = @"select m.*, c.JmenoUser AS CreationUserName
                               from S_Moduly m
                               JOIN S_LoginUser c ON m.Kdo = c.IDLogin
                               WHERE m.IDModulu = @IDModulu;";
                var condition = new{ IDModulu = id };
                return await cnn.QueryAsync<Module>(sql,condition);
            }
        }


        public Task<bool> Insert(Module item)
        {
            throw new NotImplementedException();
        }

        public async Task<Module?> Update(Module item)
        {
            using (IDbConnection cnn = new SqlConnection(Tools.GetConnectionString()))
            {
                await cnn.UpdateAsync(item);
                var result = await Select(item.Id);
                return result.First();
            }
        }
        public Task<int> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Module>> Select()
        {
            throw new NotImplementedException();
        }
    }
}
