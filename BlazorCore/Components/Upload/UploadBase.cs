using BlazorCore.Components.Popup;
using BlazorCore.Data.Consts.ENConsts;
using DotNetCore.Helpers;
using DotNetCore.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorCore.Components.Upload
{
    public abstract class UploadBase : ComponentBase, IAsyncDisposable
    {
        [Parameter]
        public EventCallback<int> UploadFilesCompleted { get; set; }
        [Parameter]
        public bool AcceptMultipleFiles { get; set; } = true;
        [Parameter]
        public string AcceptedFiles { set; get; } = ".jpg,.png,.jpeg,.gif,.pdf,.docx,.pptx,.xlsx,.zip,.rar";
        protected List<FileUpload> Files { get; } = new();
        protected ConfirmationPopup? ConfirmationPopup { set; get; }
        protected string? UploadDocumentsStatus;
        protected int UploadsWithErrors;
        protected int CompletedUploads;

        public async ValueTask DisposeAsync()
        {
            await CancelUploads();
        }

        protected abstract Task OnUploadFileTask(FileUpload upload);

        protected abstract Task OnCancelUploadTask(FileUpload upload);
        protected virtual Task FileUploadCompleted(FileUpload upload)
        {
            return Task.CompletedTask;
        }

        protected async Task OnFilesDropped(IEnumerable<IBrowserFile> files)
        {            
            var tasks = new List<Task>();

            foreach (var file in files)
            {
                if (AcceptedFiles.Contains($".{FilesHelper.GetFileExtension(file.Name)}"))
                {
                    tasks.Add(PrepareUpload(file));
                }
            }

            if (tasks.Count > 0)
            {
                await Task.WhenAll(tasks);
            }            
        }

        protected async Task OnUploadDelete(FileUpload upload)
        {
            Files.Remove(upload);

            UpdateCounters(upload.Error);

            await CancelUpload(upload);
        }

        protected async Task OnConfirm()
        {
            await UploadFilesCompleted.InvokeAsync(Files.Count);

            Reset();
        }        

        protected async Task RefreshFile(FileUpload upload)
        {
            Files.Remove(upload);

            UpdateCounters(upload.Error);

            Files.Add(upload);
            
            await OnUploadFileTask(upload);
        }

        protected async Task CancelUploads()
        {
            foreach (var upload in Files)
            {
                await CancelUpload(upload);
            }

            Reset();
        }

        protected async Task OnCancel()
        {
            if (CompletedUploads > 0)
            {
                ConfirmationPopup!.ShowConfirmation();
            }
            else
            {
                await CancelUploads();
            }
        }

        protected void Reset()
        {
            Files.Clear();

            UploadsWithErrors = 0;

            CompletedUploads = 0;
        }

        private async Task PrepareUpload(IBrowserFile file)
        {
            var upload = new FileUpload(file);

            Files.Add(upload);
            
            StateHasChanged();

            await OnUploadFileTask(upload);

            if (upload.Error is null)
            {
                upload.UploadCompletion = 100;                
            }            

            UpdateCounters(upload.Error, false);

            await FileUploadCompleted(upload);
        }

        private async Task CancelUpload(FileUpload upload)
        {
            if (upload.Error is null)
            {
                if (upload.UploadCompletion != 100)
                {
                    upload.Dispose();
                }
                else
                {
                    //No need dispose because when saved in DB already has been disposed
                    await OnCancelUploadTask(upload);
                }
            }
        }

        private void UpdateCounters(FileUpload.ErrorType? error, bool wasRemoved = true)
        {
            var increaseCounters = wasRemoved ? -1 : 1;

            if (error is null)
            {
                CompletedUploads += increaseCounters;
            }
            else
            {
                UploadsWithErrors += increaseCounters;
            }

            UploadDocumentsStatus = GetUploadStatus();
        }

        private string GetUploadStatus()
        {
            if (UploadsWithErrors > 0)
            {
                return FilesConst.ErrorOccured;
            }

            if (CompletedUploads < Files.Count)
            {
                return FilesConst.UploadingDocuments;
            }

            return string.Format(FilesConst.DocumentsUploaded,
                Files.Count);
        }
    }
}
