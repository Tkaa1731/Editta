using Havit.Blazor.Components.Web.Bootstrap;
using Havit.Blazor.Components.Web;
using Pobytne.Shared.Procedural;
using Pobytne.Shared.Struct;
using System.Net.Http.Json;

namespace Pobytne.Client.Extensions.IDirectory
{
    public class UserDir : IDirectory// add/update user
    {
        private readonly HttpClient _http;
        private readonly int _licenseNumber;
        public UserDir(HttpClient httpClient, int licenseNumber)
        {
            _http = httpClient;
            _licenseNumber = licenseNumber;
        }
        public string Name => "Users";
        public IconBase Icon => BootstrapIcon.People;
        public List<IListItem> ItemsList { get; set; } = new List<IListItem>() { };
        public List<IDirectory> Subdirectories => new List<IDirectory>();
        public async Task AddNew() => await LoadData();
        private async Task LoadData()
        {
            var users = await _http.GetFromJsonAsync<List<User>>($"api/User/UsersList?licenseNumber={_licenseNumber}");
            ItemsList.Clear();
            if (users is not null)
                ItemsList = users.Select(u => u as IListItem).ToList();
        }
        public async Task OnSelect()
        {
            if (ItemsList.Count <= 0)
                await LoadData();
        }

        public IListItem GetNew() => new User() { LicenseNumber = _licenseNumber };
    }
}
