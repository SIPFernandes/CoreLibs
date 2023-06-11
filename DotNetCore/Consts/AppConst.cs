namespace DotNetCore.Consts
{
    public class AppConst
    {
        public const string HTTPS = "https://";

        public static class Policies
        {
            public const string Admin = "Admin";
            public const string TwoFactorEnabled = "TwoFactorEnabled";
        }

        public class RealTime
        {            
            public const string NotificationsFeed = "NotificationsFeed";
        }

        public const int TwoFactorRememberMeDays = 14;
        
        public const int PasswordMinLength = 8;
    }
}
