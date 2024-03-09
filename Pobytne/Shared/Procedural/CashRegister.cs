using Pobytne.Shared.Struct;

namespace Pobytne.Shared.Procedural
{
	public class CashRegister : ACreation, IRecordAttribute
	{
		public int Id { get; set; }
		public int Order { get; set; }
		public float Price {  get; set; }
		// druh platby cash/card
		public int PaymentId { get; set; }
		//public string PaymentName { get; set; } = string.Empty;
		// UZIVATEL
		public int ClientId {  get; set; }
		public string CustomerName { get; set; } = string.Empty;
		// INTERAKCE
		public int InteractionId { get; set; }
		public string InteractionDescription {  get; set; } = string.Empty;	
		public DateTime InteractionDate { get; set; }
		// ZAZNAM
		public int RecordId { get; set; }
		public string RecordName { get; set; } = string.Empty;
		// IRecordAttribute
		public int RecordAttributeId { get; set; }
		public string RecordAttributeName { get; set; } = string.Empty;
		public int AccountA { get; set; }
		public int AccountS { get; set; }
	}
}
