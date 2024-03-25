using Dapper.FluentMap.Mapping;
using Pobytne.Shared.Procedural;

namespace Pobytne.Data.Mappers
{
    internal class SeasonTicketMapper :  EntityMap<SeasonTicket>
    {
        internal SeasonTicketMapper() 
        {
            Map(c => c.Id).ToColumn("IDPermanentka");

            Map(c => c.ClientId).ToColumn("IDUzivatele");
            Map(c => c.ClientName).ToColumn("JmenoUzivatele");

            Map(c => c.RecordId).ToColumn("IDZaznamu");
            //Map(c => c.ModuleId).ToColumn("JeSpolecnyProCisloLicence");

            Map(c => c.Valid).ToColumn("JePlatna");
            Map(c => c.IsPayed).ToColumn("JeZaplacena");
            Map(c => c.Price).ToColumn("CenaPermanentky");
            Map(c => c.Quantity).ToColumn("PocetVstupu");

            Map(c => c.ValidFrom).ToColumn("PlatiOd");
            Map(c => c.ValidTo).ToColumn("PlatiDo");

            Map(c => c.CreationUserId).ToColumn("Kdo");
            Map(c => c.CreationDate).ToColumn("Kdy");
        }
    }
}
