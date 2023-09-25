using Pobytne.Data;
using Pobytne.Data.Tables;
using Pobytne.Shared.Authentication;
using Pobytne.Shared.Procedural;

namespace Pobytne.Server.Service
{
    public class UserService
    {
        private readonly UserTable _userTable;
        private readonly LicenseTable _licenseTable;
        private readonly PermitionTable _permitionTable;
        private readonly ModuleTable _moduleTable;
        public UserService(UserTable user,LicenseTable license, PermitionTable permition, ModuleTable module) 
        {
            _userTable = user;
            _licenseTable = license;
            _permitionTable = permition;
            _moduleTable = module;
        }

        public async Task<UserAccount> GetAccount(LoginRequest userAccount)
        {
            var user = await _userTable.GetByLogin(userAccount.Name);

            if (user == null) throw new Exception("UserAccount not found");
            if (!user.CheckPassword(userAccount.Password)) throw new Exception("Wrong password");

            if (!await _licenseTable.CheckLicense(user.LicenseNumber)) throw new Exception("Your license is now unavalible");

            //var accessPermition = await PermitionTable.GetAll(new{ UserId = user.Id });
            var accessPermition = await _permitionTable.GetAll(user.Id);

            if (accessPermition.Count() == 0) throw new Exception("User does not have any valid AccessPermition");

            UserAccount result = new(user);
            result.User.AccessPermition = accessPermition.ToList();
            return result;
        }
        public async Task<IEnumerable<User>> GetUsersByLicense(int licenseNumber)
        {
                var users = await _userTable.GetAll(new{ LicenseNumber = licenseNumber});
            foreach(var u in users)
            {
                var per = await _permitionTable.GetAll(u.Id);
                u.AccessPermition = per.ToList(); //await PermitionTable.GetAll(new{ UserId = u.Id });
            }
            return users;
        }
    }
}
