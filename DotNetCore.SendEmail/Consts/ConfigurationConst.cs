namespace DotNetCore.SendEmail.Consts;

public class ConfigurationConst
{
    public const string BackgroundServiceQueueCapacity = "BackgroundServiceQueueCapacity";
    
    public class EmailConst
    {
        public const string Host = "Email:Host";
        public const string Port = "Email:Port";
        public const string EmailSender = "Email:Sender";
        public const string EmailPassword = "Email:Password";
    }
}