using BlazorCore.Areas.Interfaces;
using BlazorCore.Areas.Services;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorCore.Components.Dropdown;

public partial class DropdownTriggerPersistent : ComponentBase
{
    [Parameter, EditorRequired]
    public RenderFragment Trigger { get; set; } = default!;
    [Parameter, EditorRequired]
    public RenderFragment Content { get; set; } = default!;
    [Parameter]
    public EventCallback OnDropdownClose { get; set; }
    [Inject]
    IJSInteropService JsInteropService { get; set; } = default!;
    [JSInvokable]
    public async Task OnClickOutside() => await ToggleDropdown(false);
    private ElementReference DropdownRef { get; set; }
    private bool Show { get; set; }
    private DotNetObjectReference<DropdownTriggerPersistent>? DotNetReference { get; set; }

    private async Task ToggleDropdown(bool open)
    {
        Show = open;

        if(Show)
        {
            DotNetReference ??= DotNetObjectReference.Create(this);
            
            await JsInteropService.OnElementClickOutside(DotNetReference, DropdownRef);
        }
        else
        {
            await OnDropdownClose.InvokeAsync();
        }
    }
}