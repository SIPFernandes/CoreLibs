using System.Timers;
using BlazorCore.Areas.Interfaces;
using BlazorCore.Data.Consts.ENConsts;
using Microsoft.AspNetCore.Components;
using Timer = System.Timers.Timer;

namespace BlazorCore.Components.Popup;
public partial class PopUpDialog : ComponentBase, IDisposable
{
    [Inject]
    private IDialogService DialogService { get; set; } = default!;
    private List<PopUpModel> PopUpsList { get; set; } = new();

    protected override void OnInitialized()
    {
        DialogService.OnShowPopUp += ShowPopUp;
    }

    private void ShowPopUp(PopUpModel popUpModel, int secTimer)
    {
        if (secTimer > 0)
        {
            popUpModel.SetCountdown(secTimer, (_, _) => OnTimeOut(popUpModel));
        
            popUpModel.StartCountdown();
        }

        PopUpsList.Add(popUpModel);

        StateHasChanged();
    }

    private async void OnTimeOut(PopUpModel popUp)
    {
        await InvokeAsync(() =>
        {
            PopUpsList.Remove(popUp);

            StateHasChanged();

            popUp.OnTimeOut?.InvokeAsync();

            popUp.Dispose();
        });
    }

    private async Task OnConfirmationChange(PopUpModel popUp, bool confirmation)
    {
        PopUpsList.Remove(popUp);

        if (confirmation)
        {
            if (popUp.OnConfirm.HasValue)
            {
                await popUp.OnConfirm.Value.InvokeAsync();
            }
        }
        else if (popUp.OnTimeOut.HasValue)
        {
            await popUp.OnTimeOut.Value.InvokeAsync();
        }

        popUp.Dispose();
    }

    public void Dispose()
    {
        DialogService.OnShowPopUp -= ShowPopUp;
    }

    public class PopUpModel : IDisposable
    {
        public string? Icon { set; get; }
        public string? IconColor { set; get; }
        public string PopUpMessage { get; }
        public EventCallback? OnConfirm { get; }
        public EventCallback? OnTimeOut { get; }
        private int SecTimer { set; get; }
        private Timer? Countdown;

        public PopUpModel(string popUpMessage, EventCallback? onConfirm, EventCallback? onTimeOut, string? icon = null, string? iconColor = "icon-color")
        {
            PopUpMessage = popUpMessage;
            OnConfirm = onConfirm;
            OnTimeOut = onTimeOut;
            Icon = icon;
            IconColor = iconColor;
        }

        public PopUpModel(string popUpMessage, string? icon = null, string? iconColor = "icon-color")
        {
            PopUpMessage = popUpMessage;
            Icon = icon;
            IconColor = iconColor;
        }

        public void StartCountdown()
        {
            if (Countdown != null)
            {
                if (SecTimer <= 0)
                {
                    throw new Exception(ExceptionConst.SetCountdown);
                }

                Countdown.Start();
            }
        }

        public void StopCountdown()
        {
            if (Countdown is {Enabled: true})
            {
                Countdown.Stop();
            }
        }

        public void SetCountdown(int secTime, ElapsedEventHandler onTimeOut)
        {
            if (secTime <= 0)
            {
                throw new Exception(ExceptionConst.InvalidTime);
            }

            if (Countdown == null)
            {
                SecTimer = secTime;

                Countdown = new Timer(SecTimer * 1000);

                Countdown.Elapsed += onTimeOut;

                Countdown.AutoReset = false;
            }
        }

        public void Dispose()
        {
            Countdown?.Dispose();

            Countdown = null;
        }
    }
}