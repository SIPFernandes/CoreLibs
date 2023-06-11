namespace BlazorCore.Data.Consts.ENConsts
{
    public class GenericsConst
    {
        public const string Apply = "Apply";
        public const string Confirm = "Confirm";
        public const string Cancel = "Cancel";
        public const string Save = "Save";
        public const string Review = "Review";
        public const string TryAgain = "Try again";
        public const string Edit = "Edit";
        public const string Share = "Share";
        public const string Delete = "Delete";
        public const string Discard = "Discard";
        public const string NoResults = "No Results";
        public const string PlaceholderFormat = "Search for {0}...";
        public const string Search = "Search...";
        public const string People = "people";
        public const string Ok = "Ok";
        public const string Proceed = "Proceed";
        public const string ShowMore = "Show more";
        public const string ShowLess = "Show less";

        public class ConfirmModal
        {
            public const string ImageUploadTitle = "Are you sure you want to delete your photo?";
            public const string ImageUploadDescription = "Please note that your profile picture serves as a visual identifier for others. Before removing it, carefully consider the importance of maintaining a professional and recognisable presence online.";
        }

        public class Dates
        {
            public const string StartDate = "Start date";
            public const string EndDate = "End date";
            public const string Month = "Month";
            public const string Year = "Year";
            public static readonly string[] WeekDays = { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };
        }
    }
}
