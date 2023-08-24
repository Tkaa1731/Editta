using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Pobytne.Shared.Procedural;

namespace Pobytne.Shared.Authentication
{
    [Serializable]
    public class UserAccount : User
    {
        public string Token { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }
        public DateTime ExpiryTimeStamp { get; set; }
        public bool CheckPassword(string password) => _password == password;
        public override string ToString()
        {
            return $"UserName: {UserName} | AccessPermition: {AccessPermition.Count} ";
        }
    }
}
