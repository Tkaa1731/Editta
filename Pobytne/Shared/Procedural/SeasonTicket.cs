using Pobytne.Shared.Struct;
using System.ComponentModel.DataAnnotations;

namespace Pobytne.Shared.Procedural
{
    public class SeasonTicket : ACreation
    {
        public int Id {  get; set; }
        public int ClientId {  get; set; }
        public string ClientName { get; set; } = string.Empty;
        public int RecordId { get; set; }
        //public string RecordName { get; set; } = string.Empty;
        //public int ModuleId { get; set; }
        public bool Valid { get; set; }
        public bool IsPayed { get; set; }
        public float Price { get; set; }
        public int Quantity {  get; set; }
        [Editable(false)]
        public List<SeasonTicketEvidence> TicketEvidences { get; set; } = [];
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

    }
}
