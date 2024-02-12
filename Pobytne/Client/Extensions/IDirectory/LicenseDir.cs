using Havit.Blazor.Components.Web;
using Havit.Blazor.Components.Web.Bootstrap;
using Pobytne.Client.Services;
using Pobytne.Shared.Procedural;
using Pobytne.Shared.Struct;
using License = Pobytne.Shared.Procedural.License;

namespace Pobytne.Client.Extensions.IDirectory
{
    internal class LicenseDir : IDirectory // Add/update module
    {
        private readonly PobytneService _service;
        public LicenseDir(PobytneService service, License license)
        {
            Modules = new List<IDirectory>() { };
            _service = service;
            License = license;
            Users = new UserDir(service, License.LicenseNumber);
        }
        public Pobytne.Shared.Procedural.License License { get; set; }
        public List<IDirectory> Modules { get; set; }
        public IDirectory Users { get; set; }
        public List<IListItem> ItemsList { 
            get {
                List<ModuleDir?> modules = Modules.Select(m => m as ModuleDir).ToList();
                return modules.Select(m => m.Module as IListItem).ToList();
            } 
        }
        public List<IDirectory> SubDirectories
        {
            get
            {
                List<IDirectory> result = new List<IDirectory>() { Users };
                return Modules.Concat(result).ToList();
            }
        }

        public string Name { get { return $"{License.NameOfOrganization} | {License.LicenseNumber}"; } }
        public IconBase Icon { get { return BootstrapIcon.Folder; } }

        public async Task AddNew() => await LoadData();
        private async Task LoadData()
        {
            var response = await _service.GetAllAsync<Module>($"?licenseNumber={License.LicenseNumber}",-1);
            List<Module> modules = new(); 
            if (response is null)
                Console.WriteLine($"NO RESPONSE");
            else if (response is ErrorResponse response1)
                Console.WriteLine($"{response1.ErrorMessage}");
            else if (response is List<Module> list)
                modules = list;

            Modules.Clear();
            foreach (Module m in modules)
                Modules.Add(new ModuleDir(_service, m));
        }
        public async Task OnSelect()
        {
            if (Modules.Count <= 0)
                await LoadData();
        }

        public IListItem GetNew() => new Module() { LicenseNumber = License.LicenseNumber };

        public Task OnExpanded()
        {
            throw new NotImplementedException();
        }
    }
}
