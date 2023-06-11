using Microsoft.AspNetCore.Components;

namespace BlazorCore.Areas.Helpers;

public static class UrlHelper
{
    public static bool UrlEndsWith(this NavigationManager navigationManager, string lastUrlPart)
    {
        return navigationManager.Uri.EndsWith("/" + lastUrlPart);
    }
}