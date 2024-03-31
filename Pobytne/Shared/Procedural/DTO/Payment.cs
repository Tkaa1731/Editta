using Pobytne.Shared.Struct;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pobytne.Shared.Procedural.DTO
{
    [Serializable]
    [Table("S_TypyPlatby")]
	public class Payment : ACreation, IListItem
    {
        public int Id { get; set; } //IDTypuPlatby
		[MaxLength(50)]
        public string Name { get; set; } = string.Empty; //NazevDokladu
        public int ModuleId { get; set; } = -1;//IDModulu
        public EPaymentType Type { get; set; } //TypPlatby
		[MaxLength(10)]
        public string InvoicePrefix { get; set; } = string.Empty; //PrefixDokladu
        public int InvoiceNumber { get; set; } //CisloDokladu
        public int DefaultPayment { get; set; } //VychoziPlatba
        [Required(ErrorMessage = "Vyplňte datum")]
        public DateTime ValidFrom { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Vyplňte datum")]
        public DateTime ValidTo { get; set; } = DateTime.Now.AddYears(1);
        [Editable(false)]
        public string Description => Type.ToString();
        [Editable(false)]
        public bool Active => ValidTo > DateTime.Now;

        public object Clone()
		{
			return MemberwiseClone();
		}
	}
}
