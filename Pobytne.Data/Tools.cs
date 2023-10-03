using System.Configuration;

namespace Pobytne.Data
{
    internal static class Tools
    {
        public static string GetConnectionString(string name = "PobytneLocal")
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }

}