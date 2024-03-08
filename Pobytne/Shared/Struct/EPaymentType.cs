using System.ComponentModel.DataAnnotations;

namespace Pobytne.Shared.Struct
{
    public enum EPaymentType
    {
        [Display(Name = "Hotově")]
        Cash = 0,
        [Display(Name = "Kartou")]
        Card = 1,
        [Display(Name = "Poukázky")]
        Ticket = 3,
        [Display(Name = "Bankovní převod")]
        BankTransfer = 4,
    }
}
