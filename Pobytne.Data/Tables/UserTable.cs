using System.Data;
using Dapper;
using Dapper.Transaction;
using Pobytne.Shared.Procedural.DTO;

namespace Pobytne.Data.Tables
{
    public class UserTable //: IDataTable<User>
    {
        public async Task<User> GetByLogin(string loginUser)
        {
            using (IDbConnection cnn = Database.CreateConnection())
            {
                string sql = @"SELECT o.* ,m.Nazev AS ModuleName, u.* 
                               FROM S_LoginUser u
                               LEFT JOIN S_Opravneni o ON u.IDLogin = o.IDLogin
                               LEFT JOIN S_Moduly m ON o.IDModulu = m.IDModulu
                               WHERE u.LoginUser = @LoginUser;";
                var conditions = new { LoginUser = loginUser };

                User user = new() { Id = -1};
                var permitions = await cnn.QueryAsync<Permition, User, Permition>(sql, (p, u) => {
                    user = u;
                    return p;
                }, conditions,
                splitOn: "IDLogin");
                
                user.AccessPermition = permitions.ToList();
                return user;
            }
        }
        public async Task<User> GetById(int idLogin)
        {
            using (IDbConnection cnn = Database.CreateConnection())
            {
                string sql = @"select u.*, c.JmenoUser AS CreationUserName
                               from S_LoginUser u
                               join S_LoginUser c ON u.Kdo = c.IDLogin
                               where u.IDLogin = @IDLogin;";

                var conditions = new { IDLogin = idLogin };

                return await cnn.QueryFirstAsync<User>(sql, conditions);
            }
        }
        public async Task<IEnumerable<User>> GetByLicense(int license)
        {
            using (IDbConnection cnn = Database.CreateConnection())
            {
                string sql = @"select u.*, c.JmenoUser AS CreationUserName
                               from S_LoginUser u
                               JOIN S_LoginUser c ON u.Kdo = c.IDLogin
                               where u.CisloLicence = @License;";
                var conditions = new { License = license };
                return await cnn.QueryAsync<User>(sql, conditions);
            }
        }
        public async Task<int> GetCount(object conditions)
        {
            using IDbConnection cnn = Database.CreateConnection();
            return await cnn.RecordCountAsync<User>(conditions);
        }
        public async Task<IEnumerable<User>> GetByLicenseExsModule(int idModulu)
        {
            using (IDbConnection cnn = Database.CreateConnection())
            {
                string sql = @"SELECT *
                            FROM S_LoginUser u
                            WHERE u.CisloLicence IN(SELECT CisloLicence FROM S_Moduly WHERE IDModulu = @IDModulu) AND u.IDLogin NOT IN (SELECT u.IDLogin
                            FROM S_LoginUser u
                            JOIN S_Opravneni o ON o.IDLogin = u.IDLogin
                            WHERE o.IDModulu = @IDModulu);";

                var users = await cnn.QueryAsync<User>(sql, new { IDModulu = idModulu });
                return users;
            }
        }
		public async Task<int?> Insert(User user)
        {
            using IDbConnection cnn = Database.CreateConnection();
            return await cnn.InsertAsync(user);
        }
        public async Task<int> Update(User user)
        {
            using IDbConnection cnn = Database.CreateConnection();
            return await cnn.UpdateAsync(user);
        }
    }
}
