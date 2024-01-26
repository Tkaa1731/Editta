using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pobytne.Shared.Procedural;
using Dapper.Transaction;
using Dapper;

namespace Pobytne.Data.Tables.InteractionTables
{
    public class CashRegisterTable // P_Pokladna
    {
        public async Task<int> InsUpTran(DynamicParameters param, IDbTransaction tran, IDbConnection cnn)
        {
            string cashRegisterSQL = "p_sp_Pokladna_InsUp";

            if (tran is not null && cnn is not null)
                if (cnn.State == ConnectionState.Open)
                {
                    int success = await tran.ExecuteAsync(cashRegisterSQL, param, commandType: CommandType.StoredProcedure);
                    if (success == 1)
                        return 1;

                    throw new Exception("Failed 'p_sp_Interakce_InsUp'");
                }
            throw new Exception($"Closed connection for exec '{cashRegisterSQL}'");
        }
        public DynamicParameters GetParamsForTrans(InteractionRecordItem record, bool delete)
        {
            var result = new DynamicParameters();
            object? template = new
            {
                //IDPokladna = 0,
                IDZaznamu = record.RecordId,
                //IDInterakce = item.Id,
                Poradi = record.Order,
                Castka = record.PriceAmount,
                //Kdo = item.CreationUserId,
                //Kdy = item.CreationDate,
                Smazat = delete ? 1 : 0,
            };
            result.AddDynamicParams(template);
            return result;
        }
    }
}
