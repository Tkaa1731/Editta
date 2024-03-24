using Pobytne.Shared.Struct;

namespace Pobytne.Shared.Procedural.FilterReports
{
	public class CashRegisterFilter() : LazyList(typeof(CashRegister)), IFilterReport
	{
		public int PaymentId { get; set; }// not null
		public int? ModuleId { get; set; }// not null
		public required DateTime From { get; set; }
		public required DateTime To { get; set; }
		public int? ClientId { get; set; }
		public List<int> RecordsId { get; set; } = [];
    }
}
