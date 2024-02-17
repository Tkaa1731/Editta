﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pobytne.Shared.Procedural;
using Dapper.Transaction;
using Dapper;
using Pobytne.Shared.Procedural.FilterReports;

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
		public async Task<IEnumerable<CashRegister>> SelectByCondition(DynamicParameters parameters, string sqlCondition)
		{
			using (IDbConnection cnn = new SqlConnection(Tools.GetConnectionString()))// TODO: Odstranit Top 20
			{
				string sql = @" SELECT TOP 20 
									p.*,i.NazevInterakce, i.IDUzivatele, i.Datum, i.IDModulu, i.IDTypuPlatby, 
									z.Nazev, z.IDZaznamuVlastnosti AS RecordPropertyId,u.JmenoUzivatele,l.JmenoUser AS CreationUserName,
									zv.Nazev AS RecordPropertyName, zv.UcetA AS AccountA, zv.UcetS AS AccountS
								FROM P_Pokladna p
								JOIN P_Interakce i ON i.IDInterakce = p.IDInterakce
								JOIN S_Uzivatele u ON i.IDUzivatele = u.IDUzivatele	
								JOIN S_Zaznamy z ON p.IDZaznamu = z.IDZaznamu
								JOIN S_LoginUser l ON l.IDLogin = p.Kdo 
								LEFT JOIN S_ZaznamyVlastnosti zv ON zv.IDZaznamuVlastnosti = z.IDZaznamuVlastnosti";
				sql += sqlCondition;
				return await cnn.QueryAsync<CashRegister>(sql, parameters);
			}
		}
		public DynamicParameters HandleCondition(CashRegisterFilter filter, out string condition)
		{
			var result = new DynamicParameters();
			var strBuilder = new StringBuilder();
			strBuilder.Append(" WHERE ");

			result.Add("@DateStart", filter.From);
			result.Add("@DateEnd", filter.To);
			strBuilder.Append(" Datum BETWEEN @DateStart AND @DateEnd ");

				if (filter.ModuleId is not null && filter.ModuleId > 0)
				{
					result.Add("@IDMudulu", filter.ModuleId);
					strBuilder.Append(" AND i.IDModulu = @IDMudulu ");
				}
				if (filter.ClientId is not null && filter.ClientId > 0)
				{
					result.Add("@IDUzivatele", filter.ClientId);
					strBuilder.Append(" AND i.IDUzivatele = @IDUzivatele ");
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
