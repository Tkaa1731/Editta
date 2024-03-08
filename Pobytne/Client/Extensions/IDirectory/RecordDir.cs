﻿using Havit.Blazor.Components.Web;
using Havit.Blazor.Components.Web.Bootstrap;
using Pobytne.Client.Services;
using Pobytne.Shared.Extensions;
using Pobytne.Shared.Procedural;
using Pobytne.Shared.Struct;

namespace Pobytne.Client.Extensions.IDirectory
{
	public class RecordDir(PobytneService service, Record record) : IDirectory
    {
        private readonly PobytneService _service = service;
		public readonly Record Record = record;


        public int Id => Record.Id;
		public string Name => Record.Name;
        public IconBase Icon { get; set; } = BootstrapIcon.Folder2Open;
        private List<Record> subRecords = [];
        public List<Record> SubRecords //Vsechny podzaznamy o jednu vrstvu nize
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

        public List<IDirectory> SubDirectories { get; set; } = [];//Slozky typu RecordDir

        public List<IListItem> ItemsList => SubRecords.Select(r => r as IListItem).ToList();// IListItem Pro Zobrazeni v listu

        public async Task Refresh()
        {
            await LoadData();
            //await OnExpanded();
        }

        public IListItem GetNew() => new Record()
        {
            Id = 0,
            ParentId = Record.Id,
            RootId = Record.RootId,
            ModuleId = Record.ModuleId,
            StructDepth = Record.StructDepth + 1,
            //CreationDate = DateTime.Now, // Implementovano pri submitu v FormModal komponente
            //CreationUserId = 1,// Implementovano pri submitu v FormModal komponente
            Order = this.subRecords.Count,//default hodnoty
            ValidFrom = DateTime.Now,// default hodnoty
            ValidTo = DateTime.Now.AddYears(1),//default hodnoty
        };
        private async Task LoadData()
        {
            object? response;
            if (Record.Id <= 0)// root
                response = await _service.GetAllAsync<Record>($"RecordsRoot?moduleId={Record.ModuleId}", Record.ModuleId);

            else
                response = await _service.GetAllAsync<Record>($"RecordsBranch?parentId={Record.Id}", Record.ModuleId);

            if (response is null)
                Console.WriteLine($"NO RESPONSE");
            else if (response is ErrorResponse error)
                Console.WriteLine($"{error.ErrorMessage}");
            else if (response is List<Record> list)
            {
                SubRecords = list;
            }
        }
        public async Task OnSelect()
        {
            if(SubRecords.Count <= 0)
                await LoadData();
        }

        public async Task OnExpanded()
        {
            foreach ( var rd in SubDirectories) 
               await rd.OnSelect();
        }
    }
}
