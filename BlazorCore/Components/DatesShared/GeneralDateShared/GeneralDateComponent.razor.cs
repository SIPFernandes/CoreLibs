using Microsoft.AspNetCore.Components;
using static BlazorCore.Components.DatesShared.DatePickerShared.DatePicker;

namespace BlazorCore.Components.DatesShared.GeneralDateShared
{
    public abstract partial class GeneralDateComponent : ComponentBase
    {
        [Parameter]
        public GeneralDateModel? GeneralDateOpt { get; set; }
        [Parameter]
        public RenderFragment? StartDateLabel { get; set; }
        [Parameter]
        public RenderFragment? EndDateLabel { get; set; }
        [Parameter]
        public RenderFragment? DateSeparator { get; set; }
        protected bool HasPermissionToChangeDates { get; set; } = true;
        private DatePickerModel StartEndDateModel { get; set; } = default!;
        private BaseDateComponent StartBaseDateComponentRef { get; set; } = default!;
        private BaseDateComponent EndBaseDateComponentRef { get; set; } = default!;

        public async Task OpenDatepicker()
        {
            if (GeneralDateOpt!.DisplayOptions == DisplayOptionsEnum.BothDates
                || GeneralDateOpt.DisplayOptions == DisplayOptionsEnum.OnlyStartDate)
            {
                await StartBaseDateComponentRef.OpenDatepicker();
            }
            else if(GeneralDateOpt.DisplayOptions == DisplayOptionsEnum.OnlyEndDate)
            {
                await EndBaseDateComponentRef.OpenDatepicker();
            }
        }

        protected override void OnInitialized()
        {
            GeneralDateOpt ??= new GeneralDateModel();

            StartEndDateModel = SetStartEndDateModel();
        }

        protected abstract DatePickerModel SetStartEndDateModel();
        protected abstract Task DateChanged(DatePickerModel dates);

        private async Task DateChangedCallBack(DatePickerModel dates)
        {
            if (HasPermissionToChangeDates)
            {
                await DateChanged(dates);
            }
        }
    }

    public class GeneralDateModel
    {
        public DisplayOptionsEnum DisplayOptions { get; set; }
            = DisplayOptionsEnum.BothDates;
    }

    public enum DisplayOptionsEnum
    {
        OnlyStartDate,
        OnlyEndDate,
        BothDates
    }
}
