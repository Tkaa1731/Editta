using System.Data;
using Dapper;
using Pobytne.Shared.Extensions;
using Pobytne.Shared.Procedural.DTO;

namespace Pobytne.Data.Tables
{
	public class PaymentTable
    {
        public async Task<IEnumerable<Payment>> GetPayments(int moduleId)
        {
            using (IDbConnection cnn = Database.CreateConnection())
            {
                string sql = @"  SELECT p.*, u.JmenoUser AS CreationUserName
                                  FROM S_TypyPlatby p
                                  JOIN S_LoginUser u ON u.IDLogin = p.Kdo
                                  WHERE IDModulu = @IDModulu;";
                var conditions = new { IDModulu = moduleId };

                return await cnn.QueryAsync<Payment>(sql, conditions);
            }
        }
        public async Task<Payment> GetById(int id)
        {
            using (IDbConnection cnn = Database.CreateConnection())
            {
                string sql = @"  SELECT p.*, u.JmenoUser AS CreationUserName
                                  FROM S_TypyPlatby p
                                  JOIN S_LoginUser u ON u.IDLogin = p.Kdo
                                  WHERE IDTypuPlatby = @IDTypuPlatby;";
                var conditions = new { IDTypuPlatby = id};

                return await cnn.QueryFirstAsync<Payment>(sql, conditions);
            }
        }
		public async Task<int> GetCount(object conditions)
		{
			using IDbConnection cnn = Database.CreateConnection();
			string sql = @"SELECT COUNT(IDTypuPlatby)
                               FROM S_TypyPlatby
                               WHERE IDModulu = @ModuleId;";

			return await cnn.ExecuteScalarAsync<int>(sql, conditions);
		}
		//---------------------------- InsUpDel-------------------------------

		public async Task<int?> Insert(Payment payment)
        {
            using IDbConnection cnn = Database.CreateConnection();
            return await cnn.InsertAsync(payment);
        }
        public async Task<int> Update(Payment payment)
        {
            using IDbConnection cnn = Database.CreateConnection();
            return await cnn.UpdateAsync(payment);
        }
		public async Task<IEnumerable<DeleteError>> IsDeletable(int paymentId)
		{
			using IDbConnection cnn = Database.CreateConnection();
			var sql = @"SELECT * FROM (
                        SELECT 12 as Id, 'PohybyPokladna' as Error FROM P_PohybyPokladna WHERE IDTypuPlatby = @ID UNION  
                        SELECT 14 as Id, 'Interakce' as Error FROM P_Interakce  WHERE IDTypuPlatby = @ID
						) as ByloPouzito;";

			var conditions = new { ID = paymentId };
			return await cnn.QueryAsync<DeleteError>(sql, conditions);
		}
		public async Task<int> Delete(int id)
        {
            using IDbConnection cnn = Database.CreateConnection();
            return await cnn.DeleteAsync<Payment>(id);
        }
    }
}
