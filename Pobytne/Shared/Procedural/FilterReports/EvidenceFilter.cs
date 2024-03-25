using Pobytne.Shared.Struct;

namespace Pobytne.Shared.Procedural.FilterReports
{
	public class EvidenceFilter() : LazyList(typeof(Evidence)), IFilterReport
	{
		public required int? ModuleId { get; set; }
        public required int? ContractType { get; set; }
		public required int? OSPOD { get; set; }
		public DateTime From { get; set; } = DateTime.Today;
		public DateTime To { get; set; } = DateTime.Today.AddDays(1);
		public int? ClientId { get; set; }
        public List<int> RecordsId { get; set; } = [];
	}
}
