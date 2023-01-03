using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace WebClient.Components
{
    public partial class RZInputFile: ComponentBase
    {
        [Parameter]
        public EventCallback<InputFileChangeEventArgs> Event { get; set; } =
            new EventCallback<InputFileChangeEventArgs>();

        private string _fileStatus = "";
        private Guid  FileID  = Guid.NewGuid();

        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }

        private async Task OnChangeFileAtEditModal(InputFileChangeEventArgs e)
        {
            await Event.InvokeAsync(e);
            _fileStatus = e.File.Name;
        }
    }
}