using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pobytne.Shared.Struct
{
    public interface IFolder_Record : ICreation
    {
        string Name { get; }
        int Order { get; }
        string Note {  get; }
    }
}
