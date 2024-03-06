using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pobytne.Shared.Struct
{
    public interface IEmployeeTask_Record
    {
        string Name { get; }
        int Order { get; }
        int RecordPropertiesId { get; }
        string RecordPropertiesName { get; }
        string Note { get; }
        DateTime ValidFrom { get; set; }
        DateTime ValidTo { get; set; }
    }
}
