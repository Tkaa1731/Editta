namespace Pobytne.Shared.Struct.RecordType
{
    public interface IFolder_Record
    {
        string Name { get; set; }
        ERecordType RecordType { get; set; }
        int Order { get; set; }
        string Note { get; set; }
        DateTime ValidFrom { get; set; }
        DateTime ValidTo { get; set; }
    }
}
