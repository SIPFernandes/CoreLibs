using BlazorCore.Areas.Interfaces;
using Microsoft.AspNetCore.Components;

namespace BlazorCore.Components.JSInterop
{
    public partial class CopyToClipboardComponent : ComponentBase
    {
        [Parameter, EditorRequired]
        public string Link { get; set; } = default!;
        [Inject]
        IJSInteropService JSService { get; set; } = default!;
        private string CopyLink { get; set; } = "Copy sharing link";

        private async Task CopyToClipboard()
        {
            await JSService.CopyToClipBoard(Link);

            CopyLink = "Link copied";

            StateHasChanged();

            await Task.Delay(2000);

            CopyLink = "Copy sharing link";

            StateHasChanged();
        }
    }
}
