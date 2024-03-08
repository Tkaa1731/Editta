using System.ComponentModel.DataAnnotations;

namespace Pobytne.Shared.Struct
{
    public enum ERecordType
    {
        [Display(Name ="Složka")]
        Folder = 0,
        [Display(Name = "Produkt")]
        Ware = 1,
        [Display(Name = "Aktivita")]
        Activity = 2,
        [Display(Name = "Pravidená aktivita")]
        RegularActivity = 3,
        [Display(Name = "Zaměstanci")]
        EmployeeTasks = 4
    }
}
