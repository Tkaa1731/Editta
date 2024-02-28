using Pobytne.Shared.Procedural;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Dapper;

namespace Pobytne.Data.Tables
{
    public class PaymentTable
    {
        public async Task<IEnumerable<Payment>> GetPayments(int moduleId)
        {
            var conditions = new { IDModulu = moduleId };
            using (IDbConnection cnn = new SqlConnection(Tools.GetConnectionString()))
            {
                string sql = @"  SELECT p.*, u.JmenoUser AS CreationUserName
                                  FROM S_TypyPlatby p
                                  JOIN S_LoginUser u ON u.IDLogin = p.Kdo
                                  WHERE IDModulu = @IDModulu;";

                return await cnn.QueryAsync<Payment>(sql, conditions);
            }
        }
		public async Task<int> InsUpTran(DynamicParameters param)
		{
			string cashRegisterSQL = "p_sp_TypPlatby_InsUp";
			using IDbConnection cnn = new SqlConnection(Tools.GetConnectionString());
			int success = await cnn.ExecuteAsync(cashRegisterSQL, param, commandType: CommandType.StoredProcedure);

			if (success == 1)
				return 1;

			throw new Exception($"Failed 'p_sp_TypPlatby_InsUp' {success}");
		}
	}
}
