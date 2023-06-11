using System.Globalization;
using System.Text.RegularExpressions;
using DotNetCore.Consts;
using Microsoft.AspNetCore.Components;
using static BlazorCore.Components.DatesShared.DatePickerShared.DatePicker;

namespace BlazorCore.Components.DatesShared.DatePickerShared.DatePickerHeader
{
    public partial class DatePickerHeader : ComponentBase
    {

        [Parameter, EditorRequired]
        public DatePickerModel Model { get; set; } = default!;
        [Parameter]
        public EventCallback<DateTime?> OnStartDateChange { get; set; }
        [Parameter]
        public EventCallback<DateTime?> OnEndDateChange { get; set; }
        private string? StartDate { get; set; }
        private string? EndDate { get; set; }
        private bool StartDateInvalid { get; set; } = false;
        private bool EndDateInvalid { get; set; } = false;
        private const int MaxDateLength = 10;

        protected override void OnParametersSet()
        {
            StartDate = Model.StartDate.HasValue
                ? Model.StartDate.Value.ToString(DateConsts.Format)
                : string.Empty;

            EndDate = Model.EndDate.HasValue
                ? Model.EndDate.Value.ToString(DateConsts.Format)
                : string.Empty;
        }

        private async Task OnInputChange(ChangeEventArgs args, InputDateType dateType)
        {
            var date = args.Value!.ToString()!;

            if (Regex.IsMatch(date, @"^[0-9,\/]+$"))
            {
                if (date.Length is 2 or 5)
                {
                    date += "/";
                }
            }

            if (dateType == InputDateType.Start)
            {
                StartDate = date;

                StartDateInvalid = await ValidateDate(StartDate, OnStartDateChange);
            }
            else
            {
                EndDate = date;

                EndDateInvalid = await ValidateDate(EndDate, OnEndDateChange);
            }
        }

        private static async Task<bool> ValidateDate(string dateString, EventCallback<DateTime?> dateSender)
        {
            if (dateString.Length == MaxDateLength)
            {
                if (DateTime.TryParseExact(dateString, DateConsts.Format,
                       CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
                {
                    await dateSender.InvokeAsync(dateTime);
                }
                else
                {
                    return true;
                }
            }
            else if (dateString.Length == 0)
            {
                await dateSender.InvokeAsync(null);
            }

            return false;
        }

        public enum InputDateType
        {
            Start,
            End
        }
    }
}
