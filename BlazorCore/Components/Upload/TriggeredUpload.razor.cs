using BlazorCore.Areas.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorCore.Components.Upload
{
    public partial class TriggeredUpload : ComponentBase
    {
        [Parameter, EditorRequired] 
        public EventCallback<InputFileChangeEventArgs> OnUploadFile { get; set; }
        [Parameter]
        public string AcceptedFiles { get; set; } = "*"; //TODO define list of accepted files, it should not be all types
        [Parameter]
        public bool MultipleFiles { get; set; } = true;
        [Inject]
        IJSInteropService JSInteropService { get; set; } = default!;
        private InputFile? InputFile { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JSInteropService.TriggerElement(InputFile!.Element);
            }
        }

        private async Task UploadFile(InputFileChangeEventArgs e)
        {
            await OnUploadFile.InvokeAsync(e);
        }
    }
}
