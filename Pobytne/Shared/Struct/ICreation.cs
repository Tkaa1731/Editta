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
        DateTime ValidFrom { get; set; }
        DateTime ValidTo { get; set; }
        int CreationUserId { get; set; }
        DateTime CreationDate { get; set; }
        string CreationUserName { get; set; }
    }
}
