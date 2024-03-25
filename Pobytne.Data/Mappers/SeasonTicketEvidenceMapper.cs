using Dapper.FluentMap.Mapping;
using Pobytne.Shared.Procedural;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pobytne.Data.Mappers
{
    internal class SeasonTicketEvidenceMapper : EntityMap<SeasonTicketEvidence>
    {
        internal SeasonTicketEvidenceMapper() 
        {
            Map(c => c.Id).ToColumn("IDPermanentkaEvidence");

            Map(c => c.SeasonTicketId).ToColumn("IDPermanentka");
            Map(c => c.EvidenceId).ToColumn("IDEvidence");

            Map(c => c.InteractionId).ToColumn("IDInterakce");
            Map(c => c.InteractionDate).ToColumn("Datum");

            Map(c => c.CreationUserId).ToColumn("Kdo");
            Map(c => c.CreationDate).ToColumn("Kdy");
        }
    }
}
