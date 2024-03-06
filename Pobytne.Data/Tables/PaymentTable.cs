using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Dapper;
using Pobytne.Shared.Procedural.DTO;

namespace Pobytne.Data.Tables
{
    public class PaymentTable
    {
        public async Task<IEnumerable<Payment>> GetPayments(int moduleId)
        {
            var conditions = new { IDModulu = moduleId };
            using (IDbConnection cnn = Database.CreateConnection())
            {
                string sql = @"  SELECT p.*, u.JmenoUser AS CreationUserName
                                  FROM S_TypyPlatby p
                                  JOIN S_LoginUser u ON u.IDLogin = p.Kdo
                                  WHERE IDModulu = @IDModulu;";

                return await cnn.QueryAsync<Payment>(sql, conditions);
            }
        }
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
        public async Task<int> Delete(int id)
        {
            using IDbConnection cnn = Database.CreateConnection();
            return await cnn.DeleteAsync(id);
        }
    }
}
