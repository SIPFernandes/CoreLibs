namespace DotNetCore.SendEmail.Attributes;

[AttributeUsage(AttributeTargets.Property)] 
public class ListJoinHtmlAttribute : Attribute  
{  
    public string Prefix { get; }
    public string Suffix { get; }
    public bool IgnoreFirst { get; }
  
    public ListJoinHtmlAttribute(string prefix, string suffix, bool ignoreFirst = false)
    {
        Prefix = prefix;
        Suffix = suffix;
        IgnoreFirst = ignoreFirst;
    }
}