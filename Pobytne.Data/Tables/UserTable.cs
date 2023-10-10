using Pobytne.Shared.Procedural;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using System.Transactions;

namespace Pobytne.Data.Tables
{
    public class UserTable //: IDataTable<User>
    {
        public async Task<User?> GetByLogin(string loginUser)
        {
            using (IDbConnection cnn = new SqlConnection(Tools.GetConnectionString()))
            {
                string sql = @"select u.*, c.JmenoUser AS CreationUserName
                               from S_LoginUser u
                               join S_LoginUser c ON u.Kdo = c.IDLogin
                               where u.LoginUser = @LoginUser;";
                var conditions = new { LoginUser = loginUser };
                var users = await cnn.QueryAsync<User>(sql, conditions);
                return users.FirstOrDefault();
            }
        }
        public async Task<User?> GetById(int idLogin)
        {
            using (IDbConnection cnn = new SqlConnection(Tools.GetConnectionString()))
            {
                string sql = @"select u.*, c.JmenoUser AS CreationUserName
                               from S_LoginUser u
                               join S_LoginUser c ON u.Kdo = c.IDLogin
                               where u.IDLogin = @IDLogin;";

                var conditions = new { IDLogin = idLogin };
                var users = await cnn.QueryAsync<User>(sql, conditions);
                return users.FirstOrDefault();
            }
        }
        public async Task<IEnumerable<User>> GetAll(object conditions)
        {
            using (IDbConnection cnn = new SqlConnection(Tools.GetConnectionString()))
            {
                string sql = @"select u.*, c.JmenoUser AS CreationUserName
                               from S_LoginUser u
                               JOIN S_LoginUser c ON u.Kdo = c.IDLogin
                               where u.CisloLicence = @LicenseNumber;";

                return await cnn.QueryAsync<User>(sql, conditions);
            }
        }
        public async Task<int> GetCount(object conditions)
        {
            using (IDbConnection cnn = new SqlConnection(Tools.GetConnectionString()))
            {
                return await cnn.RecordCountAsync<User>(conditions);
            }
        }
        public async Task<IEnumerable<User>> GetWithPermitions(object conditions)
        {
            using (IDbConnection cnn = new SqlConnection(Tools.GetConnectionString()))
            {
                string sql = @"SELECT * 
                                FROM S_LoginUser u
                                JOIN S_Opravneni o ON u.IDLogin = o. IDLogin
                                WHERE o.IDModulu = @IDModulu;";

                var users = await cnn.QueryAsync<User,Permition,User>(sql, (u, p) => {
                    u.AccessPermition = new List<Permition>() { p };
                    return u;
                },conditions,
                splitOn: "IDOpravneni");
                return users;
            }
        }
        public async Task<int?> Insert(User user)
        {
            using IDbConnection cnn = new SqlConnection(Tools.GetConnectionString());
            return await cnn.InsertAsync(user);
        }
        public async Task<int> Update(User user)
        {
            using IDbConnection cnn = new SqlConnection(Tools.GetConnectionString());
            return await cnn.UpdateAsync(user);
        }
        public async Task<int> Delete(int id)
        {
            using IDbConnection cnn = new SqlConnection(Tools.GetConnectionString());
            return await cnn.DeleteAsync(id);
        }
    }
}
