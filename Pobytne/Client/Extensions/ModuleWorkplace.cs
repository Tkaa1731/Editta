namespace Pobytne.Client.Extensions
{
	public class ModuleWorkplace
	{
		public string ModuleId
		{ 
			get{
				return Id.ToString();
			}
			set
			{
				Id = int.Parse(value);
			}
		}
		public int Id {  get; set; }
		public string Name { get { return ModulesDic.TryGetValue(Id, out string? value) ? value : ""; } }
		public long LicenseNumber { get; set; }
		public string LicenseName { get; set; } = string.Empty;
		public int UserId { get; set; }
		public string UserName { get; set; } = string.Empty;
		public Dictionary<int, string> ModulesDic = [];
	}
}
