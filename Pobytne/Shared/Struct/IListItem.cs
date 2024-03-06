namespace Pobytne.Shared.Struct
{
    public interface IListItem
    {
        int Id { get; set; }
        string Name { get; }
        string Description{  get; }
        DateTime CreationDate { get; set; }
        int CreationUserId { get; set; }
        string CreationUserName { get; set; }
        bool Active { get; }
    }
}
