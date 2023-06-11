using BlazorCore.Areas.Helpers;
using BlazorCore.Areas.Interfaces;
using BlazorCore.Data.Consts;
using BlazorCore.Data.Models.FeedModels;
using DotNetCore.Entities.MessageAggregate;
using DotNetCore.Entities.MessageAggregate.CommentAggregate;
using DotNetCore.Entities.MessageAggregate.NotificationsAggregate;
using DotNetCore.Enums;
using DotNetCore.Helpers;
using DotNetCore.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using System.Text.Json;
using static DotNetCore.Hubs.BaseHub;

namespace BlazorCore.Components.CommentFeed
{
    public abstract partial class CommentFeedBase<T> : ComponentBase, IAsyncDisposable where T : Comment
    {                         
        [Inject]
        IGetItemsService<T> GetItemsService { get; set; } = default!;
        [Inject]
        ICommentService<T> CommentService { get; set; } = default!;
        [Inject]
        INotificationService NotificationService { get; set;} = default!;
        [Inject]
        IReactionService ReactionService { get; set; } = default!;
        [Inject]
        IHubClientBaseService HubClientBaseService { get; set; } = default!;
        [Inject]
        NavigationManager NavigationManager { get; set; } = default!;
        protected FeedDataModel FeedData = default!;
        protected Expression<Func<T, bool>>? Expression;
        private List<T> Comments { get; set; } = default!;        
        private CommentReplied? ReplyingTo;                
        private T? RecentReplied;
        private T? GoToComment;
        private string? ChatRoomId;        

        public async ValueTask DisposeAsync()
        {
            if (ChatRoomId != null)
            {
                await HubClientBaseService.LeaveGroup(ChatRoomId);

                HubClientBaseService.UnsubscribeToMethod(ChatRoomId);
            }            
        }

        protected override async Task OnInitializedAsync()
        {
            var currentUser = SetCurrentUser();

            if (currentUser == null)
            {
                throw new ArgumentNullException(nameof(currentUser));
            }

            FeedData ??= new();

            FeedData.FeedCurrentUser = currentUser;                       

            await ValidateUrl();

            if (FeedData.FeedOpts.NestedReplies)
            {
                Expression<Func<T, bool>> expression = x => !FeedData.FeedOpts.CommentsSoftDelete ?
                    x.CommentReplied == null && !x.IsDeleted : x.CommentReplied == null;

                if (Expression != null)
                {
                    Expression = ExpressionHelper.AndExpression(Expression, expression);
                }
                else
                {
                    Expression = expression;
                }
            }
            else if (!FeedData.FeedOpts.CommentsSoftDelete)
            {
                if (Expression != null)
                {
                    Expression = ExpressionHelper.AndExpression(Expression, x => !x.IsDeleted);
                }
                else
                {
                    Expression = x => !x.IsDeleted;
                }
            }

            await GetComments();

            if (FeedData.FeedOpts.Realtime)
            {
                ChatRoomId = AppConst.RealTime.ChatFeed + FeedData.FeedObjId;

                await HubClientBaseService.ConnectToGroup(ChatRoomId);

                HubClientBaseService.SubscribeToMethod(ReceiveMessage, ChatRoomId);
            }
        }

        protected abstract FeedCurrentUserModel SetCurrentUser();    

        protected virtual T CreateComment(string commentText)
        {
            return (T)new Comment(commentText, FeedData.FeedCurrentUser!.UserId);
        }

        protected virtual async Task SendReationNotif(T commentObj, object? notifData = null)
        {
            await ReactionService.SendNotification(commentObj, FeedData.FeedCurrentUser!.UserId, notifData);
        }

        protected virtual async Task SendNotif(T comment, object? notifData = null)
        {            
            string? destUserIds = null;
            string type;

            var text = notifData != null ? notifData.Serialize() : string.Empty;            

            if (comment.CommentReplied != null && FeedData.FeedCurrentUser!.UserId != comment.CommentReplied.CreatorId)
            {
                destUserIds = comment.CommentReplied.CreatorId;
                type = BaseNotificationEnum.Type.Replied.ToString();

                var notif = new Notification(text, type, comment.CreatorId, destUserIds);

                await NotificationService.Insert(notif);
            }
            else if (FeedData.FeedCurrentUser!.UserId != FeedData.FeedObjCreatorId)
            {
                destUserIds = FeedData.FeedObjCreatorId;
                type = BaseNotificationEnum.Type.Comment.ToString();

                var notif = new Notification(text, type, comment.CreatorId, destUserIds);

                await NotificationService.Insert(notif);
            }

            string userIds = string.Empty;

            void GetDestUserIds(string userId)
            {
                if (userId != destUserIds)
                {
                    userIds += userId + ";";
                }                
            }            

            CommentHelpers.GetTagDelimiterValues(nameof(CommentsConsts.CommentTextTags.UserId),
                comment.Text, (x, y) => GetDestUserIds(x));

            if (!string.IsNullOrEmpty(userIds))
            {                
                var notif = new Notification(text, BaseNotificationEnum.Type.Mention.ToString(),
                    comment.CreatorId, userIds);

                await NotificationService.Insert(notif);
            }            
        }

