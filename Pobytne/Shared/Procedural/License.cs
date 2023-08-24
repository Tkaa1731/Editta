using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations.Schema;


namespace Pobytne.Shared.Procedural
{
    [Serializable]
    public class License
    {
        [Key]
        public int Id { get; set; }
        public int LicenseNumber { get; set; }
        public int ICO { get; set; }
        public string NameOfOrganization { get; set; } = string.Empty;
        public int Version { get; set; }
        public bool Demo { get; set; }
        public bool IsPaid { get; set; }
        public bool IsBlocked { get; set; }
    }
}
