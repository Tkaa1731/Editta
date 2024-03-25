using Pobytne.Data.Tables;
using Pobytne.Shared.Procedural;

namespace Pobytne.Server.Service
{
	public class SeasonTicketService(SeasonTicketTable table)
	{
		private readonly SeasonTicketTable _table = table;
		public async Task<IEnumerable<SeasonTicket>> GetSeasonTicketEvidence(int recordId)
		{
			return await _table.GetSeasonTicket(new { RecordId = recordId, ValidTo = DateTime.Now });
		}
	}
}
