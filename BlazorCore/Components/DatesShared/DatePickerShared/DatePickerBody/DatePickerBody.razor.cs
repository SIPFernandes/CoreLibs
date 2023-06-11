using Microsoft.AspNetCore.Components;
using static BlazorCore.Components.DatesShared.DatePickerShared.DatePicker;

namespace BlazorCore.Components.DatesShared.DatePickerShared.DatePickerBody
{
    public partial class DatePickerBody : ComponentBase
    {
        [Parameter, EditorRequired]
        public DatePickerModel Model { get; set; } = default!;
        [Parameter]
        public EventCallback<DatePickerModel> ModelChanged { get; set; }
        [Parameter]
        public DateTime? MinDate { get; set; }
        [Parameter]
        public DateTime? MaxDate { get; set; }

        private DateTime Date { get; set; }

        protected override void OnInitialized()
        {
            if (Model.EndDate.HasValue)
            {
                Date = new DateTime(Model.EndDate.Value.Year, Model.EndDate.Value.Month, 1);
            }
            else if (Model.StartDate.HasValue)
            {
                Date = new DateTime(Model.StartDate.Value.Year, Model.StartDate.Value.Month, 1);
            }
            else
            {
                var current = DateTime.UtcNow;

                Date = new DateTime(current.Year, current.Month, 1);
            }
        }

        protected override void OnParametersSet()
        {
            if (Model.EndDate.HasValue)
                Date = new DateTime(Model.EndDate.Value.Year, Model.EndDate.Value.Month, 1);
            else if (Model.StartDate.HasValue)
                Date = new DateTime(Model.StartDate.Value.Year, Model.StartDate.Value.Month, 1);
        }

        private void PrevMonth()
        {
            Date = Date.AddMonths(-1);
        }

        private void NextMonth()
        {
            Date = Date.AddMonths(1);
        }

        private void ChangeDate(DateTime date)
        {
            Date = date;
        }
    }
}
