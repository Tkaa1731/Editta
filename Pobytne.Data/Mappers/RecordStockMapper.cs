using Dapper.FluentMap.Mapping;
using Pobytne.Shared.Procedural;

namespace Pobytne.Data.Mappers
{
    internal class RecordStockMapper :  EntityMap<RecordStock>
    {
        public RecordStockMapper() 
        {
            Map(r => r.Id).ToColumn("IDPohybu");
            Map(r => r.RecordId).ToColumn("IDZaznamu");

            Map(r => r.Date).ToColumn("Datum");
            Map(r => r.Quantity).ToColumn("Jednotka");
            Map(r => r.Price).ToColumn("Castka");

            Map(c => c.CreationUserId).ToColumn("Kdo");
            Map(c => c.CreationDate).ToColumn("Kdy");
        }
    }
}
