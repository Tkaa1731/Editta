﻿using Dapper.FluentMap.Mapping;
using Pobytne.Shared.Procedural.DTO;

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
            Map(u => u.ClientId).ToColumn("IDUzivatele");

            Map(u => u.ValidFrom).ToColumn("PlatiOd");
            Map(u => u.ValidTo).ToColumn("PlatiDo");
            Map(c => c.CreationUserId).ToColumn("Kdo");
            Map(c => c.CreationDate).ToColumn("Kdy");
        }
    }
}
