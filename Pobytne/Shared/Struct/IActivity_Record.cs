using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pobytne.Shared.Struct
{
    public interface IActivity_Record : ICreation
    {
        string Name { get; set; }
        int Order { get; set; }
        int RecordPropertiesId { get; set; }
        string RecordPropertiesName { get; set; }
        int Adult {  get; set; }
        int Child {  get; set; }
        int Quantity { get; set; }
        float Price { get; set; }
        bool IsClientRequired { get; set; }
        bool IsPriceRequired { get; set; }
        bool IsSeasonTicket { get; set; }
        int GroupQuantity { get; set; }
        float GroupPrice { get; set; }
        string Note { get; set; }
    }
}
