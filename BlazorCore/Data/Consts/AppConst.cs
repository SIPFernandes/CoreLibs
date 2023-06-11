namespace BlazorCore.Data.Consts
{
    public class AppConst
    {      
        public class Identity
        {
            private const string Base = "Identity/Account/";
            public const string Login = Base + "Login";
            public const string LogOut = Base + "LogOut";
            public const string EnableAuthenticator = Base + "Manage/EnableAuthenticator";
            public const string ResetAuthenticator = Base + "Manage/ResetAuthenticator";
            public const string GenerateRecoveryCodes = Base + "Manage/GenerateRecoveryCodes";
        }

        public class Error
        {
            public const string Base = "/Error";
        }

        public class RealTime
        {
            public const string ChatFeed = "ChatFeed";            
        }

        public class NotificationsUrlConst
        {
            public const string Notifications = "/notifications";
        }

        public class Password
        {
            public const string PasswordLengthErrorMessage = "The {0} must be at least {2} characters long.";            
        }
    }
}
