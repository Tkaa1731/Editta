namespace Pobytne.Shared.Struct
{
	public interface IFilterReport
	{
		int? ModuleId { get; set; }
		List<int> RecordsId { get; set; }
		DateTime From { get; set; }
		DateTime To { get; set; }
	}
}
