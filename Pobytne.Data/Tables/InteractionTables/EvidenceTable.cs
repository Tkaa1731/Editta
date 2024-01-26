using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pobytne.Shared.Procedural;
using System.Security.Policy;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Dapper.Transaction;
using Dapper;

namespace Pobytne.Data.Tables.InteractionTables
{
    public class EvidenceTable //P_Evidence
    {
        public async Task<int> InsUpTran(DynamicParameters param, IDbTransaction tran, IDbConnection cnn)
        {
            string evidenceSQL = "p_sp_Evidence_InsUp";

            if (tran is not null && cnn is not null)
                if (cnn.State == ConnectionState.Open)
                {
                    int success = await tran.ExecuteAsync(evidenceSQL, param, commandType: CommandType.StoredProcedure);
                    if (success == 1)
                        return 1;

                    throw new Exception("Failed 'p_sp_Interakce_InsUp'");
                }
            throw new Exception($"Closed connection for exec '{evidenceSQL}'");
        }
        public DynamicParameters GetParamsForTrans(InteractionRecordItem record, bool delete)
        {
            var result = new DynamicParameters();
            object? template = new
            {
                //IDEvidence = 0,
                IDZaznamu = record.RecordId,
                //IDInterakce = item.Id,
                Poradi = record.Order,
                Jednotka = record.Quantity,
                Dospely = record.Adult,
                Dite = record.Child,
                //Kdo = item.CreationUserId,
                //Kdy = item.CreationDate ,
                Smazat = delete ? 1 : 0,
            };
            result.AddDynamicParams(template);
            return result;
        }
    }
}
