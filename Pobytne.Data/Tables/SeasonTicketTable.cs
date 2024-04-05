using Dapper;
using Dapper.Transaction;
using Pobytne.Shared.Procedural;
using System.Data;

namespace Pobytne.Data.Tables
{
    public class SeasonTicketTable
	{
		public async Task<IEnumerable<SeasonTicket>> GetSeasonTicket(object conditions)
		{
			using IDbConnection cnn = Database.CreateConnection();
			string sql = @"SELECT p.*, u.JmenoUzivatele, l_2.JmenoUser AS CreationUserName , pe.*, l_1.JmenoUser AS CreationUserName, i.IDInterakce, i.Datum
							FROM P_Permanentka p
							JOIN S_LoginUser l_2 ON l_2.IDLogin = p.Kdo
							JOIN S_Uzivatele u ON p.IDUzivatele = u.IDUzivatele
							LEFT JOIN P_PermanentkaEvidence pe ON p.IDPermanentka = pe.IDPermanentka
							LEFT JOIN S_LoginUser l_1 ON l_1.IDLogin = pe.Kdo
							LEFT JOIN P_Evidence e ON pe.IDEvidence = e.IDEvidence
							LEFT JOIN P_Interakce i ON e.IDInterakce = i.IDInterakce
							WHERE p.IDZaznamu = @RecordId AND p.PlatiDo >= @ValidTo;";

            var tickets = await cnn.QueryAsync<SeasonTicket, SeasonTicketEvidence, SeasonTicket>(sql,(tic, evid) => {
				tic.TicketEvidences.Add(evid);
                return tic;
            }, splitOn: "IDPermanentkaEvidence",param:conditions);

            return tickets.GroupBy(p => p.Id).Select(g =>
            {
                var grouped = g.First();
                grouped.TicketEvidences = g.Select(p => p.TicketEvidences.Single()).ToList();
				grouped.TicketEvidences.RemoveAll(t => t is null);
                return grouped;
            }).ToList(); 

		}
		public async Task<IEnumerable<SeasonTicket>> GetTisketOfClient(int clientId, int recordId, IDbTransaction tran)
		{
			string sql = @"SELECT *
							FROM P_Permanentka p
							LEFT JOIN P_PermanentkaEvidence pe ON p.IDPermanentka = pe.IDPermanentka
							WHERE p.IDZaznamu = @RecordId AND p.IDUzivatele = @ClientId AND p.PlatiDo >= CURRENT_TIMESTAMP;";

			var tickets = await tran.QueryAsync<SeasonTicket, SeasonTicketEvidence, SeasonTicket>(sql, (tic, evid) => {
				tic.TicketEvidences.Add(evid);
				return tic;
			}, splitOn: "IDPermanentkaEvidence", param: new{ RecordId = recordId, ClientId = clientId });

			return tickets.GroupBy(p => p.Id).Select(g =>
			{
				var grouped = g.First();
				grouped.TicketEvidences = g.Select(p => p.TicketEvidences.Single()).ToList();
				return grouped;
			}).ToList();
		}
		// ------------------ InsUp ---------------------

		public async Task<int?> Insert(SeasonTicket seasonTicket, IDbTransaction tran)
		{// Koupe cele permanentky
			return await tran.Connection.InsertAsync(seasonTicket,tran);
		}
		public async Task<int?> Insert(SeasonTicketEvidence seasonTicketEvidence, IDbTransaction tran)
		{
			return await tran.Connection.InsertAsync(seasonTicketEvidence, tran);
		}
	}
}
