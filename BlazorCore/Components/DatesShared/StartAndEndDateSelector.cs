using BlazorCore.Components.DatesShared.DatePickerShared;
using BlazorCore.Components.DatesShared.GeneralDateShared;
using Microsoft.AspNetCore.Components;

namespace BlazorCore.Components.DatesShared
{
    public class StartAndEndDateSelector : GeneralDateComponent
    {
        [Parameter]
        public DateTime? StartDate { get; set; }
        [Parameter]
        public DateTime? EndDate { get; set; }
        [Parameter]
        public EventCallback<DatePicker.DatePickerModel> DateValueChanged { get; set; }

        protected override DatePicker.DatePickerModel SetStartEndDateModel()
        {
            return new DatePicker.DatePickerModel(StartDate, EndDate);
        }

        protected override async Task DateChanged(DatePicker.DatePickerModel dates)
        {
            await DateValueChanged.InvokeAsync(dates);
        }
    }
}
