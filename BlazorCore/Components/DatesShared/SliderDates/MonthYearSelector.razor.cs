using System.Globalization;
using Microsoft.AspNetCore.Components;

namespace BlazorCore.Components.DatesShared.SliderDates
{
    public partial class MonthYearComponent : SliderDateSelector
    {
        [Parameter]
        public DateTime? EarliestDate{ get; set; }
        protected override void SetDates()
        {
            DatesList = new List<DateTime> { _dateSelected };

            var currentDate = _dateSelected.AddMonths(-1);
            
            var earliestDate = EarliestDate ?? new DateTime(_dateSelected.Year, 1, 1);

            while (currentDate >= earliestDate)
            {
                DatesList.Add(currentDate);

                currentDate = currentDate.AddMonths(-1);
            }
        }

        protected override string DateDisplay(DateTime date)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(date.Month)
                   + date.Year;
        }

        protected override bool IsDateSelected(DateTime date)
        {
            return date.Month == _dateSelected.Month && date.Year == _dateSelected.Year;
        }

        protected override DateTime GetSelectedDate(DateTime date)
        {
            return new DateTime(date.Year, date.Month, _dateSelected.Day);
        }
    }
}
