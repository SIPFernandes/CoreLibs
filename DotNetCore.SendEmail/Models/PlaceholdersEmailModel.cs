using System.Net.Mail;
using System.Net.Mime;
using System.Resources;

namespace DotNetCore.SendEmail.Models;

public class PlaceholdersEmailModel
{
    public ResourceManager ResourceManager { get; set; }    
    public object EmailModel { get; set; }    
    public Dictionary<string, LinkedResource> LinkedResources { get; set; }    
    
    public PlaceholdersEmailModel(object emailModel, ResourceManager? resourceManager = null)
    {
        EmailModel = emailModel;
        LinkedResources = new();
        ResourceManager = resourceManager ?? Resources.ResourceManager;
    }
    
    public void AddLinkedResource(string id, string imageBase64, string name = null)
    {
        var result = CreateLinkedResource(id, imageBase64, name);

        if (result != null)
        {
            LinkedResources.Add(id, result);
        }        
    }

    public LinkedResource CreateLinkedResource(string id, string imageBase64, string name = null)
    {
        LinkedResource result = null;

        if (!string.IsNullOrEmpty(imageBase64))
        {
            var bytes = GetByteArrayFromBase64(imageBase64);

            result = new LinkedResource(new MemoryStream(bytes),
                MediaTypeNames.Image.Jpeg)
            {
                ContentId = id
            };

            if (!string.IsNullOrEmpty(name))
            {
                result.ContentType.Name = name;
            }            
        }

        return result;
    }
    
    private static byte[] GetByteArrayFromBase64(string imageBase64)
    {
        return string.IsNullOrEmpty(imageBase64)
            ? null
            : Convert.FromBase64String(imageBase64);
    }
}