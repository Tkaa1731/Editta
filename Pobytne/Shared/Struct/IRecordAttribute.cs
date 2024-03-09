namespace Pobytne.Shared.Struct
{
	public interface IRecordAttribute
	{
		int RecordAttributeId { get; set; }
		string RecordAttributeName { get; set; }
		int AccountA { get; set; }
		int AccountS { get; set; }
	}
}
