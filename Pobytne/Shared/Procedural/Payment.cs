using Pobytne.Shared.Struct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pobytne.Shared.Procedural
{
    public class Payment :ICreation
    {
        public int Id { get; set; } //IDTypuPlatby
        public string Name { get; set; } = string.Empty; //NazevDokladu
        public int ModuleId { get; set; } //IDModulu
        public EPaymentType Type { get; set; } //TypPlatby
        public string FacturePrefix { get; set; } = string.Empty; //PrefixDokladu
        public int FactureNumber { get; set; } //CisloDokladu
        public int DefaultPayment { get; set; } //VychoziPlatba

        public DateTime ValidFrom { get; set; } //PlatiOd

        public DateTime ValidTo { get; set; } //PlatiDo

        public int CreationUserId { get; set; } //Kdo

        public DateTime CreationDate { get; set;} //Kdy

        public string CreationUserName { get; set; } = string.Empty;
    }
}
