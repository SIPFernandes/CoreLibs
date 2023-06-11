using BlazorCore.Components.Modal;
using DotNetCore.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorCore.Components.Upload;

public partial class UploadModal : UploadBase
{
    [Parameter, EditorRequired]
    public EventCallback<FileUpload> OnUploadFile { get; set; }
    [Parameter, EditorRequired]
    public EventCallback<FileUpload> OnCancelUpload { get; set; }
    private BaseModal? Modal { get; set; }
    private bool IsMinimized;

    public void OpenUploadModal()
    {            
        IsMinimized = false;

        Reset();

        Modal!.Open();
    }

    protected override async Task OnUploadFileTask(FileUpload upload)
    {
        await OnUploadFile.InvokeAsync(upload);
    }

    protected override async Task OnCancelUploadTask(FileUpload upload)
    {
        await OnCancelUpload.InvokeAsync(upload);
    }

    private async Task Confirm()
    {
        Modal!.Close();

        await OnConfirm();
    }

    private async Task CancelUploadsHandle()
    {
        Modal!.Close();
        
        await CancelUploads();
    }
}