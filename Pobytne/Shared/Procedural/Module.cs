using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pobytne.Shared.Procedural
{
    [Serializable]
    [Table("S_Moduly")]
    public class Module
    {
        [Key]
        public int Id { get; set;}
        public string Name { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
        [ForeignKey("License")]
        public int LicenseId { get; set;}
        public int LicenseNumber { get; set;}
        public int EvidenceType { get; set;}
    }
}
