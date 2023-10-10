using Havit.Blazor.Components.Web.Bootstrap;
using Havit.Blazor.Components.Web;
using Pobytne.Shared.Procedural;
using Pobytne.Shared.Struct;
using System.Net.Http.Json;

namespace Pobytne.Client.Extensions.IDirectory
{
    public class ModuleDir : IDirectory // add/update permition for user
    {
        private readonly HttpClient _http;
        public ModuleDir(HttpClient httpClient, Module module)
        {
            _http = httpClient;
            Module = module;
        }
        public Module Module { get; set; }
        public string Name =>  Module.ModuleName;
        public IconBase Icon => BootstrapIcon.FolderPlus;
        public List<IListItem> ItemsList { get; set; } = new();
        public List<IDirectory> Subdirectories => new();
        public void AddNew(IListItem newItem)
        {
            ItemsList.Add(newItem);
        }
        private async Task LoadData()
        {
            if (ItemsList.Count <= 0)
            {
                var users = await _http.GetFromJsonAsync<List<User>>($"api/User/UsersList?userOfModule={Module.Id}");
                if (users is not null)
                    ItemsList = users.Select(u => u as IListItem).ToList();
            }
        }
        public async Task OnSelect()
        {
            await LoadData();
        }

        public IListItem GetNew() => new User() {
            AccessPermition = new List<Permition>{
                new Permition() { 
                    ModuleId = Module.Id 
                } },
            LicenseNumber = Module.LicenseNumber
        };

    }
}
