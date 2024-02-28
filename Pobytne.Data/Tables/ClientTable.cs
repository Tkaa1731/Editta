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
		public async Task<int> InsUpTran(DynamicParameters param)
		{
			string cashRegisterSQL = "p_sp_Uzivatele_InsUp";
			using IDbConnection cnn = new SqlConnection(Tools.GetConnectionString());
			int success = await cnn.ExecuteAsync(cashRegisterSQL, param, commandType: CommandType.StoredProcedure);

			if (success == 1)
				return 1;

			throw new Exception($"Failed 'p_sp_Uzivatele_InsUp' {success}");
		}
		public DynamicParameters GetParamsForTrans(Client client, bool delete)
		{
			var result = new DynamicParameters();
			result.Add("@IDModulu", client.Id, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
			object? template = new
			{
				//Nazev = module.Name,
				//ZkracenyNazev = module.ModuleShortName,
				//CisloLicence = module.LicenseNumber,
				//TypEvidence = module.EvidenceType,
				//JenUzivatelDleIDModulu = module.OnlyUsersByIdOfModule,
				//Kdo = module.CreationUserId,
				//Kdy = module.CreationDate,

				Smazat = delete ? 1 : 0,
			};
			result.AddDynamicParams(template);
			return result;
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
