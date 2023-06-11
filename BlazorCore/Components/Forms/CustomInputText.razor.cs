using BlazorCore.Areas.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorCore.Components.Forms;

public partial class CustomInputText : InputBase<string>
{
    [Inject]
    IJSInteropService JsInteropService { get; set; } = default!;
    [Parameter]
    public string? LabelText { get; set; }
    [Parameter]
    public string? Placeholder { get; set; }
    [Parameter]
    public bool IsRequired { get; set; }
    [Parameter]
    public string Id { get; set; } = default!;
    [Parameter]
    public InputType Type { get; set; } = InputType.Text;
    [Parameter]
    public bool ShowClearIcon { get; set; } = true;
    [Parameter]
    public string? Icon { get; set; }
    [Parameter]
    public EventCallback<string> OnInput { get; set; }
    private ElementReference SearchInputRef;
    
    public async Task Focus()
    {
        await JsInteropService.FocusElement(SearchInputRef);
    }
    
    protected override bool TryParseValueFromString(string? value, out string result, out string? validationErrorMessage)
    {
        result = value ?? string.Empty;
        validationErrorMessage = null;
        return true;
    }
    
    protected override void OnInitialized()
    {
        if (Type == InputType.Password)
        {
            Id = string.IsNullOrEmpty(Id) ? "id"
                + Guid.NewGuid().ToString().Replace("-", string.Empty) : Id;
        }
    }
    
    private async Task HandleInput(ChangeEventArgs e)
    {
        await HandleInput(e.Value?.ToString() ?? string.Empty);
    }
    
    private async Task HandleInput(string value)
    {
        if (OnInput.HasDelegate)
        {
            await OnInput.InvokeAsync(value);
        }
    }

    private async Task Clear()
    {
        await HandleInput(string.Empty);

        CurrentValue = string.Empty;
    }
    
    public enum InputType
    {
        Text,
        Password,
        TextArea
    }
}