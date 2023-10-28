using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pobytne.Shared.Authentication
{
    public class LoginRequest
    {
        [Required(ErrorMessage ="Enter user name")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage ="Enter password")]
        public string Password { get; set; } = string.Empty;
    }
}
