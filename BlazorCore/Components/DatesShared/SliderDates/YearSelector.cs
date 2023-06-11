using Microsoft.AspNetCore.Components;

namespace BlazorCore.Components.DatesShared.SliderDates
{
    public partial class YearSelector : SliderDateSelector
    {
        [Parameter]
        public int MinYearFromDate { get; set; } = 10;
        [Parameter]
        public int MaxYearFromDate { get; set; } = 10;
        protected override void SetDates()
        {
            DatesList = new List<DateTime>();
            
            var minYear = DateTime.Now.Year - MinYearFromDate;
            var maxYear = DateTime.Now.Year + MaxYearFromDate;

            var currentDate = new DateTime(maxYear, _dateSelected.Month, _dateSelected.Day);
            
            while (currentDate.Year >= minYear)
            {
                DatesList.Add(currentDate);

                currentDate = currentDate.AddYears(-1);
            }

            StateHasChanged();
        }

        protected override string DateDisplay(DateTime date)
        {
            return date.Year.ToString();
        }

        protected override bool IsDateSelected(DateTime date)
        {
            return date.Year == _dateSelected.Year;
        }
        
        protected override DateTime GetSelectedDate(DateTime date)
        {
            return new DateTime(date.Year, _dateSelected.Month, _dateSelected.Day);
        }
    }
}
