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
        public async Task<User?> GetById(int idLogin)
        {
            using (IDbConnection cnn = Database.CreateConnection())
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
            using (IDbConnection cnn = Database.CreateConnection())
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
            using (IDbConnection cnn = Database.CreateConnection())
            {
                return await cnn.RecordCountAsync<User>(conditions);
            }
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
		public async Task<int> InsUpTran(DynamicParameters param)
		{
			string cashRegisterSQL = "p_sp_LoginUser_InsUp";
			using IDbConnection cnn = Database.CreateConnection();
            int success = await cnn.ExecuteAsync(cashRegisterSQL, param, commandType: CommandType.StoredProcedure);

			if (success == 1)
				return 1;

			throw new Exception($"Failed 'p_sp_LoginUser_InsUp' {success}");
		}
		public DynamicParameters GetParamsForTrans(User user, bool delete)
		{
			var result = new DynamicParameters();
            result.Add("@IDLoginUser", user.Id, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
			object? template = new
			{
				LoginUser = user.UserLogin,
                JmenoUser = user.Name,
                Helso = user.Password,
                JeHesloInicial = user.PasswordIsInicial,
                CisloLicence = user.LicenseNumber,
				user.Email,
                Telefon = user.PhoneNumber,
                IDUzivatele = user.CustomerId,
				user.Valid,
                PlatiOd = user.ValidFrom,
                PlatiDo = user.ValidTo,
                Kdo = user.CreationUserId,
                Kdy = user.CreationDate,
				Smazat = delete ? 1 : 0,
			};
			result.AddDynamicParams(template);
			return result;
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
        public async Task<int> Delete(int id)
        {
            using IDbConnection cnn = Database.CreateConnection();
            return await cnn.DeleteAsync(id);
        }
    }
}
