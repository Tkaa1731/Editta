using Pobytne.Shared.Struct;

namespace Pobytne.Shared.Procedural
{
    public class InteractionRecordItem
    {
        public required int RecordId {  get; set; }
        public required string Name { get; set; }
        public required int Quantity { get; set; }
        public required float Price { get; set; }
        public required int Adult { get; set; }
        public required int Child { get; set; }
        public required int Order { get; set; }
        public required bool IsBalanceCheck {  get; set; }
        public required bool IsClientRequired {  get; set; }
        public required ERecordType RecordType { get; set; }
        public float PriceAmount 
        { 
            get 
            { 
                if(RecordType == ERecordType.Ware)
                    return Price * Quantity;
                return Price;
            } 
        }
    }
}
