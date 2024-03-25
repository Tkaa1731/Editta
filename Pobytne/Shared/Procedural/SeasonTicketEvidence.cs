using Pobytne.Shared.Struct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pobytne.Shared.Procedural
{
    public class SeasonTicketEvidence : ACreation
    {
        public int Id {  get; set; }
        public int SeasonTicketId { get; set;}
        public int EvidenceId { get; set; }
        public int InteractionId { get; set; }
        public DateTime InteractionDate { get; set; }
    }
}
