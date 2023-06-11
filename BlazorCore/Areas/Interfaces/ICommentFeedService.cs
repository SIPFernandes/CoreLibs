using BlazorCore.Data.Models;

namespace BlazorCore.Areas.Interfaces
{
    public interface ICommentFeedService
    {
        public string CurrentUserId { get; }
        public IEnumerable<AppUserModel> Users { get; }
        public string GetUserName(string userId);            
    }
}
