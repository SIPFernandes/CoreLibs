using Microsoft.AspNetCore.Components;

namespace BlazorCore.Components.Dropdown;

public partial class DropdownButton : ComponentBase
{
    [Parameter, EditorRequired]
    public string Text { get; set; } = default!;
    [Parameter, EditorRequired]
    public string Icon { get; set; } = default!;
    [Parameter]
    public string? CustomClass { get; set; }
    [Parameter, EditorRequired]
    public EventCallback OnClick { get; set; }    
}