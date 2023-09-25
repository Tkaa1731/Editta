using Dapper.FluentMap.Mapping;
using Pobytne.Shared.Procedural;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pobytne.Data.mappers
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
            Map(u => u.CreationUserId).ToColumn("Kdo");
            Map(u => u.CreationDate).ToColumn("Kdy");
        }
    }
}
