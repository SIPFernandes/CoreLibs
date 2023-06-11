using BlazorCore.Components.Dropdown;
using BlazorCore.Data.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorCore.Components.Tooltip
{
    public partial class LongHoverTooltip : ComponentBase
    {
        [CascadingParameter]
        public TooltipStack TooltipStackRef { get; set; } = default!;
        [Parameter, EditorRequired]
        public RenderFragment Trigger { get; set; } = default!;
        [Parameter, EditorRequired]
        public RenderFragment TooltipContent { get; set; } = default!;
        [Parameter]
        public int TooltipWidth { get; set; } = 250;
        [Parameter]
        public TooltipModel.Position TooltipPosition { get; set; } = TooltipModel.Position.Horizontal;
        private bool TooltipVisible { get; set; }
        private ElementReference TriggerReference { get; set; }
        private TooltipModel? TooltipInstance { get; set; }
        private bool MouseIn { get; set; }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                InitializeTooltipInstance();
            }
        }

        private async Task OnMouseEnter()
        {
            MouseIn = true;

            await Task.Delay(1000).ContinueWith(_ => ShowTooltip());
        }

        private async Task OnMouseLeave()
        {
            MouseIn = false;

            if (TooltipVisible)
            {
                await TooltipStackRef.CloseIfMouseNotInside();

                TooltipVisible = false;
            }
        }

        private void InitializeTooltipInstance()
        {
            TooltipInstance = new TooltipModel(TriggerReference, TooltipContent, TooltipWidth, TooltipPosition);
        }

        private async Task ShowTooltip()
        {
            if (MouseIn)
            {
                TooltipVisible = true;

                if (TooltipInstance == null)
                {
                    InitializeTooltipInstance();
                }

                await TooltipStackRef.OpenTooltip(TooltipInstance!);
            }   
        }
    }
}
