using BlazorCore.Components.Dropdown;
using Microsoft.AspNetCore.Components;
using static BlazorCore.Components.DatesShared.DatePickerShared.DatePicker;

namespace BlazorCore.Components.DatesShared.GeneralDateShared
{
    public partial class BaseDateComponent : ComponentBase
    {
        [Parameter, EditorRequired]
        public GeneralDateModel GeneralDateOpt { get; set; } = default!;
        [Parameter, EditorRequired]
        public DatePickerModel StartEndDateModel { get; set; } = default!;
        [Parameter]
        public EventCallback<DatePickerModel> DateChanged { get; set; }
        [Parameter]
        public bool IsStartDate { get; set; }
        protected DateTime? Date { get; set; }
        protected DropdownTrigger DropdownTriggerRef { get; set; } = default!;

        protected override void OnParametersSet()
        {
            if (IsStartDate && Date != StartEndDateModel.StartDate)
            {
                Date = StartEndDateModel.StartDate;
            }
            else if (!IsStartDate && Date != StartEndDateModel.EndDate)
            {
                Date = StartEndDateModel.EndDate;
            }
        }

        public async Task OpenDatepicker()
        {
            await DropdownTriggerRef.OpenDropdown();
        }
    }
}
