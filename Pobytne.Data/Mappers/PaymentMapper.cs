using Dapper.FluentMap.Mapping;
using Pobytne.Shared.Procedural.DTO;

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
            Map(p => p.InvoicePrefix).ToColumn("PrefixDokladu");
            Map(p => p.InvoiceNumber).ToColumn("CisloDokladu");
            Map(p => p.DefaultPayment).ToColumn("VychoziPlatba");
                
            Map(p => p.ValidFrom).ToColumn("PlatiOd");
            Map(p => p.ValidTo).ToColumn("PlatiDo");
            Map(c => c.CreationUserId).ToColumn("Kdo");
            Map(c => c.CreationDate).ToColumn("Kdy");
        }
    }
}
