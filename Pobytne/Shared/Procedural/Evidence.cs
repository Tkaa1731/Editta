using Pobytne.Shared.Struct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pobytne.Shared.Procedural
{
    internal class Evidence : ICreation
    {
        public int Id {  get; set; }
        public int RecordId {  get; set; }
        public string RecordName {  get; set; } = string.Empty;
        public int InteractionId { get; set; }
        public int Order {  get; set; }
        public int Quantity { get; set; }
        public int Adult { get; set; }
        public int Child {  get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime ValidTo { get; set; }

        public int CreationUserId { get; set; }

        public DateTime CreationDate { get; set; }

        public string CreationUserName { get; set; } = string.Empty;
    }
}
