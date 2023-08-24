using Pobytne.Shared.Procedural;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pobytne.Data
{
    public class ModuleTable
    {
        private static string _tableName = "S_Moduly";
        public async static Task<List<Module>> GetAll(long licenseNumber)
        {
            string sql = $"SELECT * FROM {_tableName} WHERE CisloLicence={licenseNumber};";
            List<Module> result = await Database.Select<Module>(sql, ParseData);
            return result;
        }
        private static Module ParseData(IDataRecord row, Module module)
        {
            module.Id = row.GetInt32(0);
            module.Name = row.GetString(1);
            module.ShortName = row.GetString(2);
            module.LicenseNumber = row.GetInt32(3);
            module.EvidenceType = row.GetByte(4);
            return module;
        }
    }
}
