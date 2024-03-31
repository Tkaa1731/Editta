using Pobytne.Shared.Struct;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pobytne.Shared.Procedural
{
    [Table("P_Permanentka")]
    public class SeasonTicket : ACreation
    {
        public int Id {  get; set; }
        public int ClientId {  get; set; }
        [Editable(false)]
        public string ClientName { get; set; } = string.Empty;
        public int RecordId { get; set; }
        public bool Valid { get; set; } = true;
        public bool IsPayed { get; set; } = true;
        public float Price { get; set; }
        public int Quantity {  get; set; }
        [Editable(false)]
        public List<SeasonTicketEvidence> TicketEvidences { get; set; } = [];
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

    }
}
