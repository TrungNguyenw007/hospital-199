using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Contract;
using Contract.Parts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Radzen;
using WebClient.RequestHttp;

namespace WebClient.Service.Parts
{
    public class PartService : IPartService
    {
        private NotificationService _notificationService;
        private NavigationManager _navigationManager;
        public PartService(NotificationService notificationService,
            NavigationManager navigationManager)
        {
            _notificationService = notificationService;
            _navigationManager = navigationManager;
        }

        
        public async Task<PartDto> CreateAsync(CreateUpdatePartDto input)
        {
            return  await  RequestClient.PostAPIAsync<PartDto>("part/",input);
        }

        public async Task<PartDto> UpdateAsync(CreateUpdatePartDto input, Guid id)
        {
            return  await  RequestClient.PutAPIAsync<PartDto>($"part/{id}",input);

        }
        

        public async Task DeleteAsync(Guid id)
        {
              await  RequestClient.DeleteAPIAsync<Task>($"part/{id}");
        }

        public async Task<List<PartDto>> GetListAsync()
        {
            return  await  RequestClient.GetAPIAsync<List<PartDto>>("part");
        }

        public async Task<List<PartDto>> GetListAsync(PartFilter input)
        {
            return  await  RequestClient.PostAPIAsync<List<PartDto>>("part/get-list-by-filter",input,false);
        }
    }
}