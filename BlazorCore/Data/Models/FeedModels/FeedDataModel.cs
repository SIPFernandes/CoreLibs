namespace BlazorCore.Data.Models.FeedModels
{
    public class FeedDataModel
    {
        public int FeedObjId { get; init; } 
        public string? FeedObjCreatorId { get; init; }
        public object? UploadFileData { get; init; }
        public FeedOptsModel FeedOpts { get; init; }
        public FeedCurrentUserModel? FeedCurrentUser { get; internal set; }
        
        public FeedDataModel(FeedOptsModel? feedOpts = null, int feedObjId = 0,
            string? feedObjCreatorId = null, object? uploadFileData = null)
        {
            if (feedObjId > 0 && feedObjCreatorId == null)
            {
                throw new ArgumentNullException(nameof(feedObjCreatorId));
            }

            FeedOpts = feedOpts ?? new();
            FeedObjId = feedObjId;
            FeedObjCreatorId = feedObjCreatorId;
            UploadFileData = uploadFileData;
        }
    }
}
