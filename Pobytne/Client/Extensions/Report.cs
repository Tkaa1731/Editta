namespace Pobytne.Client.Extensions
{
	internal class Report
	{
		public int RecordId { get; set; }
		public string RecordName { get; set; } = string.Empty;
		public float Price { get; set; }
		public int RecordAttributeId {  get; set; }
		public string RecordAttributeName { get; set; } = string.Empty;
		public int AccountA {  get; set; }
		public int AccountS { get; set; }
	}
}
