using System.Collections;
using System.Reflection;
using System.Resources;
using System.Text;
using DotNetCore.SendEmail.Attributes;
using DotNetCore.SendEmail.Models.TemplateModels;

namespace DotNetCore.SendEmail.Services;

public class PlaceholdersEmailService : IPlaceholdersEmailService
{
    private const string PlaceholderTag = "##";

    public StringBuilder ReplacePlaceholders(object source, ResourceManager resourceManager, StringBuilder htmlBody = null)
    {
        htmlBody ??= new StringBuilder(GetTemplateHtml(source, resourceManager));

        string replacement = null;

        foreach (var property in source.GetType().GetProperties())
        {
            if (property.DeclaringType ==
                typeof(EmailMultipleTemplates)) continue;
            
            if (property.PropertyType == typeof(string))
            {
                replacement = property.GetValue(source)?.ToString() ?? string.Empty;
            }
            else
            {
                var value = property.GetValue(source);

                if (value != null)
                {
                    string templateHtmlString = null;

                    if (property.PropertyType.IsGenericType &&
                        property.PropertyType.GetInterfaces().Any(x =>
                            x.IsGenericType &&
                            x.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
                    {
                        var attribute = property.GetCustomAttribute<ListJoinHtmlAttribute>();

                        string prefix = string.Empty, suffix = string.Empty;
                        bool ignoreFirst = false;

                        if (attribute != null)
                        {
                            prefix = attribute.Prefix;
                            suffix = attribute.Suffix;
                            ignoreFirst = attribute.IgnoreFirst;
                        }

                        var stringBuilder = new StringBuilder();

                        int i = 0;
                        
                        foreach (var v in (IEnumerable) value)
                        {
                            if (!v.GetType().IsSubclassOf(
                                typeof(EmailMultipleTemplates)))
                            {
                                templateHtmlString ??= GetTemplateHtml(v, resourceManager);
                            }

                            var templateHtml =
                                string.IsNullOrEmpty(templateHtmlString)
                                    ? null
                                    : new StringBuilder(templateHtmlString);

                            if (i == 0 && ignoreFirst)
                            {
                                stringBuilder.Append(ReplacePlaceholders(v, resourceManager, templateHtml));
                            }
                            else
                            {
                                stringBuilder.Append(prefix +
                                    ReplacePlaceholders(v, resourceManager, templateHtml) + suffix);
                            }
                            
                            i++;
                        }

                        replacement = stringBuilder.ToString();
                    }
                    else
                    {
                        replacement = ReplacePlaceholders(value, resourceManager).ToString();
                    }
                }
            }

            htmlBody =
                htmlBody.Replace($"{PlaceholderTag}{property.Name}{PlaceholderTag}",
                    replacement);

            replacement = null;
        }

        return htmlBody;
    }

    private string GetTemplateHtml(object source, ResourceManager resourceManager)
    {
        var attributes = source.GetType()
            .GetCustomAttributes(typeof(EmailTemplateAttribute), true);

        var templateNumber = 0;

        if (source.GetType()
            .IsSubclassOf(typeof(EmailMultipleTemplates)))
        {
            var temp = ((EmailMultipleTemplates) source).TemplateNumber;

            if (attributes.Length >= temp)
            {
                templateNumber = ((EmailMultipleTemplates) source)
                    .TemplateNumber;
            }
        }

        var attribute = (EmailTemplateAttribute) attributes[templateNumber];
        
        var resource = attribute.UseEmailSenderResources
            ? Resources.ResourceManager
            : resourceManager;

        return resource.GetString(attribute.TemplatePath);
    }
}