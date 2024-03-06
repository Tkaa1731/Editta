using Pobytne.Shared.Struct;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pobytne.Shared.Procedural.DTO
{
    [Serializable]
    [Table("S_TypyPlatby")]
    public class Payment : ACreation
    {
        public int Id { get; set; } //IDTypuPlatby
        public string Name { get; set; } = string.Empty; //NazevDokladu
        public int ModuleId { get; set; } //IDModulu
        public EPaymentType Type { get; set; } //TypPlatby
        public string FacturePrefix { get; set; } = string.Empty; //PrefixDokladu
        public int FactureNumber { get; set; } //CisloDokladu
        public int DefaultPayment { get; set; } //VychoziPlatba
        [Required(ErrorMessage = "Vyplňte datum")]
        public DateTime ValidFrom { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Vyplňte datum")]
        public DateTime ValidTo { get; set; } = DateTime.Now.AddYears(1);
    }
}
