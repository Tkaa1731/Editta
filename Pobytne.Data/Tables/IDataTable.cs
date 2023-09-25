using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pobytne.Data.Tables
{
    internal interface IDataTable<T>
    {
        Task<IEnumerable<T>> Select();
        Task<int> Delete(int id);
        Task<bool> Insert(T item, int editorId);
        Task<T?> Update(T item, int editorId);

    }
}
