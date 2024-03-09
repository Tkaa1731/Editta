using Dapper.FluentMap.Mapping;
using Pobytne.Shared.Procedural;

namespace Pobytne.Data.Mappers
{
	internal class CashRegisterMapper : EntityMap<CashRegister>
    {
        internal CashRegisterMapper()
		{
			Map(c => c.Id).ToColumn("IDPokladna");
			Map(c => c.Order).ToColumn("Poradi");
			Map(c => c.Price).ToColumn("Castka");

			Map(c => c.PaymentId).ToColumn("IDTypuPlatby");
			//Map(c => c.PaymentName).ToColumn("NazevDokladu");

			Map(c => c.ClientId).ToColumn("IDUzivatele");
			Map(c => c.CustomerName).ToColumn("JmenoUzivatele");

			Map(c => c.InteractionId).ToColumn("IDInterakce");
			Map(c => c.InteractionDescription).ToColumn("NazevInterakce");
			Map(c => c.InteractionDate).ToColumn("Datum");

			Map(c => c.RecordId).ToColumn("IDZaznamu");
			Map(c => c.RecordName).ToColumn("Nazev");
            Map(c => c.CreationUserId).ToColumn("Kdo");
            Map(c => c.CreationDate).ToColumn("Kdy");
        }
	}

}
