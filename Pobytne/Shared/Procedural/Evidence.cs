using Pobytne.Shared.Struct;

namespace Pobytne.Shared.Procedural
{
	public class Evidence : ACreation, IRecordProperty
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
		// IRecordProperty
		public int RecordPropertyId { get; set; }
		public string RecordPropertyName { get; set; } = string.Empty;
		public int AccountA { get; set; }
		public int AccountS { get; set; }
	}
}
