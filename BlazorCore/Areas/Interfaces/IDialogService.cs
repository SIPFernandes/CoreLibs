using BlazorCore.Components.Popup;
using Microsoft.AspNetCore.Components;

namespace BlazorCore.Areas.Interfaces;

public interface IDialogService
{
    public event Action<string, string?, EventCallback?, string?> OnShowModal;
    public event Action<PopUpDialog.PopUpModel, int> OnShowPopUp;
    public void ShowModal(string message, string? title = null, EventCallback? onConfirm = null,
        string? confirmText = null);
    public void ShowPopUp(PopUpDialog.PopUpModel popUpModel, int secTimer = 5);
}