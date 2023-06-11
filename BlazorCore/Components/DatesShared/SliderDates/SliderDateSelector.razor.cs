using BlazorCore.Components.Dropdown;
using Microsoft.AspNetCore.Components;

namespace BlazorCore.Components.DatesShared.SliderDates
{
    public abstract partial class SliderDateSelector : ComponentBase
    {
        [Parameter]
        public DateTime? DateSelected { get; set; }
        [Parameter]
        public EventCallback<DateTime> DateSelectedChanged { get; set; }
        private SlideDropdown SlideDropdownRef = default!;
        protected DateTime _dateSelected;
        protected List<DateTime>? DatesList;

        protected override void OnInitialized()
        {
            _dateSelected = DateSelected ?? DateTime.UtcNow;

            SetDates();
        }

        protected override void OnParametersSet()
        {
            if(DateSelected.HasValue && DateSelected.Value != _dateSelected)
            {
                _dateSelected = DateSelected.Value;
            }
        }

        protected abstract void SetDates();
        protected abstract string DateDisplay(DateTime date);
        protected abstract bool IsDateSelected(DateTime date);

        protected virtual DateTime GetSelectedDate(DateTime date)
        {
            return date;
        }
        
        private async Task SelectDate(DateTime date)
        {
            if (_dateSelected != date)
            {
                _dateSelected = GetSelectedDate(date);

                await DateSelectedChanged.InvokeAsync(_dateSelected);

                SlideDropdownRef.CloseDropdown();
            }
        }
    }
}
