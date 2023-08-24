using Pobytne.Data;
using Pobytne.Shared.Procedural;

namespace Pobytne.Server
{
	public class ModuleService
	{
		public async Task<List<Module>> GetModules(long licenseNumber)
		{
			var modules = ModuleTable.GetAll(licenseNumber).Result;
			if (modules is null)
				throw new Exception("No modules were loaded!");
			return modules;
		}
	}
}
