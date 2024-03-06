using System.Data.SqlClient;
using System.Data;
using Dapper;
using System;
using Pobytne.Shared.Procedural.DTO;

namespace Pobytne.Data.Tables
{
    public class LicenseTable //: IDataTable<License>
    {
        public async Task<IEnumerable<License>> GetAll(object condition)
        {
            using (IDbConnection cnn = Database.CreateConnection())
            {
                string sql = @"select l.*, c.JmenoUser AS CreationUserName
                               from S_Licence l
                               JOIN S_LoginUser c ON l.Kdo = c.IDLogin;";
                return await cnn.QueryAsync<License>(sql);
            }
        }
        public async Task<bool> CheckLicense(int license_numb)// checking if license is active
        {
            using (IDbConnection cnn = Database.CreateConnection())
            {
                string sql = @"select * from S_Licence where CisloLicence=@CisloLicence;";
                var param = new { CisloLicence = license_numb };
                var list = await cnn.QueryAsync<License>(sql, param);
                if (list.Count() != 1 || list.First().IsBlocked || !list.First().IsPayed)
                    return false;
                return true;
            }
        }

        public async Task<IEnumerable<License>> Select(int id)
        {
            using (IDbConnection cnn = Database.CreateConnection())
            {
                string sql = @"SELECT * FROM S_Licence WHERE CisloLicence=@CisloLicence;";
                var param = new { CisloLicence = id };
                return  await cnn.QueryAsync<License>(sql, param);
            }
        }
		public async Task<int> InsUpTran(DynamicParameters param)
		{
			string cashRegisterSQL = "p_sp_Licence_InsUp";
			using IDbConnection cnn = Database.CreateConnection();
			int success = await cnn.ExecuteAsync(cashRegisterSQL, param, commandType: CommandType.StoredProcedure);

			if (success == 1)
				return 1;

			throw new Exception($"Failed 'p_sp_Licence_InsUp' {success}");
		}
		public DynamicParameters GetParamsForTrans(License license, bool delete)
		{
			var result = new DynamicParameters();
			result.Add("@IDLicence", license.Id, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
			object? template = new
			{
				CisloLicence = license.LicenseNumber,
				license.ICO,
				NazevOrganizace = license.NameOfOrganization,
				Ulice = license.Street,
				Obec = license.City,
				PSC = license.PostNumber,
				KontaktniOsoba = license.ContactPerson,
				Telefon = license.PhoneNumber,
				license.Email,
				TypVerze = license.VersionType,
				JeDemo = license.IsDemo,
				JeZaplacena = license.IsPayed,
				JeBlokovana = license.IsBlocked,
				DatumVystaveni = license.DateOfLaunch,
				DatumPlatby = license.DateOfPayment,
				PlatiOd = license.ValidFrom,
				PlatiDo = license.ValidTo,
				Kdo = license.CreationUserId,
				Kdy = license.CreationDate,

				Smazat = delete ? 1 : 0,
			};
			result.AddDynamicParams(template);
			return result;
		}
        public async Task<int?> Insert(License license)
        {
            using IDbConnection cnn = Database.CreateConnection();
            return await cnn.InsertAsync(license);
        }

        public async Task<int> Update(License license)
        {
            using IDbConnection cnn = Database.CreateConnection();
            return await cnn.UpdateAsync(license);
        }
		public async Task<int> Delete(int id)
        {
            using IDbConnection cnn = Database.CreateConnection();
            return await cnn.DeleteAsync(id);
        }
    }
}
