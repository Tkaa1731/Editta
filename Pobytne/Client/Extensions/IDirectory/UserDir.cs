using Havit.Blazor.Components.Web.Bootstrap;
using Havit.Blazor.Components.Web;
using Pobytne.Shared.Procedural;
using Pobytne.Shared.Struct;
using System.Net.Http.Json;
using Pobytne.Client.Services;
using System.ComponentModel;

namespace Pobytne.Client.Extensions.IDirectory
{
    internal class UserDir : IDirectory// add/update user
    {
        private readonly int _licenseNumber;
        private readonly PobytneService _service;
        public UserDir(PobytneService service, int licenseNumber)
        {
            _service = service;
            _licenseNumber = licenseNumber;
        }
        public string Name => "Users";
        public IconBase Icon => BootstrapIcon.People;
        public List<IListItem> ItemsList { get; set; } = new List<IListItem>() { };
        public List<IDirectory> Subdirectories => new List<IDirectory>();
        public async Task AddNew() => await LoadData();
        private async Task LoadData()
        {
            var response = await _service.GetAllAsync<User>($"UsersList?licenseNumber={_licenseNumber}");
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
                await LoadData();
        }

        public IListItem GetNew() => new User() { LicenseNumber = _licenseNumber };
    }
}
