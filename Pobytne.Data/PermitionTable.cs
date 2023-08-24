using Pobytne.Shared.Procedural;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pobytne.Data
{
    public static class PermitionTable
    {
        private static readonly string _tableName = "S_Opravneni";
        public async static Task<List<Permition>> GetAllOfUser(int user_id)
        {
            string sql = $"SELECT IDOpravneni,o.IDModulu,IDLogin,Nazev,Opravneni,o.Kdo,o.Kdy FROM {_tableName} o JOIN S_Moduly m ON o.IDModulu = m.IDModulu WHERE IDLogin={user_id};";
            var permitions = await Database.Select<Permition>(sql, ParseData);
            return permitions;
        }
        public static Permition ParseData(IDataRecord row, Permition permition)
        {
            permition.Id = row.GetInt32(0);
            permition.ModuleId = row.GetInt32(1);
            permition.UserId = row.GetInt32(2);
            permition.ModuleName = row.GetString(3);
            permition.PermitionString = row.GetString(4);
            permition.EditorId = row.GetInt32(5);
            permition.LastChange = row.GetDateTime(6);
            return permition;
        }
    }
}
