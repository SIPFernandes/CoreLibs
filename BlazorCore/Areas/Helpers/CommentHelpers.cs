using BlazorCore.Data.Consts;
using DotNetCore.Entities.MessageAggregate.CommentAggregate;
using DotNetCore.Helpers;
using System.Text.RegularExpressions;

namespace BlazorCore.Areas.Helpers;

public static class CommentHelpers
{       
    public static List<string> GetTagDelimiterValues(string tag, string content, Action<string, string>? matchAction = null)
    {
        List<string> matchesString = new();

        var pattern = @"(?<=)<" + tag + @">(?'" + CommentsConsts.TagValue + @"'.*?)<\/" + tag + ">";
        var matches = Regex.Matches(content, pattern);

        foreach (Match match in matches)
        {
            matchesString.Add(match.Groups[CommentsConsts.TagValue].Value);

            matchAction?.Invoke(match.Groups[CommentsConsts.TagValue].Value, match.Value);
        }

        return matchesString;
    }

    public static string SetUserReferenceComment(string userId, string userName)
    {
        return "<span contentEditable='false' class='user-mention' " +
            "data-id='" + SetTagDelimiter(nameof(CommentsConsts.CommentTextTags.UserId), userId) + "'>"
            + userName + "</span>";
    }

    public static string SetFileReferenceComment(int fileId, string fileName)
    {
        var type = DotNetCore.Helpers.FilesHelper.GetFileExtension(fileName);

        var file = new CommentFileModel(fileId, fileName, type).Serialize();

        return SetTagDelimiter(nameof(CommentsConsts.CommentTextTags.FileReference),
            file);
    }
    
    private static string SetTagDelimiter(string tag, string content)
    {
        return "<" + tag + ">" + content + "</" + tag + ">";
    }    
}