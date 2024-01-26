using Pobytne.Shared.Struct;

namespace Pobytne.Shared.Procedural
{
    public class Client : IContact, ICreation, IListItem
    {
        public int Id { get; set; }
        public int ModuleId {  get; set; }
        public string Name { get; set; } = String.Empty;
        public bool ThroughoutOrganization { get; set; }
        public string Description { get; set; } = String.Empty;

        public string City { get; set; } = String.Empty;
        public string Street { get; set; } = String.Empty;
        public string PostNumber { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string PhoneNumber { get; set; } = String.Empty;

        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public bool Valid {  get; set; }
        public int CreationUserId { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreationUserName { get; set; } = String.Empty;
        public bool Active => ValidTo >= DateTime.Now;
    }
}
