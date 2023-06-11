using BlazorCore.Areas.Interfaces;
using DotNetCore.Entities.MessageAggregate.CommentAggregate;
using DotNetCore.Interfaces;
using Microsoft.AspNetCore.Components;

namespace BlazorCore.Components.CommentFeed
{
    public partial class CommentFile : ComponentBase
    {
        [Parameter, EditorRequired]
        public CommentFileModel File { get; set; } = default!;
        [Parameter]
        public EventCallback RemoveFile { set; get; }
        [Inject]
        public IFileUploadService FileUploadService { get; set; } = default!;
        [Inject]
        public IJSInteropService JSInteropService { get; set; } = default!;

        private async Task DownloadFile()
        {
            var file = await FileUploadService.GetFile(File.FileId);

            await JSInteropService.ExportAttachment(file);
        }
    }
}
