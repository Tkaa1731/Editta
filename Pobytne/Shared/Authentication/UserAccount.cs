using Pobytne.Shared.Procedural;

namespace Pobytne.Shared.Authentication
{
    [Serializable]
    public class UserAccount
    {
        public User User { get; }
        public string Token { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }
        public DateTime ExpiryTimeStamp { get; set; }
        public override string ToString()
        {
            return $"UserName:  {User.UserName}| AuthorizedTo: {ExpiryTimeStamp}";
        }
        public UserAccount(User user) => User = user;
    }
}
