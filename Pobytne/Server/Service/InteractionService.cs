using Pobytne.Data;
using Pobytne.Data.Tables;
using Pobytne.Data.Tables.InteractionTables;
using Pobytne.Shared.Procedural;
using Pobytne.Shared.Procedural.FilterReports;
using System.Data;

namespace Pobytne.Server.Service
{
    public class InteractionService(InteractionTable interactiontable, EvidenceTable evidenceTable, CashRegisterTable cashRegisterTable, SeasonTicketTable seasonTicketTable, RecordService recordService)
    {
        private readonly InteractionTable _interTable = interactiontable;
        private readonly EvidenceTable _evidTable = evidenceTable;
        private readonly CashRegisterTable _cashTable = cashRegisterTable;
        private readonly SeasonTicketTable _ticketTable = seasonTicketTable;
        private readonly RecordService _recordService = recordService;

        public async Task Insert(Interaction interaction)
        {
            //SET Server time
            interaction.CreationDate = DateTime.Now;

            //insert Id = 0, delete = false
            interaction.Id = 0;
            bool delete = false;

            using IDbConnection cnn = Database.CreateConnection();
            cnn.Open();
            using (var tran = cnn.BeginTransaction())
            {
                try
                {
                    var param = _interTable.GetParamsForTrans(interaction, delete);
                    param.Add("@IDInterakce", 0, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

                    interaction.Id = await _interTable.InsUpTran(param,tran,cnn);
                    var interactionParam = new
                    {
                        IDInterakce = interaction.Id,
                        Kdo = interaction.CreationUserId,
                        Kdy = interaction.CreationDate,
                    };
                    foreach (var r in interaction.Records.Where(r => r.Adult != 0 || r.Child != 0 || r.Quantity != 0 || r.IsSeasonTicket))// KDY Evidence
                    {
                        param = _evidTable.GetParamsForTrans(r, delete);
                        param.AddDynamicParams(interactionParam);
                        param.Add("@IDEvidence", 0, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

                        var evidenceId = await _evidTable.InsUpTran(param, tran, cnn);

                        // Odecitani ze skladu zasob
                        if (r.IsBalanceCheck && r.Quantity != 0)
                        {
                            //edit skladu
                            int deduction = r.Quantity * -1;
                            await _recordService.UpdateStock(r.RecordId,deduction, tran);
                        }
                        if (r.IsSeasonTicket)
                        {
                            if (r.IsSeasonTicketPayment)
                            {
                                var seasonTicket = await _ticketTable.GetTisketOfClient(interaction.ClientId, r.RecordId, tran);
                                var validFrom = DateTime.Today;
                                var validTo = DateTime.Today.Month >= 7 ? new DateTime(validFrom.Year, 1, 31) : new DateTime(validFrom.Year, 6, 30);
                                if (seasonTicket.Any( s => s.ValidTo >= validFrom)) throw new Exception($"Pro daného uživatele existuje již platná permanetka k záznamu { r.Name} v období do {validTo.ToShortDateString()}");

                                var insertTicket = new SeasonTicket()
                                {
                                    ClientId = interaction.ClientId,
                                    RecordId = r.RecordId,
                                    Price = r.Price,
                                    Quantity = r.Quantity,
                                    ValidFrom = validFrom,
                                    ValidTo = validTo,
                                    CreationUserId = interaction.CreationUserId,
                                    CreationDate = interaction.CreationDate,
                                };
                                _ = await _ticketTable.Insert(insertTicket, tran) ?? throw new Exception("Vyskytla se chyba při vložení záznamu permanetky.");
                            }
                            else
                            {
                                var seasonTicket = await _ticketTable.GetTisketOfClient(interaction.ClientId, r.RecordId, tran);
                                if (!seasonTicket.Any()) throw new Exception("Pro daného uživatele neexistuje platná permanetka k záznamu " + r.Name);

                                var freeCapacityTicket = seasonTicket.FirstOrDefault(st => st.TicketEvidences.Count < st.Quantity) ?? throw new Exception("Pro daného uživatele neexistuje permanetka s volnou kapacitou k záznamu " + r.Name);

                                var insertTicketEvidence = new SeasonTicketEvidence()
                                {
                                    SeasonTicketId = freeCapacityTicket.Id,
                                    EvidenceId = evidenceId,
                                    CreationUserId = interaction.CreationUserId,
                                    CreationDate = interaction.CreationDate,
                                };
                                _ = await _ticketTable.Insert(insertTicketEvidence, tran) ?? throw new Exception("Vyskytla se chyba při evidenci vstupu na permanetku.");
                            }

                        }
                    }
                    foreach (var r in interaction.Records.Where(r => r.PriceAmount != 0))
                    {
                        param = _cashTable.GetParamsForTrans(r, delete);
                        param.AddDynamicParams(interactionParam);
                        param.Add("@IDPokladna", 0, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

                        var cashRegisterId = await _cashTable.InsUpTran(param, tran, cnn);
                        //TODO: Instert SeasonTicketPayment
                    }
                    tran.Commit();
                }
                catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
            }
			cnn.Close();
        }
        public async Task<IEnumerable<CashRegister>> GetFilteredReports(CashRegisterFilter filter)
        {
            var recordsId = await _recordService.GetAllSubRecords(filter.RecordsId);// Get All SubRecords
            filter.RecordsId.Clear();
            if(recordsId is not null)
                filter.RecordsId.AddRange(recordsId);

			var condition = _cashTable.HandleCondition(filter, out string SQL_Where);
			return await _cashTable.SelectByCondition(condition, SQL_Where);
        }
		public async Task<IEnumerable<Evidence>> GetFilteredReports(EvidenceFilter filter)
		{
			var recordsId = await _recordService.GetAllSubRecords(filter.RecordsId);// Get All SubRecords
			filter.RecordsId.Clear();
			if (recordsId is not null)
				filter.RecordsId.AddRange(recordsId);

			var condition = _evidTable.HandleCondition(filter, out string SQL_Where);
			return await _evidTable.SelectByCondition(condition, SQL_Where);
		}
	}
}
