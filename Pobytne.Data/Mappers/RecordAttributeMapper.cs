using Dapper.FluentMap.Mapping;
using Pobytne.Shared.Procedural;

namespace Pobytne.Data.Mappers
{
	internal class RecordAttributeMapper : EntityMap<RecordAttribute>
	{
		internal RecordAttributeMapper()
		{
			Map(r => r.Id).ToColumn("IDZaznamuVlastnosti");
			Map(r => r.ModuleId).ToColumn("IDModulu");

			Map(r => r.Name).ToColumn("Nazev");
			Map(r => r.Type).ToColumn("TypZaznamu");

			Map(r => r.AccountA).ToColumn("UcetA");
			Map(r => r.AccountS).ToColumn("UcetS");

			Map(r => r.CentreNumber).ToColumn("Stredisko");
			Map(r => r.OrderNumber).ToColumn("Zakazka");
			Map(r => r.ProjectNumber).ToColumn("Projekt");

			Map(c => c.CreationUserId).ToColumn("Kdo");
			Map(c => c.CreationDate).ToColumn("Kdy");
		}
	}
}
