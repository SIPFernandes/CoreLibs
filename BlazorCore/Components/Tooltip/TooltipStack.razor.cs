using BlazorCore.Areas.Interfaces;
using BlazorCore.Data.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorCore.Components.Tooltip
{
    public partial class TooltipStack : ComponentBase
    {
        [Inject]
        private IJSInteropService JsService { set; get; } = default!;
        private ElementReference ContentRef { get; set; }
        private TooltipModel? OpenedTooltip { get; set; }
        private bool MouseIn { get; set; }

        public async Task OpenTooltip(TooltipModel tooltipInstance)
        {
            var triggerPosition = await JsService.GetElementDomRect(tooltipInstance.TriggerRef);

            if(tooltipInstance.TooltipPosition == TooltipModel.Position.Horizontal)
            {
                tooltipInstance.Top = (int) triggerPosition.Y - 6;

                var left = (int) triggerPosition.X;

                tooltipInstance.Left = left - tooltipInstance.TooltipWidth < 0
                    ? left + (int) triggerPosition.Width + 12
                    : left - tooltipInstance.TooltipWidth - 12;
            }
            else
            {
                tooltipInstance.Top = -1;
                
                tooltipInstance.Left = (int) triggerPosition.X;
            }

            OpenedTooltip = tooltipInstance;

            await InvokeAsync(StateHasChanged);

            if(OpenedTooltip.TooltipPosition == TooltipModel.Position.Vertical)
            {
                var contentPosition = await JsService.GetElementDomRect(ContentRef);
                
                var top = (int) triggerPosition.Y;
                
                OpenedTooltip.Top = top - contentPosition.Height - 12 < 0
                    ? top + (int) triggerPosition.Height + 12
                    : top - (int) contentPosition.Height - 12;

                await InvokeAsync(StateHasChanged);
            }
        }

        public async Task CloseIfMouseNotInside()
        {
            await Task.Delay(300).ContinueWith(_ => CloseTooltip());
        }

        private async Task CloseTooltip()
        {
            if (!MouseIn)
            {
                OpenedTooltip = null;
            
                await InvokeAsync(StateHasChanged);
            }
        }

        private async Task OnMouseLeave()
        {
            MouseIn = false;

            await CloseTooltip();
        }
    }
}
