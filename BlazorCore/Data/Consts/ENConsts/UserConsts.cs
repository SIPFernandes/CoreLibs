namespace BlazorCore.Data.Consts.ENConsts;

public class UserConsts
{
    public const string Password = "Password";
    public const string NewPassword = "New Password";
    public const string CurrentPassword = "Current Password";
    public const string RepeatPassword = "Repeat Password";
    public const string NewEmail = "New email";
    public const string RepeatEmail = "Repeat email";
    
    public static class PasswordErrors
    {
        public const string MaxLength = "Length must be at least {0} characters";
        public const string LowerUpperCase = "Must contain at least one upper and lowercase letter";
        public const string NumberSpecialCharacter = "Must contain at least one number and one special character";
        public const string PasswordMatch = "The passwords entered do not match.";
    }
}