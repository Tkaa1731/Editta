using Pobytne.Shared.Struct;

namespace Pobytne.Shared.Procedural
{
	public class RecordAttribute : ACreation
	{
		public int Id {  get; set; }
		public int ModuleId {  get; set; }
		public ERecordType Type { get; set; }
		public string Name { get; set; } = string.Empty;
		public string AccountA { get; set; } = string.Empty;
		public string AccountS { get; set; } = string.Empty;
		public string CentreNumber { get; set; } = string.Empty;
		public string OrderNumber { get; set; } = string.Empty;
		public string ProjectNumber { get; set; } = string.Empty;
	}
}
