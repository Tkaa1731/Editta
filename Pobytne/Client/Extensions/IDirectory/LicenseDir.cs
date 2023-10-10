using Havit.Blazor.Components.Web;
using Havit.Blazor.Components.Web.Bootstrap;
using Pobytne.Shared.Procedural;
using Pobytne.Shared.Struct;
using System.Net.Http.Json;

namespace Pobytne.Client.Extensions.IDirectory
{
    public class LicenseDir : IDirectory // Add/update module
    {
        private readonly HttpClient _http;
        public LicenseDir(HttpClient httpClient, License license)
        {
            Modules = new List<IDirectory>() { };
            _http = httpClient;
            License = license;
            Users = new UserDir(httpClient, License.LicenseNumber);
        }
        public License License { get; set; }
        public List<IDirectory> Modules { get; set; }
        public IDirectory Users { get; set; }
        public List<IListItem> ItemsList { 
            get {
                List<ModuleDir?> modules = Modules.Select(m => m as ModuleDir).ToList();
                return modules.Select(m => m.Module as IListItem).ToList();
            } 
        }
        public List<IDirectory> Subdirectories
        {
            get
            {
                List<IDirectory> result = new List<IDirectory>() { Users };
                return Modules.Concat(result).ToList();
            }
        }

        public string Name { get { return $"{License.NameOfOrganization} | {License.LicenseNumber}"; } }
        public IconBase Icon { get { return BootstrapIcon.Folder; } }

        public void AddNew(IListItem newItem)
        {
            Module? m = newItem as Module;
            if (m is not null) 
                Modules.Add(new ModuleDir(_http, m));
        }
        private async Task LoadData()
        {
            var modules = await _http.GetFromJsonAsync<List<Module>>($"api/Module/ModulesList?licenseNumber={License.LicenseNumber}");
            if (modules is not null)
                foreach (Module m in modules)
                    Modules.Add(new ModuleDir(_http, m));
        }
        public async Task OnSelect()
        {
            if (Modules.Count <= 0)
                await LoadData();
        }

        public IListItem GetNew() => new Module() { LicenseNumber = License.LicenseNumber };
    }
}
