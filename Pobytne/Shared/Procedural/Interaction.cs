using Pobytne.Shared.Struct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pobytne.Shared.Procedural
{
    public class Interaction : ICreation
    {
        public int Id {  get; set; } = -1;
        public int ModuleId { get; set; } = -1;
        public int ClientId {  get; set; } = 0;
        public int PaymentId { get; set; }
        public DateTime InteractionDate { get; set; } = DateTime.Today;

        public DateTime ValidFrom { get { return InteractionDate; } }

        public DateTime ValidTo { get{ return InteractionDate.AddYears(100); } }

        public int CreationUserId { get; set; } = -1;

        public DateTime CreationDate { get; set; } = DateTime.Now;

        public string CreationUserName { get; set; } = string.Empty;

        public DateTime TimeStamp { get; set; } = DateTime.Now;
        public List<InteractionRecordItem> Records { get; set; } = [];
        public string InteractionName {  get; set; } = string.Empty;
    }
}
