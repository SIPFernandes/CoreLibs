using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorCore.Components.Upload.ImageUpload
{
    public partial class ImageUpload : ImageUploadBase
    {
        [Parameter]
        public EventCallback<IBrowserFile> ImageChanged { get; set; }

        protected override async Task TreatFile(IBrowserFile file)
        {
            await ImageChanged.InvokeAsync(file);
        }

        protected override async Task RemoveFile()
        {
            await ImageChanged.InvokeAsync();
        }
    }
}
