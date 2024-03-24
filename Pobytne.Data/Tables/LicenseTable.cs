using System.Data;
using Dapper;
using Pobytne.Shared.Extensions;
using Pobytne.Shared.Procedural.DTO;

namespace Pobytne.Data.Tables
{
	public class LicenseTable //: IDataTable<License>
    {
        public async Task<IEnumerable<License>> GetAll()
        {
			using IDbConnection cnn = Database.CreateConnection();
			string sql = @"select l.*, c.JmenoUser AS CreationUserName
                               from S_Licence l
                               JOIN S_LoginUser c ON l.Kdo = c.IDLogin;";
			return await cnn.QueryAsync<License>(sql);
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
        public async Task<License> GetById(int id)
        {
			using IDbConnection cnn = Database.CreateConnection();
			return await cnn.GetAsync<License>(id);

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
		public async Task<IEnumerable<DeleteError>> IsDeletable(int licenseId)
		{
			using IDbConnection cnn = Database.CreateConnection();
			var sql = @"SELECT * FROM (
                        SELECT 3 as Id, 'Moduly' as Error FROM S_Moduly m JOIN S_Licence l ON m.CisloLicence = l.CisloLicence WHERE l.IDLicence = @ID UNION  
                        SELECT 9 as Id, 'Uživatelé' as Error FROM S_LoginUser u JOIN S_Licence l ON l.CisloLicence = u.CisloLicence WHERE l.IDLicence = @ID UNION
                        SELECT 34 as Id, 'OSPOD' as Error FROM S_OSPOD o WHERE o.IDLicence = @ID) as ByloPouzito;";

			var conditions = new { ID = licenseId };
			return await cnn.QueryAsync<DeleteError>(sql, conditions);
		}
		public async Task<int> Delete(int id)
        {
            using IDbConnection cnn = Database.CreateConnection();
            return await cnn.DeleteAsync<License>(id);
        }
    }
}
