using Pobytne.Shared.Struct;

namespace Pobytne.Shared.Procedural
{
	public class Evidence : ACreation, IRecordAttribute
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
		public int ClientId { get; set; }
		public string CustomerName { get; set; } = string.Empty;
		// IRecordAttribute
		public int RecordAttributeId { get; set; }
		public string RecordAttributeName { get; set; } = string.Empty;
		public int AccountA { get; set; }
		public int AccountS { get; set; }
	}
}
