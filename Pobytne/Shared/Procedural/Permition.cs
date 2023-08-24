using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pobytne.Shared.Procedural
{
    public class Permition
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Module")]
        public int ModuleId { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public string ModuleName { get; set; } = string.Empty;
        public string PermitionString { get; set; } = string.Empty;
        [ForeignKey("User")]
        public int EditorId { get; set; }
        public string EditorName { get; set; } = string.Empty;
        public DateTime LastChange { get; set; }
        public bool CheckPemition(string quaery)
        {
            return true;
        }
    }
}
