using System.Resources;
using System.Text;

namespace DotNetCore.SendEmail.Services;

public interface IPlaceholdersEmailService
{    
    public StringBuilder ReplacePlaceholders(object source, ResourceManager resourceManager,
        StringBuilder htmlBody = null);
}