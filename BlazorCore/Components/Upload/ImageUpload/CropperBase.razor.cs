using Blazor.Cropper;
using BlazorCore.Components.Modal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorCore.Components.Upload.ImageUpload
{
    public partial class CropperBase : ComponentBase
    {
        [Parameter]
        public EventCallback<byte[]> CroppedImage { get; set; }
        private BaseModal? ModalRef { get; set; }
        private Cropper? Cropper { get; set; }
        private IBrowserFile? File;       

        private void OnInputFileChange(InputFileChangeEventArgs args)
        {
            ModalRef!.Open();

            File = args.File;
        }

        private async Task GetCropResult()
        {
            var result = await Cropper!.GetCropedResult();
            
            var bytes = await result.GetDataAsync();

            await CroppedImage.InvokeAsync(bytes);

            ModalRef!.Close();
        }
    }
}
