using Havit.Blazor.Components.Web.Bootstrap;
using Havit.Blazor.Components.Web;
using Pobytne.Shared.Struct;
using Pobytne.Client.Services;
using Pobytne.Shared.Procedural.DTO;
using Pobytne.Shared.Extensions;

namespace Pobytne.Client.Extensions.IDirectory
{
    internal class ModuleDir(PobytneService service, Module module) : IDirectory
    {
        private readonly PobytneService _service = service;

        public Module Module { get; set; } = module;
        public string Name =>  Module.ModuleName;
        public int Id => Module.Id;
        public IconBase Icon => BootstrapIcon.FolderPlus;
        public List<IListItem> ItemsList { get; set; } = [];
        public List<IDirectory> SubDirectories => []; //NO subdir
        public async Task Refresh() => await LoadData();
        private async Task LoadData()
        {
            var response = await _service.GetAllAsync<Permition>($"?moduleId={Module.Id}", -1);
            List<Permition> permitions = [];

            if (response is null)
                Console.WriteLine($"NO RESPONSE");
            else if (response is ErrorResponse response1)
                Console.WriteLine($"{response1.ErrorMessage}");
            else if (response is List<Permition> list)
                permitions = list;

            ItemsList.Clear();
            if (permitions is not null)
                ItemsList = permitions.Select(p => p as IListItem).ToList();
        }
        public async Task OnSelect()
        {
            if (ItemsList.Count <= 0)
            {
                await LoadData();
            }
        }

        public IListItem GetNew() => new Permition(){ ModuleId = Module.Id, ModuleName = Module.Name};

        public Task OnExpanded()
        {
            throw new NotImplementedException();
        }

        public void Insert(IListItem item)
        {
            ItemsList.Add(item);
        }

        public void Update(IListItem item)
        {
            var index = ItemsList.FindIndex(i => i.Id == item.Id);
            if (index != -1)
            {
                ItemsList[index] = item;
            }
        }
    }
}
