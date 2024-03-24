using Havit.Blazor.Components.Web;
using Havit.Blazor.Components.Web.Bootstrap;
using Pobytne.Client.Services;
using Pobytne.Shared.Extensions;
using Pobytne.Shared.Procedural.DTO;
using Pobytne.Shared.Struct;
using License = Pobytne.Shared.Procedural.DTO.License;

namespace Pobytne.Client.Extensions.IDirectory
{
    internal class LicenseDir : IDirectory // Add/update module
    {
        private readonly PobytneService _service;
        public LicenseDir(PobytneService service, License license)
        {
            Modules = [];
            _service = service;
            License = license;
            Users = new UserDir(service, License.LicenseNumber);
        }
        public License License { get; set; }
        public int Id => License.Id;
        public List<ModuleDir> Modules { get; set; }
        public IDirectory Users { get; set; }
        public List<IListItem> ItemsList  => Modules.Select(m => m!.Module as IListItem).ToList();
        
        public List<IDirectory> SubDirectories
        {
            get
            {
                List<IDirectory> result = [Users];
                return [.. Modules, .. result];
            }
        }

        public string Name => $"{License.NameOfOrganization} | {License.LicenseNumber}";
        public IconBase Icon { get { return BootstrapIcon.Folder; } }

        public async Task Refresh() => await LoadData();
        private async Task LoadData()
        {
            var response = await _service.GetAllAsync<Module>($"?licenseNumber={License.LicenseNumber}",-1);
            List<Module> modules = []; 
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

        public void Insert(IListItem item)
        {
            if(item is Module m)
                Modules.Add(new ModuleDir(_service, m));
        }

        public void Update(IListItem item)
        {
            var index = Modules.FindIndex(i => i.Id == item.Id);
            if (index != -1 && item is Module m)
            {
                Modules[index].Module = m;
            }
        }

        public void Delete(IListItem item)
        {
            var index = Modules.FindIndex(i => i.Id == item.Id);
            if (index != -1 && item is Module m)
            {
                Modules.RemoveAt(index);
            }
        }
    }
}
