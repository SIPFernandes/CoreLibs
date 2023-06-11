using DotNetCore.Consts;

namespace DotNetCore.Helpers
{
    public static class DateHelper
    {
        public static string DateAgo(this DateTime referenceDate)
        {
            var timeElapsed = DateTime.UtcNow - referenceDate;

            if(timeElapsed.Days == 0)
            {
                var minutesElapsed = (int)timeElapsed.TotalMinutes;
                return minutesElapsed switch
                {
                    0 => DateConsts.Now,
                    < 60 => string.Format(DateConsts.MinutesFormat, minutesElapsed),
                    _ => string.Format(DateConsts.HoursFormat, (int)timeElapsed.TotalHours),
                };
            }
            
            if(timeElapsed.Days < 7)
            {
                return timeElapsed.Days switch
                {
                    1 => DateConsts.DayFormat,
                    _ => string.Format(DateConsts.DaysFormat, timeElapsed.Days),
                };
            }
            
            if (timeElapsed.Days < 30)
            {
                var weeksElapsed = timeElapsed.Days / 7 + 1;
                return weeksElapsed switch
                {
                    1 => DateConsts.WeekFormat,
                    _ => string.Format(DateConsts.WeeksFormat, weeksElapsed),
                };
            }

            if (timeElapsed.Days < 365)
            {
                var monthsElapsed = timeElapsed.Days / 30 + 1;
                return monthsElapsed switch
                {
                    1 => DateConsts.MonthFormat,
                    _ => string.Format(DateConsts.MonthsFormat, monthsElapsed),
                };
            }

            return referenceDate.ToString(DateConsts.Format);
        }
        
        public static string DatePipe(DateTime? nullableDate, string placeholder = "---", DateTime? nullableDateNow = null)
        {
            nullableDateNow = nullableDateNow?.Date ?? DateTime.UtcNow.Date;

            var dateNow = nullableDateNow.Value;
            
            if (!nullableDate.HasValue)
            {
                return placeholder;
            }

            var date = nullableDate.Value;

            if (date == dateNow)
            {
                return DateConsts.Today;
            }

            if (date >= dateNow && date <= dateNow.AddDays(1))
            {
                return DateConsts.Tomorrow;
            }

            if (date >= dateNow && date <= dateNow.AddDays(2))
            {
                return DateConsts.InTwoDays;
            }

            if (date >= dateNow && date <= dateNow.AddDays(7))
            {
                return date.DayOfWeek.ToString();
            }

            return date.ToString(DateConsts.Format);
        }
        
        public static int GetDayOfWeek(this DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Sunday ? 7 : (int) date.DayOfWeek;
        }
    }
}
