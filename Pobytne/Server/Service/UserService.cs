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
        public UserService(UserTable user,LicenseTable license, PermitionTable permition) 
        {
            _userTable = user;
            _licenseTable = license;
            _permitionTable = permition;
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
            return users;
        }
        public async Task<IEnumerable<User>> GetUsersExsModule(int moduleNumber)
        {
            return await  _userTable.GetByLicenseExsModule(moduleNumber);
        }
        public async Task<IEnumerable<User>> GetUsersByModule(int moduleId)
        {
            var users = await _userTable.GetWithPermitions(new { IDModulu = moduleId});
            return users;
        }
        public async Task<User> GetUserById(int id)
        {
            var user = await _userTable.GetById(id);
			return user is null ? throw new Exception($"No user with id {id}.") : user;
		}
		public async Task<int> Update(User updateUser)
        {
            return await _userTable.Update(updateUser);
        }
        public async Task<int?> Insert(User insertUser)
        {
            insertUser.Password = GeneratePassword();
            return await _userTable.Insert(insertUser);
        }
        public async Task<int> Delete(int it)
        {
            return await _userTable.Delete(it);
        }
        private string GeneratePassword()
        {
            return "heslo";
        }
    }
}
