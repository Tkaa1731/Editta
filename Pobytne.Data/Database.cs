using Dapper.FluentMap;
using Pobytne.Data.Mappers;
using System.Data.SqlClient;

namespace Pobytne.Data
{
    public class Database
    {
        public static void OnInitialize()
        {
            FluentMapper.Initialize(configure =>
            {
                configure.AddMap(new UserMapper());
                configure.AddMap(new ModuleMapper());
                configure.AddMap(new LicenseMapper());
                configure.AddMap(new PermitionMapper());
                configure.AddMap(new ClientMapper());
                configure.AddMap(new RecordMapper());
                configure.AddMap(new PaymentMapper());

            });
        }
        public static SqlConnection CreateConnection()
        {
            return new SqlConnection(Tools.GetConnectionString());
        }

    }

}