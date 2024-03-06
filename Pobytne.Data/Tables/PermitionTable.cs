﻿using Dapper;
using Pobytne.Shared.Procedural.DTO;
using System.Data;
using System.Data.SqlClient;

namespace Pobytne.Data.Tables
{
    public class PermitionTable //: IDataTable<Permition>
    {
        public async Task<IEnumerable<Permition>> GetAllOfUser(int idLogin)
        {
            using (IDbConnection cnn = Database.CreateConnection())
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
        public async Task<IEnumerable<Permition>> GetAllOfModule(int idModule)
        {
            using (IDbConnection cnn = Database.CreateConnection())
            {
                string sql = @"SELECT m.Nazev AS ModuleName,o.*,l.JmenoUser AS CreationUserName, u.JmenoUser AS UserName
                                from S_Opravneni o
                                JOIN S_Moduly m ON m.IDModulu = o.IDModulu
                                JOIN S_LoginUser u ON u.IDLogin = o.IDLogin 
                                JOIN S_LoginUser l ON l.IDLogin = o.Kdo
                                WHERE o.IDModulu = @IDModulu;";
                var condition = new { IDModulu = idModule };
                return await cnn.QueryAsync<Permition>(sql, condition);
            }
        }
        public async Task<int> InsUpTran(DynamicParameters param)
		{
			string cashRegisterSQL = "p_sp_Opravneni_InsUp";
			using IDbConnection cnn = Database.CreateConnection();
			int success = await cnn.ExecuteAsync(cashRegisterSQL, param, commandType: CommandType.StoredProcedure);

			if (success == 1)
				return 1;

			throw new Exception($"Failed 'p_sp_Opravneni_InsUp' {success}");
		}
		public async Task<int?> Insert(Permition item)
        {
            using IDbConnection cnn = Database.CreateConnection();
            return await cnn.InsertAsync(item);
        }
        public async Task<int> Update(Permition item)
        {
            using IDbConnection cnn = Database.CreateConnection();
            return await cnn.UpdateAsync(item);
        }
        public async Task<int> Delete(int id)
        {
            using IDbConnection cnn = Database.CreateConnection();
            return await cnn.DeleteAsync(id);
        }
    }
}
