using Dapper;
using Dapper.Transaction;
using Pobytne.Shared.Procedural;
using System.Data;
using System.Data.SqlClient;

namespace Pobytne.Data.Tables.InteractionTables
{
    public class InteractionTable //P_Interakce
    {
        public async Task<int> InsUpTran(DynamicParameters param, IDbTransaction tran, IDbConnection cnn)
        {
            string interactionSQL = "p_sp_Interakce_InsUp";

            if (tran is not null && cnn is not null)
                if(cnn.State == ConnectionState.Open)
                {
                    int success = await tran.ExecuteAsync(interactionSQL, param, commandType: CommandType.StoredProcedure);
                    if (success == 1)
                        return param.Get<int>("@IDInterakce");

                    throw new Exception("Failed 'p_sp_Interakce_InsUp'");
                }
            throw new Exception($"Closed connection for exec '{interactionSQL}'");

        }
        public DynamicParameters GetParamsForTrans(Interaction item, bool delete)
        {
            var result = new DynamicParameters();
            object? template = new
            {
                //IDInterakce = item.Id,
                IDModulu = item.ModuleId,
                IDUzivatele = item.ClientId,
                IDTypuPlatby = item.PaymentId,
                Datum = item.InteractionDate,
                NazevInterakce = item.InteractionName,
                Kdo = item.CreationUserId,
                Kdy = item.CreationDate,
                Smazat = delete ? 1 : 0,
            };
            result.AddDynamicParams(template);
            return result;
        }
    }
}
