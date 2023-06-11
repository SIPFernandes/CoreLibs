using BlazorCore.Areas.Interfaces;
using BlazorCore.Data.Consts;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorCore.Components.Dropdown
{
    public partial class SlideDropdown : ComponentBase
    {
        [Parameter, EditorRequired]
        public RenderFragment Trigger { get; set; } = default!;
        [Parameter]
        public string? ActiveClass { get; set; }
        [Parameter]
        public bool Disabled { get; set; }
        [Parameter]
        public string Arrow { get; set; } = IconsConst.ChevronFilled;
        [Parameter]
        public RenderFragment? Options { get; set; }
        [Parameter]
        public string DropdownClass { get; set; } = "bordered-dropdown";
        [JSInvokable]
        public void OnClickOutside() => CloseDropdown();
        [Inject]
        private IJSInteropService JsService { set; get; } = default!;
        private ElementReference DropdownRef;
        private ElementReference SlideContainerRef { get; set; }
        private DotNetObjectReference<SlideDropdown>? DotNetReference;
        private bool DropdownExpanded;

        public void CloseDropdown()
        {
            DropdownExpanded = false;

            StateHasChanged();
        }

        private async Task ToggleDropdown()
        {
            if(!Disabled)
            {
                DropdownExpanded = !DropdownExpanded;

                if (DropdownExpanded)
                {
                    if(!string.IsNullOrEmpty(ActiveClass))
                    {
                        await JsService.ScrollToElementWithClass(SlideContainerRef, ActiveClass);
                    }
                    
                    await SetOnCloseListener();
                }
            }
        }

        private async Task SetOnCloseListener()
        {
            DotNetReference ??= DotNetObjectReference.Create(this);

            await JsService.OnElementClickOutside(DotNetReference, DropdownRef);
        }
    }
}
