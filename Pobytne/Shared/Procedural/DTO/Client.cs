using Pobytne.Shared.Struct;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace Pobytne.Shared.Procedural.DTO
{
    [Serializable]
    [Table("S_Uzivatele")]
    public partial class Client : ACreation, IListItem
    {
        [Key]
        public int Id { get; set; }
        public int ModuleId { get; set; }
        [Required(ErrorMessage = "Vyplňte jméno")]
		[MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        public bool ThroughoutOrganization { get; set; }
		[MaxLength(80)]
        public string Description { get; set; } = string.Empty;
        [MaxLength(40)]
        public string City { get; set; } = string.Empty;
		[MaxLength(40)]
        public string Street { get; set; } = string.Empty;
		[MaxLength(10)]
        public string PostNumber { get; set; } = string.Empty;
        [EmailAddress(ErrorMessage = "Vyplňte validní email")]
		[MaxLength(50)]
        public string Email { get; set; } = string.Empty;
        [Phone(ErrorMessage = "Vyplňte validní telefonní číslo")]
		[MaxLength(30)]
        public string PhoneNumber { get; set; } = string.Empty;
        public bool Valid { get; set; }
        [Editable(false)]
        public bool Active => Valid;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    //other properties needed for DB insert
    public partial class Client
    {
        public string JmenoDitete => string.Empty;
        public DateTime DatumNarozeni => SqlDateTime.MinValue.Value;
        public bool ClenSdruzeni => false;
        public DateTime ClenSdruzeniOd => SqlDateTime.MinValue.Value;
        public DateTime ClenSdruzeniDo => SqlDateTime.MinValue.Value;
        public DateTime PrvniNavsteva => SqlDateTime.MinValue.Value;
        public byte MesiceDite1 => 0;
        public byte MesiceDite2 => 0;
        public byte MesiceDite3 => 0;
        public DateTime DatumNarozeniDite1 => SqlDateTime.MinValue.Value;
        public DateTime DatumNarozeniDite2 => SqlDateTime.MinValue.Value;
        public DateTime DatumNarozeniDite3 => SqlDateTime.MinValue.Value;
        public byte InformaceOCentru => 0;
        public int TypKlienta => 0;
        public bool GDPR_Souhlas => false;
        public DateTime GDPR_SouhlasDatum => SqlDateTime.MinValue.Value;
        public DateTime GDPR_ZadostVymazDatum => SqlDateTime.MinValue.Value;
        public DateTime GDPR_VymazDatum => SqlDateTime.MinValue.Value;
        public bool GDPR_SouhlasFotografie => false;
    }
}
