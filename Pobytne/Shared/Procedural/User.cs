//using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pobytne.Shared.Procedural
{
    [Serializable]
    [Table("S_LoginUser")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        //[Write(false)]
        public string Password { set { _password = value; } }
        protected string _password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        //[Write(false)]
        public List<Permition> AccessPermition { get; set; } = new List<Permition>();
        [ForeignKey("License")]
        public int LicenseId { get; set; }
        public int LicenseNumber { get; set; }
        public bool Valid { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        
    }
}
