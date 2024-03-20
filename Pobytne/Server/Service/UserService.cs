using Microsoft.AspNetCore.Components;
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
            if (!user.CheckPassword(userAccount.Password)) throw new Exception("Neplatné přihlašovací údaje");//TODO:Kontrola Hashe
            //TODO: Load user from other DB
            if (!await _licenseTable.CheckLicense(user.LicenseNumber)) throw new Exception("Neaktivní licence");

            UserAccount result = new(user);
            return result;
        }
        public async Task<IEnumerable<User>> GetUsersByLicense(int licenseNumber)
        {
            return await _userTable.GetByLicense(licenseNumber);
        }
        public async Task<IEnumerable<User>> GetUsersExsModule(int moduleNumber)
        {
            return await  _userTable.GetByLicenseExsModule(moduleNumber);
        }
        public async Task<User?> GetUserById(int id)
        {
            return await _userTable.GetById(id);
		}
		public async Task<User?> Update(User updateUser)
        {
            //SET Server time
            updateUser.CreationDate = DateTime.Now;

            var rows =  await _userTable.Update(updateUser);
            if(rows > 0)
                return await GetUserById(updateUser.Id);
            return null;
        }
        public async Task<User?> Insert(User insertUser)
        {
            //SET Server time
            insertUser.CreationDate = DateTime.Now;

            insertUser.Password = GeneratePassword();
            var id = await _userTable.Insert(insertUser);
            if (id.HasValue)
                return await GetUserById(id.Value);
            return null;
        }
        private string GeneratePassword()
        {
            return "heslo";
        }
    }// TODO: vyresit heslo
}
