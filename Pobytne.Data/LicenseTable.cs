using Pobytne.Shared.Authentication;
using Pobytne.Shared.Procedural;
using System.Data;

namespace Pobytne.Data
{
    public static class LicenseTable
    {
        private static string _tableName = "S_Licence";
        public async static Task<List<License>> GetAll()
        {
            string sql = $"SELECT * FROM {_tableName};";
            List<License> result = await Database.Select<License>(sql, ParseData);
            return result;
        }
        public async static Task<License> CheckLicense(int license_numb)
        {
            string sql = $"SELECT * FROM {_tableName} WHERE CisloLicence={license_numb};";
            List<License> result = await Database.Select<License>(sql, ParseData);
            return result.FirstOrDefault();
        }
        private static License ParseData(IDataRecord row, License license)
        {
            license.Id = row.GetInt32(0);
            license.LicenseNumber = row.GetInt32(1);
            license.ICO = row.GetInt32(2);
            license.NameOfOrganization = row.GetString(3);
            license.Version = row.GetInt32(10);
            license.Demo = row.GetBoolean(11);
            license.IsPaid = row.GetBoolean(12);
            license.IsBlocked = row.GetBoolean(13);
            return license;
        }
    }
}
