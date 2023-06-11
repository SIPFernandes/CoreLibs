using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorCore.Components.Upload
{
    public partial class Dropzone : ComponentBase
    {
        [Parameter]
        public bool MultipleFiles { set; get; }
        [Parameter]
        public string AcceptedFiles { set; get; } = ".jpg,.png,.jpeg,.gif,.pdf,.docx,.pptx,.xlsx,.zip,.rar";
        [Parameter, EditorRequired]
        public EventCallback<IEnumerable<IBrowserFile>> OnFilesDropped { get; set; }
        [Parameter]
        public RenderFragment? DropzoneContent { get; set; }
        private string Class = string.Empty;

        private void HandleDragEnter()
        {
            Class = "dropzone-drag";
        }

        private void HandleDragLeave()
        {
            Class = string.Empty;
        }

        private async Task OnInputFileChange(InputFileChangeEventArgs e)
        {
            Class = string.Empty;

            var files = e.GetMultipleFiles();

            await OnFilesDropped.InvokeAsync(files);
        }
    }
}
