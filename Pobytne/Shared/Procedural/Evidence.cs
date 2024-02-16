using Pobytne.Shared.Struct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pobytne.Shared.Procedural
{
    public class Evidence
    {
        public int Id {  get; set; }
		public int Order {  get; set; }
        public int Quantity { get; set; }
        public int Adult { get; set; }
        public int Child {  get; set; }
		// ZAZNAM
		public int RecordId {  get; set; }
        public string RecordName {  get; set; } = string.Empty;
		// INTERAKCE
		public int InteractionId { get; set; }
		public DateTime InteractionDate { get; set; }
		public string InteractionDescription { get; set; } = string.Empty;
		// UZIVATEL
		public int CustomerId { get; set; }
		public string CustomerName { get; set; } = string.Empty;
		// ICREATION
		public int CreationUserId { get; set; }
		public DateTime CreationDate { get; set; }
		public string CreationUserName { get; set; } = string.Empty;
	}
}
