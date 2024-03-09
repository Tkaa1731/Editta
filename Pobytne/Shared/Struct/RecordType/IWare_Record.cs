namespace Pobytne.Shared.Struct
{
    public interface IWare_Record
    {
        string Name { get; set; }
        int Order { get; set; }
        int Stock { get; set; }
        int? RecordAttributeId { get; set; }
        string RecordAttributeName { get; set; }
        int Quantity { get; set; }
        float Price { get; set; }
        bool IsSeasonTicket { get; set; }
        int GroupQuantity { get; set; }
        float GroupPrice { get; set; }
        bool IsClientRequired { get; set; }
        bool IsPriceRequired { get; set; }
        bool IsBalanceCheck {  get; set; }
        string Note { get; set; }
        DateTime ValidFrom { get; set; }
        DateTime ValidTo { get; set; }
    }
}
