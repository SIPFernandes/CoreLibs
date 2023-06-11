using Microsoft.AspNetCore.Components;

namespace BlazorCore.Components.Stepper
{
    public partial class StepperBackContinue : ComponentBase
    {
        [Parameter]
        public int CurrentStep { get; set; }
        [Parameter]
        public EventCallback<int> CurrentStepChanged { get; set; }

        [Parameter]
        public bool[] CanGoToStepArray { get; set; }

        [Parameter]
        public RenderFragment PreviousBtn { get; set; }

        [Parameter]
        public RenderFragment NextBtn { get; set; }
        [Parameter]
        public bool Waiting { get; set; }
        private int PreviousStep;
        private int NextStep;
        private int _currentStep;

        protected override void OnInitialized()
        {
            SetSteps();
        }

        protected override void OnParametersSet()
        {
            if (_currentStep != CurrentStep)
            {
                _currentStep = CurrentStep;

                SetSteps();
            }
        }

        private void SetSteps()
        {
            PreviousStep = CurrentStep - 1;

            NextStep = CurrentStep + 1;
        }
    }
}