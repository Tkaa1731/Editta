using Pobytne.Shared.Struct;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pobytne.Shared.Procedural
{
    [Table("P_PermanentkaEvidence")]
	public class SeasonTicketEvidence : ACreation
    {
        public int Id {  get; set; }
        public int SeasonTicketId { get; set;}
        public int EvidenceId { get; set; }
        [Editable(false)]
        public int InteractionId { get; set; }
        [Editable(false)]
        public DateTime InteractionDate { get; set; }
	}
}
