using Havit.Blazor.Components.Web.Bootstrap;
using Havit.Blazor.Components.Web;
using Pobytne.Shared.Procedural;
using Pobytne.Shared.Struct;
using Pobytne.Client.Services;

namespace Pobytne.Client.Extensions.IDirectory
{
    internal class ModuleDir : IDirectory
    {
        private readonly PobytneService _service;
        public ModuleDir(PobytneService service, Module module)
        {
            _service = service;
            Module = module;
        }
        public Module Module { get; set; }
        public string Name =>  Module.ModuleName;
        public IconBase Icon => BootstrapIcon.FolderPlus;
        public List<IListItem> ItemsList { get; set; } = new();
        public List<IDirectory> SubDirectories => new();
        public async Task AddNew() => await LoadData();
        private async Task LoadData()
        {
            var response = await _service.GetAllAsync<User>($"UsersList?userOfModule={Module.Id}");
            List<User> users = new();

            if (response is null)
                Console.WriteLine($"NO RESPONSE");
            else if (response is ErrorResponse response1)
                Console.WriteLine($"{response1.ErrorMessage}");
            else if (response is List<User> list)
                users = list;

            ItemsList.Clear();
            if (users is not null)
                ItemsList = users.Select(u => u as IListItem).ToList();
        }
        public async Task OnSelect()
        {
            if (ItemsList.Count <= 0)
            {
                await LoadData();
            }
        }

        public IListItem GetNew() => new User() {
            AccessPermition = new List<Permition>{
                new Permition() { 
                    ModuleId = Module.Id ,
                    ModuleName = Module.Name ,
                } },
            LicenseNumber = Module.LicenseNumber
        };

        public Task OnExpanded()
        {
            throw new NotImplementedException();
        }
    }
}
