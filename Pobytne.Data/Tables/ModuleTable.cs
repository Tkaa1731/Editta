using Pobytne.Shared.Procedural;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace Pobytne.Data.Tables
{
    public class ModuleTable
    {

        public async Task<IEnumerable<Module>> GetAll(object condition)
        {
            using (IDbConnection cnn = new SqlConnection(Tools.GetConnectionString()))
            {
                string sql = @"select m.*, c.JmenoUser AS CreationUserName
                               from S_Moduly m
                               JOIN S_LoginUser c ON m.Kdo = c.IDLogin
                               WHERE m.CisloLicence = @CisloLicence;";

                return await cnn.QueryAsync<Module>(sql,condition);
            }
		}
		public async Task<IEnumerable<Module>> GetByUser(int userId)
		{
			using (IDbConnection cnn = new SqlConnection(Tools.GetConnectionString()))
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
            using (IDbConnection cnn = new SqlConnection(Tools.GetConnectionString()))
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
		public async Task<int> InsUpTran(DynamicParameters param)
		{
			string cashRegisterSQL = "p_sp_Moduly_InsUp";
			using IDbConnection cnn = new SqlConnection(Tools.GetConnectionString());
			int success = await cnn.ExecuteAsync(cashRegisterSQL, param, commandType: CommandType.StoredProcedure);

			if (success == 1)
				return 1;

			throw new Exception($"Failed 'p_sp_Moduly_InsUp' {success}");
		}
		public DynamicParameters GetParamsForTrans(Module module, bool delete)
		{
			var result = new DynamicParameters();
			result.Add("@IDModulu", module.Id, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
			object? template = new
			{
				Nazev = module.Name,
				ZkracenyNazev = module.ModuleShortName,
				CisloLicence = module.LicenseNumber,
				TypEvidence = module.EvidenceType,
				JenUzivatelDleIDModulu = module.OnlyUsersByIdOfModule,
				Kdo = module.CreationUserId,
				Kdy = module.CreationDate,

				Smazat = delete ? 1 : 0,
			};
			result.AddDynamicParams(template);
			return result;
		}
		public async Task<int?> Insert(Module item)
        {
            using IDbConnection cnn = new SqlConnection(Tools.GetConnectionString());
            return await cnn.InsertAsync(item);
        }

        public async Task<int> Update(Module item)
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
