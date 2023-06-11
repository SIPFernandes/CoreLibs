using BlazorCore.Areas.Interfaces;
using BlazorCore.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorCore.Components.Dropdown
{
    public partial class DropdownComponent : ComponentBase
    {
        [Parameter]
        public DropdownModel DropdownInstanceModel { get; set; }
        [Parameter]
        public EventCallback<DropdownModel> DropdownClosed { set; get; }
        [JSInvokable]
        public async Task OnClickOutside() => await CloseDropdown();
        [Inject]
        private IJSInteropService JsService { set; get; }        
        private ElementReference DropdownRef { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var dotNetObjectReference = DotNetObjectReference.Create(this);

                await JsService.SetOnCloseListener(DropdownRef, dotNetObjectReference);
            }
        }

        public async Task CloseDropdown()
        {
            await DropdownClosed.InvokeAsync(DropdownInstanceModel);
        }
    }
}
