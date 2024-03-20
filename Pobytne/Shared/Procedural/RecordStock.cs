using Pobytne.Shared.Struct;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pobytne.Shared.Procedural
{
    [Table("P_PohybyEvidence")]
    public class RecordStock : ACreation, IListItem
    {
        [Key]
        public int Id { get; set; }
        public int RecordId { get; set; }
        public int Quantity { get; set; } = 0;
        [Range(0.0,float.MaxValue, ErrorMessage ="Minimální možná cena je 0.0")]
        public float Price { get; set; } = 0;
        [Required(ErrorMessage = "Vyplňte datum")]
        public DateTime Date { get; set; } = DateTime.Now;
        // IListItem needed only for using FormModal
        [Editable(false)]
        public string Name => "";
        [Editable(false)]
        public string Description => "";
        [Editable(false)]
        public bool Active => true;

        public object Clone()
        {
           return MemberwiseClone();
        }
    }
}
