using System.ComponentModel.DataAnnotations;

namespace Pobytne.Shared.Struct
{
    public abstract class ACreation
    { // Kdo, Kdy, LoginKdo
        public int CreationUserId { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        [Editable(false)]
        public string CreationUserName { get; set; } = string.Empty;
    }
}
