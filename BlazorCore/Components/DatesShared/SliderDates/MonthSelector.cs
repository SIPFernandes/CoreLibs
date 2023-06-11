using System.Globalization;

namespace BlazorCore.Components.DatesShared.SliderDates
{
    public partial class MonthSelector : SliderDateSelector
    {
        protected override void SetDates()
        {
            DatesList = new List<DateTime>();

            var currentDate = new DateTime(_dateSelected.Year, 1, _dateSelected.Day);

            for (var i = 0; i < 12; i++)
            {
                DatesList.Add(currentDate);

                currentDate = currentDate.AddMonths(1);
            }

            StateHasChanged();
        }

        protected override string DateDisplay(DateTime date)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(date.Month);
        }

        protected override bool IsDateSelected(DateTime date)
        {
            return date.Month == _dateSelected.Month;
        }

        protected override DateTime GetSelectedDate(DateTime date)
        {
            return new DateTime(_dateSelected.Year, date.Month, _dateSelected.Day);
        }
    }
}
