using DotNetCore.Helpers;
using Microsoft.AspNetCore.Components;
using static BlazorCore.Components.DatesShared.DatePickerShared.DatePicker;

namespace BlazorCore.Components.DatesShared.DatePickerShared.DatePickerBody
{
    public partial class DatePickerMonthDays : ComponentBase
    {
        [Parameter, EditorRequired]
        public DateTime Date { get; set; }
        [Parameter, EditorRequired]
        public DatePickerModel Model { get; set; } = default!;
        [Parameter]
        public EventCallback<DatePickerModel> ModelChanged { get; set; }
        [Parameter]
        public DateTime? MinDate { get; set; }
        [Parameter]
        public DateTime? MaxDate { get; set; }

        private DateTime _calendarFirstDay;
        private DateTime _tmpDate;
        private int _days;
        private int _daysThisMonth;
        private DateTime _date;

        protected override void OnParametersSet()
        {
            if (Date != _date)
            {
                _date = Date;

                _daysThisMonth = DateTime.DaysInMonth(_date.Year, _date.Month);

                var lastMonthDayOfWeek = new DateTime(_date.Year, _date.Month, _daysThisMonth)
                    .GetDayOfWeek();

                var firstDayOfWeek = _date.GetDayOfWeek();
                
                _days = firstDayOfWeek + _daysThisMonth + ((int)DayOfWeek.Saturday - lastMonthDayOfWeek);

                _calendarFirstDay = _date.AddDays(-firstDayOfWeek);

                _tmpDate = _calendarFirstDay;
            }
        }

        private async Task OnClick(DateTime date)
        {
            if (Model.StartDate.HasValue && date.Date == Model.StartDate.Value.Date && Model.EndDate.HasValue) //Cleans Calendar
            {
                Model.StartDate = null;

                Model.EndDate = null;
            }
            else if (Model.EndDate.HasValue && date.Date == Model.EndDate.Value.Date) //Cleans End Date
            {
                Model.EndDate = null;
            }
            else
            {
                if ((Model.StartDate.HasValue && Model.EndDate.HasValue)
                    || (Model.StartDate.HasValue && date.Date < Model.StartDate.Value.Date)) //Selects Start Date again
                {
                    Model.StartDate = null;

                    Model.EndDate = null;
                }

                if (Model.OnlyPickOneDate)
                {
                    Model.StartDate = date;
                    
                    Model.EndDate = date;
                }
                else
                {
                    if (!Model.StartDate.HasValue)
                    {
                        Model.StartDate = date;
                    }
                    else
                    {
                        Model.EndDate = date;
                    }
                }
            }

            await ModelChanged.InvokeAsync(Model);
        }
    }
}
