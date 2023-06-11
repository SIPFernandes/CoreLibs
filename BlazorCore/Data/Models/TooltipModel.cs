using Microsoft.AspNetCore.Components;

namespace BlazorCore.Data.Models;

public class TooltipModel
{
    public RenderFragment DropdownContent { get; }
    public ElementReference TriggerRef { get; }
    public int TooltipWidth { get; }
    public int Top { get; set; }
    public int Left { get; set; }
    public Position TooltipPosition { get; set; }
    
    public TooltipModel(ElementReference triggerRef, RenderFragment dropdownContent, int tooltipWidth, Position tooltipPosition)
    {
        TriggerRef = triggerRef;
        DropdownContent = dropdownContent;
        TooltipWidth = tooltipWidth;
        TooltipPosition = tooltipPosition;
    }

    public enum Position
    {
        Horizontal,
        Vertical
    }
}