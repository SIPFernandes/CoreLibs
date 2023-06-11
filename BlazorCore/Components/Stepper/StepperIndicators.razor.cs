using BlazorCore.Data.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorCore.Components.Stepper
{
    public partial class StepperIndicators : ComponentBase
    {
        [Parameter, EditorRequired]
        public StepperModel[] Steps { set; get; } = default!;
        [Parameter, EditorRequired]
        public bool[] CanGoToStepArray { set; get; } = default!;
        [Parameter]
        public EventCallback<bool[]> CanGoToStepArrayChanged { set; get; }
        [Parameter]
        public int CurrentStep { set; get; }
        [Parameter]
        public EventCallback<int> CurrentStepChanged { set; get; }
        [Parameter]
        public bool CanContinue { set; get; } = true;
        [Parameter]
        public EventCallback<int> LastStepCallBack { set; get; }
        [Parameter]
        public bool VerticalSteps { set; get; }
        private bool _canContinue;
        private int _currentStep;

        protected override void OnInitialized()
        {
            _currentStep = CurrentStep;

            _canContinue = CanContinue;
        }

        protected override async Task OnParametersSetAsync()
        {
            if (_currentStep != CurrentStep ||
                _canContinue != CanContinue)
            {
                if (_currentStep != CurrentStep)
                {
                    await GoToStep(CurrentStep, false);

                    _currentStep = CurrentStep;
                }

                _canContinue = CanContinue;

                if (_currentStep < Steps.Length)
                {
                    if (Steps[_currentStep].IsCompleted)
                    {
                        await SetCanGoToStepArray(_canContinue);
                    }

                    CanGoToStepArray[_currentStep + 1] = _canContinue;
                }
            }
        }

        private async Task SetCanGoToStepArray(bool canGo)
        {
            for (var i = 0; i < Steps.Length; i++)
            {
                if (i != _currentStep)
                {
                    CanGoToStepArray[i] = canGo && Steps[i].IsCompleted;
                }
            }

            await CanGoToStepArrayChanged.InvokeAsync(CanGoToStepArray);
        }

        /// <summary>
        /// Can only go to the Step if it is complete.
        /// It can only go if the current step is valid or to a previous step if the current one is not complete
        /// </summary>
        /// <param name="step"></param>
        private async Task GoToStep(int step, bool localCall = true)
        {
            if (CanGoToStepArray[step])
            {
                if (step == Steps.Length)
                {
                    await LastStepCallBack.InvokeAsync();
                }
                else
                {
                    if (CanContinue)
                    {
                        Steps[_currentStep].IsCompleted = true;
                    }

                    if (localCall)
                    {
                        await CurrentStepChanged.InvokeAsync(step);
                    }
                }
            }
        }
    }
}
