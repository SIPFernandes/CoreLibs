using Blazor.Cropper;
using BlazorCore.Components.Modal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorCore.Components.Upload.ImageUpload
{
    public partial class CropImageUpload : ImageUploadBase
    {
        [Parameter, EditorRequired]
        public EventCallback<byte[]> CroppedImage { get; set; }
        [Parameter]
        public bool FixedRatio { get; set; }
        private ModalConfirm? ModalRef { get; set; }
        private Cropper? Cropper { get; set; }
        private IBrowserFile? File;

        protected override async Task TreatFile(IBrowserFile file)
        {
            ModalRef!.OpenModal();

            File = await file.RequestImageFileAsync("jpg", 200, 200);
        }

        protected override async Task RemoveFile()
        {
            await CroppedImage.InvokeAsync();
        }

        private async Task GetCropResult()
        {
            var result = await Cropper!.GetCropedResult();

            var bytes = await result.GetDataAsync();

            await CroppedImage.InvokeAsync(bytes);
        }
    }
}
