using Pobytne.Shared.Struct;
using Pobytne.Shared.Struct.RecordType;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pobytne.Shared.Procedural
{
    [Table("S_Zaznamy")]
    public class Record : ACreation, IActivity_Record, IFolder_Record, IWare_Record, IEmployeeTask_Record, IListItem
    {
        [Key]
        [Editable(false)]
        public int Id { get; set; }
        public int ModuleId { get; set; }
        public int RootId {  get; set; }
        public int ParentId {  get; set; }
        [Required(ErrorMessage = "Vyplňte pořadí v seznamu")]
        public int Order {  get; set; }
        [Required(ErrorMessage = "Vyplňte název položky")]
        public string Name { get; set; } = string.Empty;
        public ERecordType RecordType { get; set; }
        // S_ZaznamyVlastnosti
        public int RecordPropertiesId {  get; set; }
        public string RecordPropertiesName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Vyplňte množství")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Vyplňte počet dospělých")]
        public int Adult {  get; set; }
        [Required(ErrorMessage = "Vyplňte počet dětí")]
        public int Child {  get; set; }
        [Required(ErrorMessage = "Vyplňte cenu")]
        public float Price {  get; set; }
        public int Stock {  get; set; }
        public bool IsClientRequired { get; set; }
        public bool IsPriceRequired {  get; set; }
        public bool IsBalanceCheck { get; set; }
        public bool IsSeasonTicket{  get; set; }
        public int GroupQuantity { get; set; }
        public float GroupPrice { get; set; }
        public string Note {  get; set; } = string.Empty;
        public int StructDepth { get; set; }
        [Required(ErrorMessage = "Vyplňte datum")]
        public DateTime ValidFrom { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Vyplňte datum")]
        public DateTime ValidTo { get; set; } = DateTime.Now.AddYears(1);
        // IListItem
        public string Description => Note;
        public bool Active => ValidTo >= DateTime.Now;

    }
}
