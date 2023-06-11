using BlazorCore.Data.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorCore.Components.Dropdown
{
    public partial class DropdownTrigger : ComponentBase
    {
        [CascadingParameter]
        public DropdownStack DropdownStackRef { get; set; }
        [Parameter]
        public RenderFragment Trigger { get; set; }
        [Parameter]
        public RenderFragment Content { get; set; }
        [Parameter]
        public EventCallback OnDropdownClose { get; set; }
        [Parameter]
        public int DropdownWidth { get; set; } = 250;
        [Parameter]
        public bool CloseOnSelect { get; set; }
        [Parameter]
        public bool Disabled { get; set; }        
        [Parameter]
        public string TriggerStyle { get; set; }
        private ElementReference TriggerReference { get; set; }
        private DropdownModel DropdownInstance { get; set; }
        private bool DropdownOpened { get; set; }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender && DropdownInstance == null)
            {
                InitializeDropdownInstance();
            }
        }

        public bool IsDropdownOpen()
        {
            return DropdownOpened;
        }

        public async Task OpenDropdown(ElementReference ? elementReference = null)
        {
            if (!Disabled && !DropdownOpened)
            {
                DropdownOpened = true;

                StateHasChanged();

                if(DropdownInstance == null)
                {
                    InitializeDropdownInstance();
                }
                
                if(elementReference.HasValue)
                {
                    DropdownInstance.TriggerRef = elementReference.Value;
                }

                await DropdownStackRef.OpenDropdown(DropdownInstance);
            }
        }

        public async Task CloseDropdown()
        {
            await DropdownStackRef.RemoveDropdown(DropdownInstance);
        }

        private void InitializeDropdownInstance()
        {
            DropdownInstance = new DropdownModel()
            {
                CloseOnSelect = CloseOnSelect,
                DropdownWidth = DropdownWidth,
                OnDropdownClose = new EventCallback(this, DropdownCallback),
                DropdownContent = Content,
                TriggerRef = TriggerReference,
            };
        }

        private async Task DropdownCallback()
        {
            if (DropdownOpened)
            {
                DropdownOpened = false;

                if (OnDropdownClose.HasDelegate)
                {
                    await OnDropdownClose.InvokeAsync();
                }
            }
        }
    }
}
