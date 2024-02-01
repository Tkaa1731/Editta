using Havit.Blazor.Components.Web;
using Havit.Blazor.Components.Web.Bootstrap;
using Pobytne.Client.Services;
using Pobytne.Shared.Procedural;
using Pobytne.Shared.Struct;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Pobytne.Client.Extensions.IDirectory
{
    public class RecordDir : IDirectory
    {
        private readonly PobytneService _service;
        public RecordDir( PobytneService service, Record record ) 
        {
            _service = service;
            _record = record;
        }
        public Record _record {  get; set; }

        public string Name => _record.Name;

        public IconBase Icon { get; set; } = BootstrapIcon.Folder2Open;
        private List<Record> subRecords { get; set; } = [];
        private List<Record> SubRecords 
        { 
            get { 
                return subRecords; 
            } 
            set {
                subRecords = value;

                SubDirectories.Clear();
                foreach (Record record in value)
                {
                    if (record.RecordType == ERecordType.Folder)
                        SubDirectories.Add(new RecordDir(_service, record));
                }
            } 
        }

        public List<IDirectory> SubDirectories { get; set; } = [];

        public List<IListItem> ItemsList => SubRecords.Select(r => r as IListItem).ToList();

        public Task AddNew()
        {
            throw new NotImplementedException();
        }

        public IListItem GetNew() => new Record()
        {
            Id = 0,
            ParentId = _record.Id,
            RootId = _record.RootId,
            ModuleId = _record.ModuleId,
            StructDepth = _record.StructDepth + 1,
            Order = this.subRecords.Count,
            //CreationDate = DateTime.Now,
            //CreationUserId = 1,// TODO: author id
            ValidFrom = DateTime.Now,
            ValidTo = DateTime.Now.AddYears(1),
        };

        public async Task OnSelect()
        {
            if(SubRecords.Count <= 0)
            {
                object? response;
                if(_record.Id <= 0)// root
                    response = await _service.GetAllAsync<Record>($"RecordsRoot?moduleId={_record.ModuleId}");

                else
                    response = await _service.GetAllAsync<Record>($"RecordsBranch?parentId={_record.Id}");

                if (response is null)
                    Console.WriteLine($"NO RESPONSE");
                else if (response is ErrorResponse response1)
                    Console.WriteLine($"{response1.ErrorMessage}");
                else if (response is List<Record> list)
                {
                    SubRecords = list;
                }
            }
        }

        public async Task OnExpanded()
        {
            foreach ( RecordDir rd in SubDirectories) 
            {
               await rd.OnSelect();
            }
        }
    }
}
