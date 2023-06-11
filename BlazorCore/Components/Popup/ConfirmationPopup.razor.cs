using Microsoft.AspNetCore.Components;

namespace BlazorCore.Components.Popup
{
    public partial class ConfirmationPopup : ComponentBase
    {
        [Parameter, EditorRequired]
        public RenderFragment Content { get; set; } = default!;
        [Parameter, EditorRequired]
        public EventCallback OnConfirmation { get; set; }
        [Parameter]
        public EventCallback OnCancel { get; set; }
        [Parameter]
        public bool IsVisible { set; get; }
        [Parameter]
        public string Style { get; set; } = string.Empty;


        public void ShowConfirmation()
        {
            IsVisible = true;

            StateHasChanged();
        }

        private async Task OnConfirm(bool confirmation)
        {
            IsVisible = false;

            if (confirmation)
            {
                await OnConfirmation.InvokeAsync();
            }
            else
            {
                await OnCancel.InvokeAsync();
            }
        }
    }
}
