using Pobytne.Shared.Struct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pobytne.Shared.Procedural
{
	public class CashRegister
	{
		public int Id { get; set; }
		public float Price {  get; set; }
		// druh platby cash/card
		public int PaymentId { get; set; }
		public string PaymentName { get; set; } = string.Empty;
		// UZIVATEL
		public int CustomerId {  get; set; }
		public string CustomerName { get; set; } = string.Empty;
		// INTERAKCE
		public int InteractionId { get; set; }
		public DateTime InteractionDate { get; set; }
		public string InteractionDescription {  get; set; } = string.Empty;	
		// ZAZNAM
		public int RecordId { get; set; }
		public string RecordName { get; set; } = string.Empty;
		// ICREATION
		public int CreationUserId { get; set; }
		public DateTime CreationDate { get; set; }
		public string CreationUserName { get; set; } = string.Empty;

	}
}
