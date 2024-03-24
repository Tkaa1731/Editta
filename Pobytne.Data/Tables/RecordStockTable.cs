using Dapper;
using Pobytne.Shared.Procedural;
using System.Data;

namespace Pobytne.Data.Tables
{
    public class RecordStockTable
    {
        public async Task<IEnumerable<RecordStock>> GetStockByRecord(int recordId)
        {
            using IDbConnection cnn = Database.CreateConnection();
            string sql = @"  SELECT pe.*, lu.JmenoUser AS CreationUserName
                              FROM P_PohybyEvidence pe
                              JOIN S_LoginUser lu ON lu.IDLogin = pe.Kdo
                              WHERE IDZaznamu = @IDZaznamu
                              ORDER BY pe.Datum DESC;";
            return await cnn.QueryAsync<RecordStock>(sql, new { IDZaznamu = recordId });
        }
        public async Task<RecordStock> GetById(int id)
        {
            using IDbConnection cnn = Database.CreateConnection();
            string sql = @"SELECT pe.*, lu.JmenoUser AS CreationUserName
                              FROM P_PohybyEvidence pe
                              JOIN S_LoginUser lu ON lu.IDLogin = pe.Kdo
                              WHERE pe.IDPohybu = @IDPohybu;";
            var conditions = new { IDPohybu = id };

            return await cnn.QueryFirstAsync<RecordStock>(sql, conditions);
        }
        public async Task<int?> Insert(RecordStock stock, IDbTransaction tran)
        {
            return await tran.Connection.InsertAsync(stock,transaction:tran);
        }
    }
}
