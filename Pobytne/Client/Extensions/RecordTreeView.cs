using Pobytne.Client.Services;
using Pobytne.Shared.Extensions;
using Pobytne.Shared.Procedural;
using Pobytne.Shared.Procedural.Filters;

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
		public bool SeasonTickedSelected = false;
		public RecordTreeView(PobytneService service)
		{
			_service = service;
		}
		public async Task LoadData()
		{
			var response = await _service.GetAllAsync<Record>("?", Record.ModuleId,new RecordFilter() { ParentId = Record.Id });

			if (response is null)
				Console.WriteLine($"NO RESPONSE");
			else if (response is ErrorResponse error)
				Console.WriteLine($"{error.ErrorMessage}");
			else if (response is List<Record> list)
			{
				SubRecords.Clear();
				foreach (Record r in list)
					SubRecords.Add(new RecordTreeView(_service) { Record = r});
			}
		}
	}
}
