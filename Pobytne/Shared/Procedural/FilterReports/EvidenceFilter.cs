using Pobytne.Shared.Struct;

namespace Pobytne.Shared.Procedural.FilterReports
{
	public class EvidenceFilter : IFilterReport
	{
		public required int? ModuleId { get; set; }
        public required int? ContractType { get; set; }
		public required int? OSPOD { get; set; }
		public required DateTime From { get; set; }
		public required DateTime To { get; set; }
		public int? ClientId { get; set; }
        public List<int> RecordsId { get; set; } = [];
	}
}
