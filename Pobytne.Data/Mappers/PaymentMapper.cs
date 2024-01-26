using Dapper.FluentMap.Mapping;
using Pobytne.Shared.Procedural;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pobytne.Data.Mappers
{
    internal class PaymentMapper : EntityMap<Payment>
    {
        public PaymentMapper() 
        {
            Map(p => p.Id).ToColumn("IDTypuPlatby");
            Map(p => p.ModuleId).ToColumn("IDModulu");
            Map(p => p.Type).ToColumn("TypPlatby");
            Map(p => p.Name).ToColumn("NazevDokladu");
            Map(p => p.FacturePrefix).ToColumn("PrefixDokladu");
            Map(p => p.FactureNumber).ToColumn("CisloDokladu");
            Map(p => p.DefaultPayment).ToColumn("VychoziPlatba");
                
            Map(p => p.ValidFrom).ToColumn("PlatiOd");
            Map(p => p.ValidTo).ToColumn("PlatiDo");
            Map(p => p.CreationUserId).ToColumn("Kdo");
            Map(p => p.CreationDate).ToColumn("Kdy");
        }
    }
}
