using Pobytne.Shared.Struct;

namespace Pobytne.Shared.Procedural.FilterReports
{
	public class CashRegisterFilter : IFilterReport
	{
		public required int PaymentId { get; set; }
		public required int? ModuleId { get; set; }
		public required int? RecordId { get; set; }
		public required DateTime From { get; set; }
		public required DateTime To { get; set; }
		public int? ClientId { get; set; }
	}
}
