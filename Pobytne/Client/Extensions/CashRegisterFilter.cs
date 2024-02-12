namespace Pobytne.Client.Extensions
{
	public class CashRegisterFilter
	{
		public required int? PaymentId { get; set; }
		public required int ModuleId { get; set; }
		public required DateTime From { get; set; }
		public required DateTime To { get; set; }
		public int CustimerId { get; set; } = -1;

	}
}
