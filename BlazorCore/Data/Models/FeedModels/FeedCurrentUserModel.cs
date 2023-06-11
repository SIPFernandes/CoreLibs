namespace BlazorCore.Data.Models.FeedModels
{
    public class FeedCurrentUserModel
    {
        public string UserId { get; private set; }
        public bool IsAdmin { get; private set; }        

        public FeedCurrentUserModel(string userId, bool isAdmin)
        {
            UserId = userId;
            IsAdmin = isAdmin;
        }

        public FeedCurrentUserModel(string userId)
        {
            UserId = userId;
        }
    }
}
