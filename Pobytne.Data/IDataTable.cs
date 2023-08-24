using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pobytne.Data
{
    internal interface IDataTable<T>
    {
        public async Task<List<T>> Select() 
        {
            throw new NotImplementedException();
        }
        public async Task<bool> Delete(T item) 
        {
            throw new NotImplementedException();
        }
        public async Task<bool> Insert(T item) 
        {
            throw new NotImplementedException();
        }
        public async Task<T> Update(T item) 
        {
            throw new NotImplementedException();
        }

    }
}
