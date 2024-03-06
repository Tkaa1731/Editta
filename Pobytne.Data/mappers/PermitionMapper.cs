using Dapper.FluentMap.Mapping;
using Pobytne.Shared.Procedural.DTO;

namespace Pobytne.Data.Mappers
{
    internal class PermitionMapper : EntityMap<Permition>
    {
        internal PermitionMapper() 
        {
            Map(u => u.Id).ToColumn("IDOpravneni");
            Map(u => u.UserId).ToColumn("IDLogin");
            Map(u => u.ModuleId).ToColumn("IDModulu");

            Map(u => u.PermitionString).ToColumn("Opravneni");

            Map(u => u.ValidFrom).ToColumn("PlatiOd");
            Map(u => u.ValidTo).ToColumn("PlatiDo");
            Map(c => c.CreationUserId).ToColumn("Kdo");
            Map(c => c.CreationDate).ToColumn("Kdy");
        }
    }
}
