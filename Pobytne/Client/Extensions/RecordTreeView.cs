using Pobytne.Client.Services;
using Pobytne.Shared.Procedural;
using System.ComponentModel;
using System.Reflection;

namespace Pobytne.Client.Extensions
{
	public class RecordTreeView
	{
		private readonly PobytneService _service;
		public required Record Record {  get; set; }
		public List<RecordTreeView> SubRecords { get; set; } = [];
		public int Depth { get {  return Record.StructDepth; } }
		public bool Active { 
			get 
			{
				bool active = true;
				if (Record.RecordType == Pobytne.Shared.Struct.ERecordType.Ware)// kontrola mnostvi polozek na skladu
					active &= (Record.Stock > 0);

				active &= (Record.ValidTo >= DateTime.Now);// aktivni polozka

				return active;
			} 
		}
		public RecordTreeView(PobytneService service)
		{
			_service = service;
		}
		public async Task LoadData()
		{
			var response = await _service.GetAllAsync<Record>($"RecordsBranch?parentId={Record.Id}", Record.ModuleId);

			if (response is null)
				Console.WriteLine($"NO RESPONSE");
			else if (response is ErrorResponse response1)
				Console.WriteLine($"{response1.ErrorMessage}");
			else if (response is List<Record> list)
			{
				SubRecords.Clear();
				foreach (Record r in list)
					SubRecords.Add(new RecordTreeView(_service) { Record = r});
			}
		}
	}
}
