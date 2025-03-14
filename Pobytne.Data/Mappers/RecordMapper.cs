﻿using Dapper.FluentMap.Mapping;
using Pobytne.Shared.Procedural;

namespace Pobytne.Data.Mappers
{
    internal class RecordMapper : EntityMap<Record>
    {
        internal RecordMapper()
        {
            Map(r => r.Id).ToColumn("IDZaznamu");
            Map(r => r.ModuleId).ToColumn("IDModulu");
            Map(r => r.Order).ToColumn("Poradi");
            Map(r => r.Name).ToColumn("Nazev");
            Map(r => r.RecordType).ToColumn("TypZaznamu");

            Map(r => r.RecordAttributeId).ToColumn("IDZaznamuVlastnosti");

            Map(r => r.Quantity).ToColumn("Jednotka");
            Map(r => r.GroupQuantity).ToColumn("JednotkaCelkem");
            Map(r => r.Adult).ToColumn("Dospely");
            Map(r => r.Child).ToColumn("Dite");
            Map(r => r.Price).ToColumn("Castka");
            Map(r => r.GroupPrice).ToColumn("CastkaCelkem");
            Map(r => r.Stock).ToColumn("Zustatek");

            Map(r => r.IsClientRequired).ToColumn("JeVyzadovanUzivatel");
            Map(r => r.IsPriceRequired).ToColumn("JeVyzadovanaCastka");
            Map(r => r.IsBalanceCheck).ToColumn("JeKontrolaNaZustatek");
            Map(r => r.IsSeasonTicket).ToColumn("JePermanentka");
            Map(r => r.Note).ToColumn("Poznamka");

            Map(r => r.ParentId).ToColumn("IDParent");
            Map(r => r.RootId).ToColumn("IDRoot");                                                 
            Map(r => r.StructDepth).ToColumn("Uroven");

            Map(r => r.ValidFrom).ToColumn("PlatiOd");
            Map(r => r.ValidTo).ToColumn("PlatiDo");
            Map(c => c.CreationUserId).ToColumn("Kdo");
            Map(c => c.CreationDate).ToColumn("Kdy");
        }
    }
}
