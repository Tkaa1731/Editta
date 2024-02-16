namespace Pobytne.Shared.Struct
{
	public interface IFilterReport
	{
		int? ModuleId { get; set; }
		int? RecordId { get; set; }
		DateTime From { get; set; }
		DateTime To { get; set; }
	}
}
