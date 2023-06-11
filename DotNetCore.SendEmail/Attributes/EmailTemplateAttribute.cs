namespace DotNetCore.SendEmail.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)] 
public class EmailTemplateAttribute : Attribute  
{  
    public string TemplatePath { get; }
    public bool UseEmailSenderResources { get; }
  
    public EmailTemplateAttribute(string templatePath, bool useEmailSenderResources = false)
    {
        TemplatePath = templatePath;
        UseEmailSenderResources = useEmailSenderResources;
    }
}