using Havit.Blazor.Components.Web;
using Havit.Blazor.Components.Web.Bootstrap;
using Pobytne.Client.Services;
using Pobytne.Shared.Procedural;
using Pobytne.Shared.Struct;
using System.Collections.Generic;

namespace Pobytne.Client.Extensions.IDirectory
{
	internal class Directory : IDirectory
	{
		private readonly PobytneService _service;
		public string Name { get; set; }
		public int Id => 0;
		public IconBase Icon => BootstrapIcon.Archive;
		private List<LicenseDir> Licenses { get; set; } = new List<LicenseDir>();

		public Directory(PobytneService service, string dirName) { 
			_service = service;
			Name = dirName;
		}
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

		public async Task AddNew() => await LoadData();
		private async Task LoadData()
		{
			var response = await _service.GetAllAsync<License>("",-1);
            List<License> licenses = new();

			if(response is null)
                Console.WriteLine($"NO RESPONSE");
            else if (response is ErrorResponse response1)
				Console.WriteLine($"{response1.ErrorMessage}");
			else if(response is List<License> list)
				licenses = list;

			Licenses.Clear();
			foreach (License l in licenses)
				Licenses.Add(new LicenseDir(_service, l));
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
    }
}
