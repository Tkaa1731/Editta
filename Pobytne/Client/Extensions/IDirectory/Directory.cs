using Havit.Blazor.Components.Web;
using Havit.Blazor.Components.Web.Bootstrap;
using Pobytne.Client.Services;
using Pobytne.Shared.Extensions;
using Pobytne.Shared.Procedural.DTO;
using Pobytne.Shared.Struct;
using System.Collections.Generic;
using System.Reflection;

namespace Pobytne.Client.Extensions.IDirectory
{
    internal class Directory(PobytneService service, string dirName) : IDirectory
	{
		private readonly PobytneService _service = service;
        public string Name { get; set; } = dirName;
        public int Id => 0;
		public IconBase Icon => BootstrapIcon.Archive;
		private List<LicenseDir> Licenses { get; set; } = new List<LicenseDir>();

        public List<IDirectory> SubDirectories {
			get { 
				return Licenses.Select(l => l as IDirectory).ToList();
			} 
		}
		public List<IListItem> ItemsList
		{
			get
			{
				return Licenses.Select(l => l.License as IListItem).ToList();
			}
		}

		public async Task Refresh() => await LoadData();
		private async Task LoadData()
		{
			var response = await _service.GetAllAsync<License>(string.Empty,-1);
            List<License> licenses = [];

			if(response is null)
                Console.WriteLine($"NO RESPONSE");
            else if (response is ErrorResponse response1)
				Console.WriteLine($"{response1.ErrorMessage}");
			else if(response is List<License> list)
				licenses = list;

			Licenses.Clear();
			Licenses.AddRange(licenses.Select(l => new LicenseDir(_service, l)));
		}
		public async Task OnSelect()
		{
            if (Licenses.Count <= 0)
			{
				await LoadData();
				foreach (LicenseDir l in Licenses)
				{
					await l.OnSelect();
				}
			}

        }

		public IListItem GetNew() => new License();

        public Task OnExpanded()
        {
            throw new NotImplementedException();
        }

        public void Insert(IListItem item)
        {
            if(item is License l)
                Licenses.Add(new LicenseDir(_service, l));
        }

        public void Update(IListItem item)
        {
            var index = Licenses.FindIndex(i => i.Id == item.Id);
            if (index != -1 && item is License l)
            {
                Licenses[index].License = l;
            }
        }

        public void Delete(IListItem item)
        {
            var index = Licenses.FindIndex(i => i.Id == item.Id);
            if (index != -1 && item is License)
            {
                Licenses.RemoveAt(index);
            }
        }
    }
}
