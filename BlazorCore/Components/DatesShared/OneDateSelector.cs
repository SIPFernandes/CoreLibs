using BlazorCore.Components.DatesShared.DatePickerShared;
using BlazorCore.Components.DatesShared.GeneralDateShared;
using BlazorCore.Data.Consts.ENConsts;
using Microsoft.AspNetCore.Components;

namespace BlazorCore.Components.DatesShared
{
    public class OneDateSelector : GeneralDateComponent
    {
        [Parameter]
        public DateTime? DateValue { get; set; }
        [Parameter]
        public string ConfirmText { get; set; } = GenericsConst.Confirm;
        [Parameter]
        public int MinYearFromDate { get; set; } = 10;
        [Parameter]
        public int MaxYearFromDate { get; set; } = 10;
        [Parameter]
        public EventCallback<DateTime?> DateValueChanged { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            
            GeneralDateOpt!.DisplayOptions = DisplayOptionsEnum.OnlyStartDate;
        }

        protected override DatePicker.DatePickerModel SetStartEndDateModel()
        {
            return new DatePicker.DatePickerModel(DateValue)
            {
                ConfirmText = ConfirmText,
                MinYearFromDate = MinYearFromDate,
                MaxYearFromDate = MaxYearFromDate
            };
        }

        protected override async Task DateChanged(DatePicker.DatePickerModel dates)
        {
            await DateValueChanged.InvokeAsync(dates.StartDate);
        }
    }
}
