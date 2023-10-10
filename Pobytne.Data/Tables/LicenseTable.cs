using Pobytne.Shared.Procedural;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace Pobytne.Data.Tables
{
    public class LicenseTable //: IDataTable<License>
    {
        public async Task<IEnumerable<License>> GetAll(object condition)
        {
            using (IDbConnection cnn = new SqlConnection(Tools.GetConnectionString()))
            {
                string sql = @"select l.*, c.JmenoUser AS CreationUserName
                               from S_Licence l
                               JOIN S_LoginUser c ON l.Kdo = c.IDLogin;";
                return await cnn.QueryAsync<License>(sql);
            }
        }
        public async Task<bool> CheckLicense(int license_numb)// checking if license is active
        {
            using (IDbConnection cnn = new SqlConnection(Tools.GetConnectionString()))
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
            using (IDbConnection cnn = new SqlConnection(Tools.GetConnectionString()))
            {
                string sql = @"SELECT * FROM S_Licence WHERE CisloLicence=@CisloLicence;";
                var param = new { CisloLicence = id };
                return  await cnn.QueryAsync<License>(sql, param);
            }
        }

        public async Task<int> Delete(int id)
        {
            using IDbConnection cnn = new SqlConnection(Tools.GetConnectionString());
            return await cnn.DeleteAsync(id);
        }

        public async Task<int?> Insert(License item)
        {
            using IDbConnection cnn = new SqlConnection(Tools.GetConnectionString());
            return await cnn.InsertAsync(item);
        }

        public async Task<int> Update(License item)
        {
            using IDbConnection cnn = new SqlConnection(Tools.GetConnectionString());
            return await cnn.UpdateAsync(item);
        }
    }
}
