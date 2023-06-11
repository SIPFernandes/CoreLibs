using BlazorCore.Areas.Interfaces;
using BlazorCore.Data.Consts.ENConsts;
using Microsoft.AspNetCore.Components;

namespace BlazorCore.Components.Modal;

public partial class ConfirmModalDialog : ComponentBase, IDisposable
{
    [Inject]
    private IDialogService DialogService { get; set; } = default!;
    private string? ConfirmationTitle { get; set; }
    private string? ConfirmationMessage { get; set; }
    private EventCallback? OnConfirm { get; set; }
    private string ConfirmText { get; set; } = GenericsConst.Confirm;
    private ModalConfirm? _modalConfirm;

    public void Dispose()
    {
        DialogService.OnShowModal -= ShowModal;
    }

    protected override void OnInitialized()
    {
        DialogService.OnShowModal += ShowModal;
    }

    private void ShowModal(string message, string? title, EventCallback? onConfirm, string? confirmText = "")
    {
        ConfirmationTitle = title;

        ConfirmationMessage = message;

        OnConfirm = onConfirm;
        
        _modalConfirm!.OpenModal();

        if (!string.IsNullOrEmpty(confirmText))
        {
            ConfirmText = confirmText;
        }

        StateHasChanged();
    }

    private async Task OnConfirmationChange(bool value)
    {
        _modalConfirm!.CloseModal();

        if (value && OnConfirm.HasValue)
        {
            await OnConfirm.Value.InvokeAsync();
        }
    }
    
}