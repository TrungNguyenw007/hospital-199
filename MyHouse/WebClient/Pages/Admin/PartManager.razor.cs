using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Contract.Parts;
using Contract.Uploads;
using Core.Enum;
using Domain.Parts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Radzen;
using WebClient.Components;
using WebClient.Service.Upload;

namespace WebClient.Pages.Admin
{
    public partial class PartManager
    {
        [Inject] IMessageService _messageService { get; set; }
        private List<PartDto> Parts { get; set; }
        private Modal CreateModal = new Modal();
        private Modal EditModal = new Modal();
        private CreateUpdatePartDto NewPart { get; set; }

        private CreateUpdatePartDto EditPart { get; set; }
        private Guid EditPartId { get; set; }

        private long maxFileSize = 1024 * 15;
        
        private IBrowserFile? NewFile { get; set; }
        private IBrowserFile? EditFile { get; set; }

        private string HeaderTitle { get; set; } = "Part";
        
        public PartManager()
        {
            Parts = new List<PartDto>();
            NewPart = new CreateUpdatePartDto();
            EditPart = new CreateUpdatePartDto();
        }


        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await GetList();
                StateHasChanged();
            }
        }

        private async Task GetList()
        {
            Parts = await _partService.GetListAsync();
            StateHasChanged();
        }

        private void ShowNewModal()
        {
            NewPart = new CreateUpdatePartDto();
            NewFile = null;
            CreateModal.Show();
        }

        private void HideNewModal()
        {
            CreateModal.Hide();
        }

        private async void CreatePart()
        {
            await InvokeAsync(async () =>
                {
                    string path = "";
                    var fileDto = new FileDto();
                    if (NewFile != null)
                    { 
                        fileDto =  await _uploadService.UploadImage(NewFile);
                        NewPart.ImageUrl = fileDto.Url;
                    }
                    NewPart.FileName = fileDto.FileName;
                    NewPart.Path = fileDto.Path;

                    await _partService.CreateAsync(NewPart);
                    await GetList();
                    HideNewModal();
                }, ActionType.Create,true);
        }

        private async Task ShowConfirmMessage(Guid id)
        {
            if ( await _messageService.Confirm( "Are you sure you want to confirm?", "Confirmation" ) )
            {
                await InvokeAsync(async () =>
                {
                    await DeletePart(id);
                },ActionType.Delete,true);
            }
        }
        private async Task DeletePart(Guid id)
        {
    
                await _partService.DeleteAsync(id);
                await GetList();
        }

        async Task OnChangeFileAtNewModal(InputFileChangeEventArgs e)
        {
            
            NewFile = e.GetMultipleFiles().FirstOrDefault();
        }
        
        async Task OnChangeFileAtEditModal(InputFileChangeEventArgs e)
        {
            EditFile = e.GetMultipleFiles().FirstOrDefault();
        }

        void OnChange(int value, string name)
        {
            Console.Write($"{name} value changed to {value}");
        }

        private async Task UpdatePart()
        {
           await  InvokeAsync(async () =>
            {
                var fileDto = new FileDto();
                if (EditFile != null)
                {
                    fileDto = await _uploadService.UploadImage(EditFile);
                    EditPart.ImageUrl = fileDto.Url;
                }

                EditPart.FileName = fileDto.FileName;
                EditPart.Path = fileDto.Path;
                await _partService.UpdateAsync(EditPart, EditPartId);
                await GetList();
                HideEditModal();
            },ActionType.Update,true);
        }

        private Task ShowEditModal(PartDto partDto)
        {
            EditPart = new CreateUpdatePartDto();
            EditPart = ObjectMapper.Map<PartDto, CreateUpdatePartDto>(partDto);
            EditFile = null;
            EditPartId = partDto.Id;
            return EditModal.Show();
        }

        private void HideEditModal()
        {
            EditModal.Hide();
        }
    }
}