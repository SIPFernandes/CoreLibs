using BlazorCore.Areas.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorCore.Components.JSInterop
{
    public class OnScrollTopBottomComponent : ComponentBase, IDisposable
    {
        protected virtual ElementReference? ElementReference { get; set; }
        protected DotNetObjectReference<OnScrollTopBottomComponent> Reference;
        [Inject]
        protected IJSInteropService JSInteropService { get; set; } = default!;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (Reference == null)
                {
                    Reference = DotNetObjectReference.Create(this);
                }

                await JSInteropService.ScrollTopBottomListener(Reference, ElementReference);
            }
        }

        public virtual void Dispose()
        {
            Reference?.Dispose();
        }
    }
}
