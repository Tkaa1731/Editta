using System.Data;
using System.Data.SqlClient;

namespace Pobytne.Data
{
    internal static class Database
    {
        private readonly static string _databaseName = "PobytneTest";
        private readonly static string _userName = "UserPobytneTest";
        private readonly static string _password = "HesloPobytne23+";
        private readonly static string _serverAddress = "pobytne.cz,3341";
        private readonly static string _connectionString = $"Server={_serverAddress};Database={_databaseName};User Id={_userName};Password={_password};";

        public static async Task<List<T>> Select<T>(string sql, Func<IDataRecord,T,T> parserFunction) where T : class, new()
        {
            List<T> list = new List<T>();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(sql, connection))
                {
                    var result = command.ExecuteReader();
                    while (result.Read())
                    {
                        T item = new T();   
                        list.Add(parserFunction(result,item));
                    }
                }
                await connection.CloseAsync();
            }
            return list;
        }
    }
}