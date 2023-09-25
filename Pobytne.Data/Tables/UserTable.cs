using Pobytne.Shared.Procedural;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace Pobytne.Data.Tables
{
    public class UserTable : IDataTable<User>
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
        public async Task<bool> Insert(User user, int editorId)
        {
            user.CreationUserId = editorId;
            using (IDbConnection cnn = new SqlConnection(Tools.GetConnectionString()))
            {
                var rowsAffected = await cnn.InsertAsync(user);
                if (rowsAffected > 0) { return true; }
                return false;
            }
        }
        public async Task<IEnumerable<User>> Select()
        {
            throw new NotImplementedException();
        }
        public async Task<User?> Update(User user, int editorId)
        {
            user.CreationUserId = editorId;
            using (IDbConnection cnn = new SqlConnection(Tools.GetConnectionString()))
            {
                await cnn.UpdateAsync(user);
                var result = await GetByLogin(user.UserLogin);
                return result;
            }
        }
        public async Task<int> Delete(int idLogin)
        {
            using (IDbConnection cnn = new SqlConnection(Tools.GetConnectionString()))
            {
                var rowsAffected = await cnn.DeleteAsync<User>(new { IDLogin = idLogin });
                return rowsAffected;
            }
        }


    }
}
