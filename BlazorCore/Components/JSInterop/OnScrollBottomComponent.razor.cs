using BlazorCore.Areas.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorCore.Components.JSInterop
{
    public partial class OnScrollBottomComponent : ComponentBase, IDisposable
    {
        [Parameter, EditorRequired]
        public RenderFragment Content { get; set; } = default!;
        [Parameter, EditorRequired]
        public EventCallback OnScrollBottomEvent { get; set; }
        [JSInvokable]
        public async Task OnScrollBottom() => await OnScrollBottomEvent.InvokeAsync();
        protected virtual ElementReference? ElementReference { get; set; }
        protected DotNetObjectReference<OnScrollBottomComponent> Reference;
        [Inject]
        protected IJSInteropService JSInteropService { get; set; } = default!;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Reference ??= DotNetObjectReference.Create(this);

                await JSInteropService.ScrollBottomListener(Reference, ElementReference);
            }
        }

        //This cause errors, is this really needed?
        public void Dispose()
        {
            Reference?.Dispose();
        }
    }
}
