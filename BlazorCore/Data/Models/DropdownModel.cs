using Microsoft.AspNetCore.Components;

namespace BlazorCore.Data.Models;

public class DropdownModel
{
    public int Id { get; set; }
    public RenderFragment DropdownContent { get; set; }
    public ElementReference TriggerRef { get; set; }
    public EventCallback OnDropdownClose { get; set; }
    public bool CloseOnSelect { get; set; }
    public int Top { get; set; }
    public int Left { get; set; }
    public int DropdownWidth { get; set; }

    public DropdownModel()
    {
        Id = this.GetHashCode();
    }
}