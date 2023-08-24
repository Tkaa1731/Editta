using Pobytne.Shared.Authentication;
using Pobytne.Shared.Procedural;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pobytne.Data
{
    public static class UserTable//TODO: Predelat s Dapper
    {
        private static readonly string _tableName = "S_LoginUser";
        public async static Task<UserAccount?> Login(string userName)
        {
            string sql = $"SELECT * FROM {_tableName}  WHERE LoginUser = '{userName}';";//TODO: chceck validity of user => exception
            List<UserAccount> result = await Database.Select<UserAccount>(sql, ParseData);
            return result.FirstOrDefault();
        } 
        public async static Task<List<User>> GetAll(long licenseNumber)
        {
            string sql = $"SELECT * FROM {_tableName} WHERE CisloLicence={licenseNumber};";
            List<User> result = await Database.Select<User>(sql, ParseData);
            return result;
        }
        public async static Task<bool> AddUser(User user)
        {
            return true;
        }
        public async static Task<bool> DeleteUser(int user_id)
        {
            return true;
        }
        private static UserAccount ParseData(IDataRecord row, UserAccount userAccount)
        {
            userAccount.Id = row.GetInt32(0);
            userAccount.UserName = row.GetString(1);
            userAccount.Password = row.GetString(3);
            userAccount.LicenseNumber = row.GetInt32(5);
            return userAccount;
        }
        private static User ParseData(IDataRecord row,User user)
        {
            user.Id = row.GetInt32(0);
            user.UserName = row.GetString(1);
            user.FullName = row.GetString(2);
            user.Email = row.GetString(6);
            user.PhoneNumber = row.GetString(7);
            user.LicenseNumber = row.GetInt32(5);
            user.Valid = row.GetBoolean(9);
            user.ValidFrom = row.GetDateTime(10);
            user.ValidTo = row.GetDateTime(11);
            return user;
        }
    }
}
