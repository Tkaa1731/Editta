using Pobytne.Shared.Struct;
using Pobytne.Shared.Struct.RecordType;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pobytne.Shared.Procedural
{
    [Table("S_Zaznamy")]
    public class Record : IActivity_Record, IFolder_Record, IWare_Record, IEmployeeTask_Record, IListItem
    {
        [Key]
        [Editable(false)]
        public int Id { get; set; }
        public int ModuleId { get; set; }
        public int Order {  get; set; }
        public string Name { get; set; } = string.Empty;
        public ERecordType RecordType { get; set; }
        // S_ZaznamyVlastnosti
        public int RecordPropertiesId {  get; set; }
        public string RecordPropertiesName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public int Adult {  get; set; }
        public int Child {  get; set; }
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
        // ICREATION
        public DateTime ValidFrom { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Enter date of the end of validation")]
        public DateTime ValidTo { get; set; } = DateTime.Now.AddYears(1);
        public int CreationUserId { get; set; }
        public DateTime CreationDate { get; set; }
        [Editable(false)]
        public string CreationUserName { get; set; } = string.Empty;
        // IListItem
        public string Description => Note;

        public bool Active => ValidTo >= DateTime.Now;
        public int RootId {  get; set; }
        public int ParentId {  get; set; }
    }
}
