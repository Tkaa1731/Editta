using System.Data.SqlClient;
using System.Data;
using Dapper;
using Pobytne.Shared.Procedural.DTO;

namespace Pobytne.Data.Tables
{
    public class ClientTable
    {
        public async Task<IEnumerable<Client>> GetAll(object conditions)
        {
            using (IDbConnection cnn = Database.CreateConnection())
            {
                string sql = @"select TOP 30 cl.*, cr.JmenoUser AS CreationClientName
                               from S_Uzivatele cl
                               JOIN S_LoginUser cr ON cl.Kdo = cr.IDLogin
                               WHERE cl.IDModulu = @ModuleNumber
                               ORDER BY cl.IDUzivatele DESC;";

                return await cnn.QueryAsync<Client>(sql, conditions);
            }
        }
        public async Task<int> GetCount(object conditions)
        {
            using (IDbConnection cnn = Database.CreateConnection())
            {
                return await cnn.RecordCountAsync<Client>(conditions);
            }
        }
		public async Task<int> InsUpTran(DynamicParameters param)
		{
			string cashRegisterSQL = "p_sp_Uzivatele_InsUp";
			using IDbConnection cnn = Database.CreateConnection();
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
        public async Task<int> Delete(int id)
        {
            using IDbConnection cnn = Database.CreateConnection();
            return await cnn.DeleteAsync(id);
        }
    }
}
