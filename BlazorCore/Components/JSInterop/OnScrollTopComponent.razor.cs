using BlazorCore.Areas.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorCore.Components.JSInterop
{
    public partial class OnScrollTopComponent : ComponentBase, IDisposable
    {
        [Parameter, EditorRequired]
        public RenderFragment Content { get; set; } = default!;
        [Parameter, EditorRequired]
        public EventCallback OnScrollTopEvent { get; set; }
        [JSInvokable]
        public async Task OnScrollTop() => await OnScrollTopEvent.InvokeAsync();
        protected virtual ElementReference? ElementReference { get; set; }
        protected DotNetObjectReference<OnScrollTopComponent> Reference;
        [Inject]
        protected IJSInteropService JSInteropService { get; set; } = default!;       

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Reference ??= DotNetObjectReference.Create(this);

                await JSInteropService.ScrollTopListener(Reference, ElementReference);
            }
        }

        //This cause errors, is this really needed?
        public virtual void Dispose()
        {
            Reference?.Dispose();
        }
    }
}
