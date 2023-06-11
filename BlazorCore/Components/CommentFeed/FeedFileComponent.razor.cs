using BlazorCore.Components.Upload;
using DotNetCore.Interfaces;
using DotNetCore.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorCore.Components.CommentFeed
{
    public partial class FeedFileComponent : UploadBase
    {
        [Parameter, EditorRequired]
        public EventCallback<FileUpload> OnUploadFile { get; set; }
        [Parameter, EditorRequired]
        public string CurrentUserId { set; get; } = default!;
        [Parameter, EditorRequired]
        public EventCallback OnFileUploadCompleted { get; set; } = default!;
        [Parameter]
        public object? UploadFileData { set; get; }
        [Inject]
        IFileUploadService FileUploadService { get; set; } = default!;

        public int GetCompletedUploads()
        {
            return CompletedUploads;
        }
        
        public new async Task OnConfirm()
        {
            await base.OnConfirm();
        }
        
        public new async Task RefreshFile(FileUpload file)
        {
            await base.RefreshFile(file);
        }

        public new async Task OnUploadDelete(FileUpload file)
        {
            await base.OnUploadDelete(file);
        }

        protected override async Task OnUploadFileTask(FileUpload upload)
        {
            await OnUploadFile.InvokeAsync(upload);

            await FileUploadService.Upload(upload, CurrentUserId, UploadFileData);
        }

        protected override async Task OnCancelUploadTask(FileUpload upload)
        {
            await FileUploadService.DeleteUploadedFile(upload);
        }

        protected override async Task FileUploadCompleted(FileUpload upload)
        {
            await OnFileUploadCompleted.InvokeAsync();
        }

        private bool Show;

        private async Task OnFilesDropped(InputFileChangeEventArgs e)
        {
            var files = e.GetMultipleFiles();

            await OnFilesDropped(files);

            Show = false;
        }
    }
}
