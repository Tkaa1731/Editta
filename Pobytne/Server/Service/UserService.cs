using Pobytne.Data;
using Pobytne.Data.Tables;
using Pobytne.Shared.Authentication;
using Pobytne.Shared.Procedural.DTO;

namespace Pobytne.Server.Service
{
    public class UserService(UserTable user, LicenseTable license)
    {
        private readonly UserTable _userTable = user;
        private readonly LicenseTable _licenseTable = license;

        public async Task<UserAccount> GetAccount(LoginRequest userAccount)
        {
            var user = await _userTable.GetByLogin(userAccount.Name);

            if (user == null || user.Id <= 0) throw new Exception("Neplatné přihlašovací údaje");
            if (!user.CheckPassword(userAccount.Password)) throw new Exception("Špatné heslo");
            if (!await _licenseTable.CheckLicense(user.LicenseNumber)) throw new Exception("Neaktivní licence");

            UserAccount result = new(user);
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
    }// TODO: vyresit heslo
}
