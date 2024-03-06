using Dapper.FluentMap.Mapping;
using Pobytne.Shared.Procedural.DTO;

namespace Pobytne.Data.Mappers
{
    internal class ClientMapper : EntityMap<Client>
    {
        internal ClientMapper() 
        { 
            Map(c => c.Id).ToColumn("IDUzivatele");
            Map(c => c.ModuleId).ToColumn("IDModulu");
            Map(c => c.Name).ToColumn("JmenoUzivatele");
            Map(c => c.Description).ToColumn("Poznamka");
            Map(c => c.ThroughoutOrganization).ToColumn("JeSpolecnyProCisloLicence");

            Map(c => c.PhoneNumber).ToColumn("Telefon");
            Map(c => c.City).ToColumn("Obec");
            Map(c => c.Street).ToColumn("Ulice");
            Map(c => c.PostNumber).ToColumn("PSC");

            Map(c => c.Valid).ToColumn("JePlatny");

            Map(c => c.CreationUserId).ToColumn("Kdo");
            Map(c => c.CreationDate).ToColumn("Kdy");
        }
    }
}
