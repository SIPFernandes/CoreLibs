using BlazorCore.Data.Consts.ENConsts;
using DotNetCore.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorCore.Components.Upload
{
    public partial class FileUploadItem : ComponentBase
    {
        [Parameter, EditorRequired]
        public FileUpload Upload { set; get; } = default!;
        [Parameter]
        public bool WithControls { set; get; }
        [Parameter]
        public EventCallback<FileUpload> UploadDeleted { get; set; }
        [Parameter]
        public EventCallback<FileUpload> RefreshFile { get; set; }
        [Parameter]
        public string TextProgress { set; get; } = FilesConst.FileUploaded;
        private bool IsDeleting { get; set; } = false;

        private async Task Delete()
        {
            if (Upload.Error is null) 
            {
                IsDeleting = true;
            }
            else
            {
                await UploadDeleted.InvokeAsync(Upload);
            }
        }

        private async Task DeleteUpload()
        {
            IsDeleting = false;

            await UploadDeleted.InvokeAsync(Upload);
        }
        
        private string GetFileError()
        {
            return Upload.Error switch
            {
                FileUpload.ErrorType.FileSizeError => FilesConst.FileSizeError,
                FileUpload.ErrorType.FileSizeLimit => FilesConst.FileSizeLimit,
                FileUpload.ErrorType.FileTypeError => FilesConst.FileTypeRequiredSrc,
                _ => FilesConst.UploadGenericError
            };
        }
    }
}
