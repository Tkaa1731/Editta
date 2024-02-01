using Pobytne.Shared.Procedural;

namespace Pobytne.Shared.Authentication
{
    [Serializable]
    public class UserAccount
    {
        public User User { get; }
        public string Token { get; set; } = string.Empty;
        public string Refresh { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }
        public DateTime ExpiryTimeStamp { get; set; }
        public UserAccount(User user) => User = user;
    }
}
