using Dapper.FluentMap.Mapping;
using Pobytne.Shared.Procedural;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Map(u => u.CreationUserId).ToColumn("Kdo");
            Map(u => u.CreationDate).ToColumn("Kdy");
        }
    }
}
