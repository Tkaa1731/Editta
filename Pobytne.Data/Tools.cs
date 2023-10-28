using System.Configuration;

namespace Pobytne.Data
{
    internal static class Tools // TODO: Local a Test nejsou stejne ... jine columns napr v Modulech
    {
        public static string GetConnectionString(string name = "PobytneTest")
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }

}