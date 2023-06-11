
# Send Email Library
## Features

- Send emails
- Dynamic Html Tempates

## How to use

1. Reference the library in the project you intend to use.
2. Add settings in appsettings / user secrets
```json
"Email": {
  "Host": "",
  "Password": "",
  "Port": "",
  "Sender": ""
}
``` 
3. Add dependency injection
```c#
services.AddEmailsDependencyInjection(configuration, Resources.ResourceManager);
``` 
4. Create your own html template, or use an existing one, saving in Resources
5.   Create the class with the parameters that the html model is expecting
```c#
[EmailTemplate(nameof(Resources.EmailBaseTemplate))]  
public class EmailTemplateBaseModel  
{  
  public string EmailTitle { get; set; }  
  public string EmailDescription { get; set; }  
}
``` 
6.   Populate the object and send the email
```c#
var exampleEmail = new EmailTemplatesModels.LinkEmailTemplateModel  
{  
  MiddleInfo = new EmailTemplatesModels.LinkModel  
  {  
  Link = "www.google.pt",  
  LinkName = "Link name",  
  LinkWidth = "300"  
  },  
  EmailTitle = "Email title",  
  EmailDescription = "Email Description"  
};  
  
await _emailSenderService.SendEmailAsync("catarina2019visualforma@gmail.com",  
  "Subject",  
  new PlaceholdersEmailModel(exampleEmail));
``` 



## How to create EmailModels

### EmailTemplateAttribute
The model must have the attribute EmailTemplateAttribute that takes two parameters:
1. TemplatePath -  nameof the resource that has the html template
When getting Resource file, first we will look for the Resources of the project that are using the library,
if no resource is found, it will try to get the resource from the SendEmail project.

*Example of usage:*
```c#
[EmailTemplate(nameof(Resources.EmailBaseTemplate))]
``` 
```c#
[EmailTemplate(nameof(Resources.EmailBaseTemplate, true))]
``` 

### ListJoinHtmlAttribute
It is possible to have templates within templates and thus present a listing of elements, if we want to combine them with specific tags we can use this attribute.

Arguments:
- prefix
- sufix
- ignoreFirst => if we don't want the first element to have the separator (it is not mandatory)

*Example of usage:*
```c#
[ListJoinHtml("<td>", "</td>")]  
public List<UserAvatarEmail> UsersAssigned {  get; set; }
``` 

### How to use images
1. In the html, the image tag should have src="cid:##ImageId##"
```html
<img src="cid:##ImageId##">
``` 
2. In the email class we create, the image property will be a string.
```c#
[EmailTemplate(nameof(Properties.Resources.UserAvatar))]  
public class UserAvatarEmail
{  
  public string ImageId {  get; set; }
}
``` 
3. This property will store an id that will correspond to the id of the image.
```c#
UserAvatarEmail userAvatarEmail = new UserAvatarEmail(){
  ImageId = "1"
};
``` 
5. Multiple resources must be added for each ImageId to be used.
   This resource contains the image id, the base64 image and a name to use to represent the image.
```c#
var placeholderEmailModel = new PlaceholdersEmailModel(userAvatarEmail); 

var image = //some base64 image string; 
  
placeholderEmailModel.AddLinkedResource(userAvatarEmail.ImageId,  
  image, "user_name");
``` 


### How to define multiple templates for the same template
1. It is necessary for the model to inherit the class EmailMultipleTemplates.
2. You can add multiple EmailTemplate attributes with the various templates.
3. When filling in the template, you must inform which template to use, using the TemplateNumber property.
```c#
[EmailTemplate(nameof(Properties.Resources.UserAvatarTemplate))]  
[EmailTemplate(nameof(Properties.Resources.UserAvatarWithoutImage))] 
public class UserAvatarEmail : EmailMultipleTemplates
{  
  public string ImageId {  get; set; }
 
  public UserAvatarEmail(bool? hasImage = false)  
  {
    if (hasImage != null && hasImage == true)  
    {  
      TemplateNumber = 0;  
    } else  
    {  
      TemplateNumber = 1;  
    }
  }
}
``` 