using Pobytne.Shared.Procedural;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace Pobytne.Data.Tables
{
    public class RecordTable
    {
		public async Task<IEnumerable<Record>> GetSubRecords(object conditions)
		{
			using (IDbConnection cnn = new SqlConnection(Tools.GetConnectionString()))
			{
				string sql = @"SELECT z.*, sz.*, u.JmenoUser AS CreationUserName, zv.Nazev AS RecordPropertiesName
                                FROM S_Zaznamy z
                                JOIN S_StrukturaZaznamu sz ON z.IDZaznamu = sz.IDZaznamu
								LEFT JOIN S_ZaznamyVlastnosti zv ON z.IDZaznamuVlastnosti = zv.IDZaznamuVlastnosti
                                JOIN S_LoginUser u ON z.Kdo = u.IDLogin
                                WHERE sz.IDParent IN @ParentID;";

				return await cnn.QueryAsync<Record>(sql, conditions);
			}
		}
		public async Task<IEnumerable<Record>> GetRoot(object conditions)
        {
            using (IDbConnection cnn = new SqlConnection(Tools.GetConnectionString()))
            {
                string sql = @"SELECT z.*, sz.*, u.JmenoUser AS CreationUserName, zv.Nazev AS RecordPropertiesName
                                FROM S_Zaznamy z
                                JOIN S_StrukturaZaznamu sz ON z.IDZaznamu = sz.IDZaznamu
								LEFT JOIN S_ZaznamyVlastnosti zv ON z.IDZaznamuVlastnosti = zv.IDZaznamuVlastnosti
                                JOIN S_LoginUser u ON z.Kdo = u.IDLogin
                                WHERE sz.IDModulu = @ModuleID  AND sz.IDParent = 0;";

                return await cnn.QueryAsync<Record>(sql, conditions);
            }
        }
        public async Task<int> GetCount(object conditions)
        {
            using (IDbConnection cnn = new SqlConnection(Tools.GetConnectionString()))
            {
                return await cnn.RecordCountAsync<Record>(conditions);
            }
        }
        public async Task<int> GetMaxDepth(object conditions)
        {
            using (IDbConnection cnn = new SqlConnection(Tools.GetConnectionString()))
            {
                string sql = @"SELECT MAX(Uroven)
                                FROM S_StrukturaZaznamu
                                WHERE IDModulu = @ModuleID;";

                return await cnn.QueryFirstAsync<int>(sql,conditions);
            }
        }
		public async Task<int> InsUpTran(DynamicParameters param)
		{
			string cashRegisterSQL = "p_sp_Zaznamy_InsUp";
			using IDbConnection cnn = new SqlConnection(Tools.GetConnectionString());
			int success = await cnn.ExecuteAsync(cashRegisterSQL, param, commandType: CommandType.StoredProcedure);

			if (success == 1)
				return 1;

			throw new Exception($"Failed 'p_sp_Zaznamy_InsUp' {success}");
		}
		public async Task<int?> Insert(Record record)
        {
            using IDbConnection cnn = new SqlConnection(Tools.GetConnectionString());
            return await cnn.InsertAsync(record);
        }
        public async Task<int> Update(Record record)
        {
            using IDbConnection cnn = new SqlConnection(Tools.GetConnectionString());
            return await cnn.UpdateAsync(record);
        }
        public async Task<int> Delete(int id)
        {
            using IDbConnection cnn = new SqlConnection(Tools.GetConnectionString());
            return await cnn.DeleteAsync(id);
        }
    }
}
