using Dapper;
using Pobytne.Shared.Procedural;
using System.Data;
using System.Data.SqlClient;

namespace Pobytne.Data.Tables
{
    public class PermitionTable //: IDataTable<Permition>
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
		public async Task<int> InsUpTran(DynamicParameters param)
		{
			string cashRegisterSQL = "p_sp_Opravneni_InsUp";
			using IDbConnection cnn = new SqlConnection(Tools.GetConnectionString());
			int success = await cnn.ExecuteAsync(cashRegisterSQL, param, commandType: CommandType.StoredProcedure);

			if (success == 1)
				return 1;

			throw new Exception($"Failed 'p_sp_Opravneni_InsUp' {success}");
		}
		public async Task<int?> Insert(Permition item)
        {
            using IDbConnection cnn = new SqlConnection(Tools.GetConnectionString());
            return await cnn.InsertAsync(item);
        }
        public async Task<int> Update(Permition item)
        {
            using IDbConnection cnn = new SqlConnection(Tools.GetConnectionString());
            return await cnn.UpdateAsync(item);
        }
        public async Task<int> Delete(int id)
        {
            using IDbConnection cnn = new SqlConnection(Tools.GetConnectionString());
            return await cnn.DeleteAsync(id);
        }
    }
}