        private async Task ReceiveMessage(HubMessage message)
        {
            var comment = message.GetObj<T>()!;

            if (!FeedData.FeedOpts.NestedReplies || comment.ReplyToId is null)
            {
                AddRecentComment(comment);

                await InvokeAsync(StateHasChanged);
            }
            else
            {                
                var commentReplied = Comments.SingleOrDefault(x => x.Id == comment.ReplyToId);

                if (commentReplied != null)
                {
                    commentReplied.RecentReplies ??= new();

                    commentReplied.RecentReplies.Add(comment.Id, comment);

                    await InvokeAsync(StateHasChanged);
                }                       
            }
        }

        private async Task GetMore()
        {
            await GetItemsService.GetMoreItems(Comments);
        }

        private async Task ValidateUrl()
        {
            if (NavigationManager.Uri.Contains("/comment/"))
            {
                try
                {
                    var array = NavigationManager.ToBaseRelativePath(NavigationManager.Uri).Split("/");

                    var commentId = 0;

                    for (var i = 0; i < array.Length; i++)
                    {
                        if (array[i] == "comment")
                        {
                            commentId = int.Parse(array[i + 1]);

                            break;
                        }
                    }

                    if (commentId > 0)
                    {
                        var comment = await CommentService.GetItems(x => x.CommentObjId == FeedData.FeedObjId && x.Id == commentId, 0, 1);

                        GoToComment = comment.SingleOrDefault();

                        if (GoToComment is null || GoToComment.IsDeleted && !FeedData.FeedOpts.CommentsSoftDelete)
                        {
                            throw new Exception();
                        }
                    }
                }
                catch (Exception)
                {
                    NavigationManager.NavigateTo(CssConst.Error, true);
                }
            }
        }

        private async Task GetComments()
        {
            if (GoToComment is null)
            {
                Comments = await GetItemsService.GetRecentItems(Expression);
            }
            else
            {
                GetItemsService.CurrentExpression = Expression;

                if (Expression != null)
                {
                    Expression = ExpressionHelper.AndExpression(Expression, x => x.Id >= GoToComment.Id);
                }
                else
                {
                    Expression = x => x.Id >= GoToComment.Id;
                }

                Comments = await GetItemsService.GetNItems(Expression);
            }            
        }

        private async Task AddComment(string commentText)
        {
            var comment = CreateComment(commentText);

            if (ReplyingTo != null)
            {
                comment.ReplyToId = ReplyingTo.Id;
                comment.CommentReplied = ReplyingTo;
            }            

            var newComment = await CommentService.InsertComment(comment);

            if (FeedData.FeedOpts.Realtime)
            {
                var msg = new BaseGroupMsgModel(ChatRoomId!, newComment,
                    AppConst.RealTime.ChatFeed, FeedData.FeedCurrentUser!.UserId);

                await HubClientBaseService.SendGroupMessage(msg, ChatRoomId);
            }

            if (FeedData.FeedOpts.SendNotifications)
            {
                await SendNotif(newComment);
            }            

            ReplyingTo = null;

            if (RecentReplied != null) 
            {
                RecentReplied.RecentReplies ??= new();

                RecentReplied.RecentReplies.Add(newComment.Id, newComment);

                RecentReplied = null;
            }
            else
            {
                AddRecentComment(newComment);
            }            
        }     
        
        private void AddRecentComment(T newComment) 
        {
            Comments = Comments.Prepend(newComment).ToList();            
        }

        private void OnReply((T replyingTo, T originalComment) data)
        {
            if (FeedData.FeedOpts.NestedReplies)
            {
                RecentReplied = data.originalComment;
            }            

            ReplyingTo = new CommentReplied(data.replyingTo);            
        }

        private void OnDelete(T comment)
        {
            if (!FeedData.FeedOpts.CommentsSoftDelete)
            {
                Comments.Remove(comment);
            }            

            StateHasChanged();
        }        
    }    
}
