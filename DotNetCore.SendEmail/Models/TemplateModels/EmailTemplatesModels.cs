using DotNetCore.SendEmail.Attributes;

namespace DotNetCore.SendEmail.Models.TemplateModels;

[EmailTemplate(nameof(Resources.EmailBaseTemplate), true)]
public class EmailBaseTemplate
{
    public string EmailTitle { get; set; }
    public string PoweredByIcon { get; set; }
    public string EmailBanner { get; set; } = string.Empty;
    public string AppLink { get; set; }
    public string FooterInfo { get; set; }
    public string MiddleInfo { get; set; } = string.Empty;
}

[EmailTemplate(nameof(Resources.RoundedBtnTemplate), true)]
public class RoundedBtnTemplate
{
    public RoundedBtnTemplate(int width, int height, int borderRadius)
    {
        LinkWidth = width.ToString();
        LinkHeight = height.ToString();
        BorderRadius = borderRadius.ToString();
        BorderRadiusMSO = (borderRadius * 50 / (width/2)).ToString();
    }

    public string Link { get; set; }
    public string LinkName { get; set; }
    public string LinkWidth { get; }
    public string LinkHeight { get; }
    public string TextColor { get; set; } = "#FFFFFF";
    public string BackgroundColor { get; set; } = "#298DCC";
    public string BorderRadius { get; } = "100";
    public string BorderRadiusMSO { get; } = "5";
    public string MarginTop { get; set; } = "auto";
}

[EmailTemplate(nameof(Resources.CardTemplate), true)]
public class CardTemplate
{
    public string Description { get; set; }
    public string TextColor { get; set; } = "#2D2D2D";
    public string BorderColor { get; set; } = "#E4E4E4";
    public string BackgroundColor { get; set; } = "#FFFFFF";
    public string BorderRadius { get; set; } = "100";
    public RoundedBtnTemplate Btn { get; set; }
}

[EmailTemplate(nameof(Resources.UserAvatarTemplate), true)]
[EmailTemplate(nameof(Resources.UserAvatarLettersTemplate), true)]
public class UserAvatarEmailTemplate : EmailMultipleTemplates
{
    public string UserUrl { get; }
    public string FirstNameLetter { get; }
    public string LastNameLetter { get; }
    public string AvatarColor { get; set; }
    public string UserIconId { get; }
    public string UserAvatarSize { get; set; } = "32";
    public string UserAvatarFontSize { get; set; } = "12";

    public UserAvatarEmailTemplate(string firstLetter, string lastLetter, string userUrl,
        string userId, bool userHasImage = false)
    {
        FirstNameLetter = firstLetter;
        LastNameLetter = lastLetter;
        UserUrl = userUrl;
            
        if (userHasImage)
        {                
            UserIconId = userId;
            TemplateNumber = 0;
        }
        else
        {
            TemplateNumber = 1;
        }
    }
}

[EmailTemplate(nameof(Resources.HeaderTemplate), true)]
public class HeaderEmail
{
    public string Title { get; }
    public string BackgroundImage { get; }

    public HeaderEmail(string backgroundImage, string title)
    {
        BackgroundImage = backgroundImage;
        Title = title;
    }
}