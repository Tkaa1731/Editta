using Dapper.FluentMap.Mapping;
using Pobytne.Shared.Procedural;

namespace Pobytne.Data.Mappers
{
	internal class EvidenceMapper : EntityMap<Evidence>
	{
		internal EvidenceMapper()
		{
			Map(c => c.Id).ToColumn("IDEvidence");
			Map(c => c.Order).ToColumn("Poradi");
			Map(c => c.Quantity).ToColumn("Jednotka");
			Map(c => c.Adult).ToColumn("Dospely");
			Map(c => c.Child).ToColumn("Dite");

			Map(c => c.RecordId).ToColumn("IDZaznamu");
			Map(c => c.RecordName).ToColumn("Nazev");

			Map(c => c.InteractionId).ToColumn("IDInterakce");
			Map(c => c.InteractionDescription).ToColumn("NazevInterakce");
			Map(c => c.InteractionDate).ToColumn("Datum");

			Map(c => c.CustomerId).ToColumn("IDUzivatele");
			Map(c => c.CustomerName).ToColumn("JmenoUzivatele");

			Map(c => c.CreationUserId).ToColumn("Kdo");
			Map(c => c.CreationDate).ToColumn("Kdy");
		}
	}
}
