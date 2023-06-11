using BlazorCore.Data.Consts.ENConsts;
using Microsoft.AspNetCore.Components;

namespace BlazorCore.Components.DatesShared.DatePickerShared
{
    public partial class DatePicker : ComponentBase
    {
        [Parameter]
        public DatePickerModel? Model { get; set; }
        [Parameter]
        public EventCallback<DatePickerModel> ModelChanged { get; set; }
        [Parameter]
        public DateTime? MinDate { get; set; }
        [Parameter]
        public DateTime? MaxDate { get; set; }

        private DatePickerModel _model = default!;
        public class DatePickerModel
        {
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public bool OnlyPickOneDate { get; }
            public string ConfirmText { get; init; } = GenericsConst.Confirm;
            public int MinYearFromDate { get; init; } = 10;
            public int MaxYearFromDate { get; init; } = 10;

            public DatePickerModel(DateTime? startDate, DateTime? endDate, bool onlyPickOneDate = false)
            {
                StartDate = startDate;
                EndDate = endDate;
                OnlyPickOneDate = onlyPickOneDate;
            }

            public DatePickerModel(DateTime? date)
            {
                StartDate = date;
                EndDate = date;
                OnlyPickOneDate = true;
            }

            public DatePickerModel Clone()
            {
                return (DatePickerModel)MemberwiseClone();
            }
        }

        protected override void OnInitialized()
        {
            Model ??= new DatePickerModel(null, null);

            _model = Model.Clone();
        }

        protected override void OnParametersSet()
        {
            if (_model.StartDate != Model!.StartDate || _model.EndDate != Model.EndDate)
            {
                _model.StartDate = Model.StartDate;

                _model.EndDate = Model.EndDate;
            }
        }

        private void OnStartDateChanged(DateTime? date)
        {
            _model.StartDate = date;

            if (_model.StartDate > _model.EndDate)
            {
                _model.EndDate = null;
            }
        }

        private void OnEndDateChanged(DateTime? date)
        {
            _model.EndDate = date;

            if (_model.StartDate > _model.EndDate)
            {
                _model.StartDate = null;

                _model.EndDate = null;
            }
        }

        private async Task ConfirmDates()
        {
            Model!.StartDate = _model.StartDate;

            Model.EndDate = _model.EndDate;
            
            await ModelChanged.InvokeAsync(_model);
        }
    }
}
