using Dapper.FluentMap.Mapping;
using Pobytne.Shared.Procedural;

namespace Pobytne.Data.Mappers
{
    internal class UserMapper : EntityMap<User>
    {
        internal UserMapper() 
        {
            Map(u => u.Id).ToColumn("IDLogin");
            Map(u => u.UserLogin).ToColumn("LoginUser");
            Map(u => u.UserName).ToColumn("JmenoUser");
            Map(u => u.Password).ToColumn("Heslo");
            Map(u => u.PasswordIsInicial).ToColumn("JeHesloInicial");
            Map(u => u.LicenseNumber).ToColumn("CisloLicence");
            Map(u => u.PhoneNumber).ToColumn("Telefon");
            Map(u => u.CustomerId).ToColumn("IDUzivatele");
            Map(u => u.ValidFrom).ToColumn("PlatiOd");
            Map(u => u.ValidTo).ToColumn("PlatiDo");
            Map(u => u.CreationUserId).ToColumn("Kdo");
            Map(u => u.CreationDate).ToColumn("Kdy");
            Map(u => u.Email).ToColumn("Email");
            Map(u => u.Valid).ToColumn("Valid");
        }
    }
}
