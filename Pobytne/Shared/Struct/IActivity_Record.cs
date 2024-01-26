using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pobytne.Shared.Struct
{
    public interface IActivity_Record : ICreation
    {
        string Name { get; }
        int Order { get; }
        int RecordPropertiesId { get; }
        string RecordPropertiesName { get; }
        int Adult {  get; }
        int Child {  get; }
        int Quantity { get; }
        int Price { get; }
        bool IsClientRequired { get; }
        bool IsPriceRequired { get; }
        bool IsSeasonTicket { get; }
        int GroupQuantity { get; }
        int GroupPrice { get; }
        string Note { get; }
    }
}
