using Pobytne.Shared.Struct;

namespace Pobytne.Shared.Procedural.FilterReports
{
	public class CashRegisterFilter() : LazyList(typeof(CashRegister)), IFilterReport
	{
		public int PaymentId { get; set; }// not null
		public int? ModuleId { get; set; }// not null
		public DateTime From { get; set; } = DateTime.Today;
		public DateTime To { get; set; } = DateTime.Today.AddDays(1);
		public int? ClientId { get; set; }
		public List<int> RecordsId { get; set; } = [];
    }
}
