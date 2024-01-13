using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pobytne.Shared.Struct
{
    public interface IWare_Record : ICreation
    {
        string Name { get; }
        int Order { get; }
        int Stock { get; }
        int RecordPropertiesId { get; }
        string RecordPropertiesName { get; }
        int Quantity { get; }
        int Price { get; }
        bool IsCustomerRequired { get; }
        bool IsPriceRequired { get; }
        bool IsBalanceCheck {  get; }
        string Note { get; }
    }
}
