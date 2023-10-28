using Havit.Blazor.Components.Web;
using Havit.Blazor.Components.Web.Bootstrap;
using Pobytne.Shared.Procedural;
using Pobytne.Shared.Struct;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace Pobytne.Client.Extensions.IDirectory
{
	public class Directory : IDirectory
	{
		private readonly HttpClient _http;
		public string Name { get; set; }

		public IconBase Icon => BootstrapIcon.Archive;
		private List<LicenseDir> Licenses { get; set; } = new List<LicenseDir>();

		public Directory(HttpClient httpClient, string dirName) { 
			_http = httpClient;
			Name = dirName;
		}
		public List<IDirectory> Subdirectories {
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
			var licenses = await _http.GetFromJsonAsync<List<License>>("api/License");
			Licenses.Clear();
			if (licenses is not null)
				foreach (License l in licenses)
				{
					Licenses.Add(new LicenseDir(_http, l));
				}
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
    }
}
