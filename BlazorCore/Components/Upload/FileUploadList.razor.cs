using DotNetCore.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorCore.Components.Upload
{
    public partial class FileUploadList : ComponentBase
    {
        [Parameter, EditorRequired]
        public List<FileUpload> Files { get; set; } = default!;
        [Parameter, EditorRequired]
        public EventCallback<FileUpload> OnUploadDelete { get; set; }
        [Parameter, EditorRequired]
        public EventCallback<FileUpload> RefreshFile { get; set; }
        [Parameter]
        public int CompletedUploads { get; set; }
    }
}
