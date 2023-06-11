using BlazorCore.Areas.Interfaces;
using BlazorCore.Components.Popup;
using Microsoft.AspNetCore.Components;

namespace BlazorCore.Areas.Services;

public class DialogService : IDialogService
{
    public event Action<string, string?, EventCallback?, string?>? OnShowModal;
    public event Action<PopUpDialog.PopUpModel, int>? OnShowPopUp;

    public void ShowModal(string message, string? title = null, EventCallback? onConfirm = null,
        string? confirmText = null)
    {
        OnShowModal?.Invoke(message, title, onConfirm, confirmText);
    }

    public void ShowPopUp(PopUpDialog.PopUpModel popUpModel, int secTimer = 5)
    {
        OnShowPopUp?.Invoke(popUpModel, secTimer);
    }
}