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
        DateTime ValidFrom { get;}
        DateTime ValidTo { get;}
        int CreationUserId { get;}
        DateTime CreationDate { get;}
        string CreationUserName { get;}
    }
}
