using Dapper;
using Pobytne.Data;
using Pobytne.Data.Tables;
using Pobytne.Data.Tables.InteractionTables;
using Pobytne.Shared.Procedural;
using Pobytne.Shared.Procedural.FilterReports;
using Pobytne.Shared.Struct;
using System.Data;

namespace Pobytne.Server.Service
{
    public class InteractionService
    {
        private readonly InteractionTable _interTable;
        private readonly EvidenceTable _evidTable;
        private readonly CashRegisterTable _cashTable;
        private readonly RecordService _recordService;

        public InteractionService(InteractionTable interactiontable,EvidenceTable evidenceTable, CashRegisterTable cashRegisterTable, RecordService recordService)
        {
            _interTable = interactiontable;
            _evidTable = evidenceTable;
            _cashTable = cashRegisterTable;
            _recordService = recordService;
        }

        public async Task<int?> Insert(Interaction interaction)
        {
            int result = 0;

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

                    var id = await _interTable.InsUpTran(param,tran,cnn);
                    interaction.Id = id;
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
                        /// TODO: Dodelat odecitani ze skladu zasob
                        if (r.IsBalanceCheck)
                        {
                            //edit skladu
                            // kontrola zustatku + update zustatku?? atomicka operace?
                        }
                        result += await _evidTable.InsUpTran(param, tran, cnn);
                    }
                    foreach (var r in interaction.Records.Where(r => r.PriceAmount != 0))
                    {
                        param = _cashTable.GetParamsForTrans(r, delete);
                        param.AddDynamicParams(interactionParam);
                        param.Add("@IDPokladna", 0, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

                        result += await _cashTable.InsUpTran(param, tran, cnn);
                    }
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
            
            return result;
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
