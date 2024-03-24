using Pobytne.Shared.Struct;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pobytne.Shared.Procedural
{
    public class Interaction : ACreation
    {
        public int Id {  get; set; } = -1;
        public int ModuleId { get; set; } = -1;
        public int ClientId {  get; set; } = 0;
        public int PaymentId { get; set; }
        public DateTime InteractionDate { get; set; } = DateTime.Today;
        public DateTime TimeStamp { get; set; } = DateTime.Now;
        public List<InteractionRecordItem> Records { get; set; } = [];
		[MaxLength(200)]
        public string InteractionName {  get; set; } = string.Empty;
    }
}
