using Dapper.FluentMap.Mapping;
using Pobytne.Shared.Procedural.DTO;

namespace Pobytne.Data.Mappers
{
    internal class ModuleMapper : EntityMap<Module>
    {
        internal ModuleMapper() 
        {
            Map(u => u.Id).ToColumn("IDModulu");
            Map(u => u.ModuleName).ToColumn("Nazev");
            Map(u => u.ModuleShortName).ToColumn("ZkracenyNazev");

            Map(u => u.LicenseNumber).ToColumn("CisloLicence");
            Map(u => u.EvidenceType).ToColumn("TypEvidence");
            Map(u => u.OnlyUsersByIdOfModule).ToColumn("JenUzivatelDleIDModulu");

            Map(c => c.CreationUserId).ToColumn("Kdo");
            Map(c => c.CreationDate).ToColumn("Kdy");
        }
    }
}
