using Dapper;
using Dapper.FluentMap;
using Pobytne.Data.mappers;
using Pobytne.Shared.Procedural;
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
            });
        }
    }

}