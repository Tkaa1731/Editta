using Dapper.FluentMap.Mapping;
using Pobytne.Shared.Procedural.DTO;

namespace Pobytne.Data.Mappers
{
    internal class LicenseMapper : EntityMap<License>
    {
        internal LicenseMapper()
        {
            Map(l => l.Id).ToColumn("IDLicence");
            Map(l => l.LicenseNumber).ToColumn("CisloLicence");
            Map(l => l.NameOfOrganization).ToColumn("NazevOrganizace");
            Map(l => l.VersionType).ToColumn("TypVerze");

            Map(l => l.IsDemo).ToColumn("JeDemo");
            Map(l => l.IsPayed).ToColumn("JeZaplacena");
            Map(l => l.IsBlocked).ToColumn("JeBlokovana");

            Map(l => l.DateOfLaunch).ToColumn("DatumVystaveni");
            Map(l => l.DateOfPayment).ToColumn("DatumPlatby");

            Map(l => l.ContactPerson).ToColumn("KontaktniOsoba");
            Map(l => l.City).ToColumn("Obec");
            Map(l => l.Street).ToColumn("Ulice");
            Map(l => l.PostNumber).ToColumn("PSC");
            Map(l => l.PhoneNumber).ToColumn("Telefon");

            Map(l => l.ValidFrom).ToColumn("PlatiOd");
            Map(l => l.ValidTo).ToColumn("PlatiDo");
            Map(c => c.CreationUserId).ToColumn("Kdo");
            Map(c => c.CreationDate).ToColumn("Kdy");
        }
    }
}
