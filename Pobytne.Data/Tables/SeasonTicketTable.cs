using Dapper;
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
							FROM P_PermanentkaEvidence pe
							JOIN P_Permanentka p ON p.IDPermanentka = pe.IDPermanentka
							JOIN S_Uzivatele u ON p.IDUzivatele = u.IDUzivatele
							JOIN S_LoginUser l_1 ON l_1.IDLogin = pe.Kdo
							JOIN S_LoginUser l_2 ON l_2.IDLogin = p.Kdo
							JOIN P_Evidence e ON pe.IDEvidence = e.IDEvidence
							JOIN P_Interakce i ON e.IDInterakce = i.IDInterakce
							WHERE p.IDZaznamu = @RecordId AND p.PlatiDo >= @ValidTo;";

            var tickets = await cnn.QueryAsync<SeasonTicket, SeasonTicketEvidence, SeasonTicket>(sql,(tic, evid) => {
                tic.TicketEvidences.Add(evid);
                return tic;
            }, splitOn: "IDPermanentkaEvidence",param:conditions);

            return tickets.GroupBy(p => p.Id).Select(g =>
            {
                var grouped = g.First();
                grouped.TicketEvidences = g.Select(p => p.TicketEvidences.Single()).ToList();
                return grouped;
            });

        }
	}
}
