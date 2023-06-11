using BlazorCore.Components.Modal;
using DotNetCore.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorCore.Components.Upload.ImageUpload
{
    public abstract partial class ImageUploadBase : ComponentBase
    {
        [Parameter]
        public string? Image { get; set; }
        [Parameter]
        public RenderFragment? ImageHolder { get; set; }
        [Parameter]
        public RenderFragment? UploadInstructions { get; set; }        
        [Parameter]
        public double MaxFileSizeKb { get; set; } = 500;
        [Parameter]
        public bool PreventDelete { get; set; }
        // [Inject]
        // private IDialogService DialogService { get; set; }
        private bool ErrorFileSize { set; get; }     
        private const string FileSizeUnit = "kb";
        private ModalConfirm? ModalConfirm;

        protected abstract Task TreatFile(IBrowserFile file);
        protected abstract Task RemoveFile();

        private async Task OnInputFileChange(InputFileChangeEventArgs e)
        {
            if (FilesHelper.IsValidSize(e.File, MaxFileSizeKb / 1000))
            {
                ErrorFileSize = false;

                await TreatFile(e.File);                                              
            }
            else
            {
                ErrorFileSize = true;
                
                //todo add dialog service
                
                // DialogService.ShowModal(string.Format(UsersConst.UnableToUploadDescription,
                //         MaxFileSizeKb, FileSizeUnit),
                //     UsersConst.UnableToUpload);
            }
        }        

        private async Task RemoveImageComplete()
        {
            Image = null;
            
            ErrorFileSize = false;

            await RemoveFile();                      
        }

        private async void RemoveImage()
        {
            if(PreventDelete)
            {
                ModalConfirm?.OpenModal();
            }
            else
            {
                await RemoveImageComplete();
            }
        }
    }
}
