using Dapper.FluentMap;
using Pobytne.Data.Mappers;

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