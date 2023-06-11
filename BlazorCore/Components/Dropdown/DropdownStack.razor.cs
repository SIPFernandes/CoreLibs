using BlazorCore.Areas.Interfaces;
using BlazorCore.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorCore.Components.Dropdown
{
    public partial class DropdownStack : ComponentBase, IDisposable
    {
        [JSInvokable]
        public async Task OnWindowResized() => await SetWindowDimensions();
        [Inject]
        private IJSInteropService JsService { set; get; }        
        private Dictionary<int, DropdownModel> DropdownModelsStack { get; set; }
        private DomRectModel WindowDimensions { get; set; }
        private DotNetObjectReference<DropdownStack> Reference { get; set; }

        protected override void OnInitialized()
        {
            DropdownModelsStack = new Dictionary<int, DropdownModel>();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await SetWindowDimensions();

                Reference = DotNetObjectReference.Create(this);

                await JsService.SetOnWindowResizeListener(Reference, "OnWindowResized");
            }
        }

        public async Task OpenDropdown(DropdownModel dropdownInstance)
        {
            var triggerPosition = await JsService.GetElementDomRect(dropdownInstance.TriggerRef);

            dropdownInstance.Top = (int)triggerPosition.Y + (int)triggerPosition.Height + 5;

            var left = (int)triggerPosition.X;

            dropdownInstance.Left = left + dropdownInstance.DropdownWidth > WindowDimensions.Width 
                ? left - dropdownInstance.DropdownWidth + (int)triggerPosition.Width
                : left;

            DropdownModelsStack.Add(
                dropdownInstance.Id,
                dropdownInstance);

            StateHasChanged();
        }

        public async Task RemoveDropdown(DropdownModel dropdown)
        {
            DropdownModelsStack.Remove(dropdown.Id);

            if (dropdown.OnDropdownClose.HasDelegate)
            {
                await dropdown.OnDropdownClose.InvokeAsync();
            }

            StateHasChanged();
        }

        public void Dispose()
        {
            Reference?.Dispose();
        }

        private async Task SetWindowDimensions()
        {
            WindowDimensions = await JsService.GetWindowDimensions();
        }
    }
}
