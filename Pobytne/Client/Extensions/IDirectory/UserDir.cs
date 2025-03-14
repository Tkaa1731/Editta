﻿using Havit.Blazor.Components.Web.Bootstrap;
using Havit.Blazor.Components.Web;
using Pobytne.Shared.Struct;
using System.Net.Http.Json;
using Pobytne.Client.Services;
using System.ComponentModel;
using Pobytne.Shared.Procedural.DTO;
using Pobytne.Shared.Extensions;

namespace Pobytne.Client.Extensions.IDirectory
{
    internal class UserDir(PobytneService service, int licenseNumber) : IDirectory// add/update user
    {
        private readonly PobytneService _service = service;
        private readonly int _licenseNumber = licenseNumber;


        public string Name => "Users";
        public int Id => 0;
        public IconBase Icon => BootstrapIcon.People;
        public List<IListItem> ItemsList { get; set; } = [];
        public List<IDirectory> SubDirectories => [];// NO subdir
        private async Task LoadData()
        {
            var response = await _service.GetAllAsync<User>($"?licenseNumber={_licenseNumber}", -1);
            List<User> users = [];

            if (response is null)
                Console.WriteLine($"NO RESPONSE");
            else if (response is ErrorResponse response1)
                Console.WriteLine($"{response1.ErrorMessage}");
            else if (response is List<User> list)
                users = list;

            ItemsList.Clear();
            if (users is not null)
                ItemsList = users.Select(u => u as IListItem).ToList();
        }
        public async Task OnSelect()
        {
            if (ItemsList.Count <= 0)
                await LoadData();
        }

        public IListItem GetNew() => new User() { LicenseNumber = _licenseNumber };

        public Task OnExpanded()
        {
            throw new NotImplementedException();
        }

        public void Insert(IListItem item)
        {
            ItemsList.Add(item);
        }

        public void Update(IListItem item)
        {
            var index = ItemsList.FindIndex(i => i.Id == item.Id);
            if (index != -1)
            {
                ItemsList[index] = item;
            }
        }

        public void Delete(IListItem item)
        {
            ItemsList.Remove(item);
        }
    }
}
