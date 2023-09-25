using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pobytne.Shared.Struct
{
    public interface ICreation
    {
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public int CreationUserId { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreationUserName { get; set; }
    }
}
