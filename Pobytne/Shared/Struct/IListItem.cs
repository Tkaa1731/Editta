using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pobytne.Shared.Struct
{
    public interface IListItem
    {
        int Id { get; set; }
        string Name { get; }
        string Description{  get; }
        DateTime CreationDate { get; set; }
        int CreationUserId { get; set; }
    }
}
