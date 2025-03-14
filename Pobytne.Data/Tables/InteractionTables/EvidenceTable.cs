﻿using System.Data;
using Pobytne.Shared.Procedural;
using Dapper.Transaction;
using Dapper;
using System.Data.SqlClient;
using Pobytne.Shared.Procedural.FilterReports;
using System.Text;

namespace Pobytne.Data.Tables.InteractionTables
{
	public class EvidenceTable //P_Evidence
    {
        public async Task<int> GetCount(object conditions)
        {
            using IDbConnection cnn = Database.CreateConnection();
            return await cnn.RecordCountAsync<Evidence>(conditions);
        }
        public async Task<int> InsUpTran(DynamicParameters param, IDbTransaction tran, IDbConnection cnn)
        {
            string evidenceSQL = "p_sp_Evidence_InsUp";

            if (tran is not null && cnn is not null)
                if (cnn.State == ConnectionState.Open)
                {
                    int success = await tran.ExecuteAsync(evidenceSQL, param, commandType: CommandType.StoredProcedure);
                    if (success == 1)
                        return param.Get<int>("@IDEvidence"); ;

                    throw new Exception("Failed 'p_sp_Interakce_InsUp'");
                }
            throw new Exception($"Closed connection for exec '{evidenceSQL}'");
        }
        public async Task<IEnumerable<Evidence>> SelectByCondition(DynamicParameters parameters, string sqlCondition)
        {
			using (IDbConnection cnn = Database.CreateConnection())
			{
				string sql = @" SELECT
                                    e.*, i.NazevInterakce, i.IDUzivatele, i.Datum, i.IDModulu, u.JmenoUzivatele, 
                                    z.Nazev, z.IDZaznamuVlastnosti AS RecordAttributeId, l.JmenoUser AS CreationUserName,
                                    zv.Nazev AS RecordAttributeName, zv.UcetA AS AccountA, zv.UcetS AS AccountS
                                FROM P_Evidence e
                                JOIN P_Interakce i ON i.IDInterakce = e.IDInterakce
                                LEFT JOIN S_Uzivatele u ON i.IDUzivatele = u.IDUzivatele	
                                JOIN S_Zaznamy z ON e.IDZaznamu = z.IDZaznamu
                                JOIN S_LoginUser l ON l.IDLogin = e.Kdo 
                                LEFT JOIN S_ZaznamyVlastnosti zv ON zv.IDZaznamuVlastnosti = z.IDZaznamuVlastnosti";
                sql += sqlCondition;
				return await cnn.QueryAsync<Evidence>(sql, parameters);
			}
		}
        public DynamicParameters HandleCondition(EvidenceFilter filter, out string condition)
        {
			var result = new DynamicParameters();
            var strBuilder = new StringBuilder();
            strBuilder.Append(" WHERE ");

            result.Add("@DateStart", filter.From);
            result.Add("@DateEnd", filter.To);
            strBuilder.Append(" Datum BETWEEN @DateStart AND @DateEnd ");

            result.Add("@IDMudulu", filter.ModuleId);
            strBuilder.Append(" AND i.IDModulu = @IDMudulu ");

			if (filter.ClientId is not null && filter.ClientId > 0)
			{
				result.Add("@IDUzivatele", filter.ClientId);
				strBuilder.Append(" AND i.IDUzivatele = @IDUzivatele ");
			}
			if (filter.RecordsId.Count > 0)
			{
				result.Add("@RecordId", filter.RecordsId);
				strBuilder.Append(" AND p.IDZaznamu IN @RecordId ");
			}

			strBuilder.Append(';');
			condition = strBuilder.ToString();
            return result;
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
