using Pobytne.Shared.Struct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pobytne.Shared.Procedural.Filters
{
    public class RecordFilter(int start = 0, int count = int.MaxValue, string subfix = "",bool valid = false) : LazyList(typeof(Record), start, count, subfix, valid)
    {
        public int RecordId { get; set; }
        public int RootId { get; set; }
        public int ParentId { get; set; }
        public int ModuleId { get; set; }
        public bool IsSeasonTicket { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
