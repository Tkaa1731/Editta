using Pobytne.Data;
using Pobytne.Shared.Authentication;
using Pobytne.Shared.Procedural;

namespace Pobytne.Server.Service
{
    public class UserService
    {

        public async Task<UserAccount> GetAccount(LoginRequest userAccount)
        {
            var result = await UserTable.Login(userAccount.Name);
            if (result == null) throw new Exception("UserAccount not found");
            if (!result.CheckPassword(userAccount.Password)) throw new Exception("Wrong password");

            var license = await LicenseTable.CheckLicense(result.LicenseNumber);
            if (!license.IsPaid) throw new Exception("Your license is now unavalible");

            var accessPermition = await PermitionTable.GetAllOfUser(result.Id);
            if (accessPermition == null) throw new Exception("User does not have any valid AccessPermition");

            result.AccessPermition = accessPermition;
            return result;
        }
        public async Task<List<User>> GetUsers(long licenseNumber)
        {
            var users = UserTable.GetAll(licenseNumber).Result;
            foreach(var u in users)
                u.AccessPermition = await PermitionTable.GetAllOfUser(u.Id);
            return users;
        }
    }
}
