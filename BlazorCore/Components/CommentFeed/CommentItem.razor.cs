using BlazorCore.Areas.Interfaces;
using BlazorCore.Components.Modal;
using BlazorCore.Data.Consts.ENConsts;
using BlazorCore.Data.Models.FeedModels;
using DotNetCore.Entities.MessageAggregate.CommentAggregate;
using DotNetCore.Helpers;
using DotNetCore.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using System.Text;

namespace BlazorCore.Components.CommentFeed
{
    public partial class CommentItem<T> : ComponentBase where T : Comment
    {
        [Parameter, EditorRequired]
        public FeedDataModel FeedData { get; set; } = default!;
        [Parameter, EditorRequired]
        public T CommentObj { get; set; } = default!;                
        [Parameter]
        public EventCallback<(T, T?)> OnReply { get; set; }
        [Parameter]
        public EventCallback<T> OnDelete { get; set; }
        [Parameter]
        public EventCallback<T> OnReacting { get; set; }
        [Inject]
        ICommentService<T> CommentService { get; set; } = default!;
        [Inject]
        IGetItemsService<T> GetItemsService { get; set; } = default!;
        [Inject]
        IJSInteropService JsService { set; get; } = default!;
        [Inject]
        NavigationManager NavigationManager { get; set; } = default!;
        private List<T>? Replies { get; set; }
        private Expression<Func<T, bool>> Expression = default!;
        private bool ShowingReplies;
        private bool Editing;
        private string? Message;
        private ModalConfirm? ModalRef;
        private string ModalTitle = FeedConsts.ConfirmModal.DeleteCommentTitle;
        private string ModalDescription = FeedConsts.ConfirmModal.DeleteCommentDescription;
        private string ConfirmBtn = GenericsConst.Delete;
        private ConfirmTypes ConfirmType;
        private bool IsActive;
        private T? GoToReply;

        protected override async Task OnInitializedAsync()
        {
            Expression = x => !FeedData.FeedOpts.CommentsSoftDelete ?
                x.ReplyToId == CommentObj.Id && !x.IsDeleted : x.ReplyToId == CommentObj.Id;

            if (FeedData.FeedOpts.NestedReplies)
            {
                if (CommentObj.CommentReplied == null && NavigationManager.Uri.Contains("/comment/" + CommentObj.Id))
                {
                    if (NavigationManager.Uri.Contains("/reply/"))
                    {                        
                        var id = int.Parse(NavigationManager.Uri.Split("/").Last());

                        var comment = await CommentService.GetItems(x => x.ReplyToId == CommentObj.Id && x.Id == id, 0, 1);

                        GoToReply = comment.SingleOrDefault();

                        if (GoToReply is null || GoToReply.IsDeleted && !FeedData.FeedOpts.CommentsSoftDelete)
                        {
                            throw new Exception();
                        }

                        GetItemsService.CurrentExpression = Expression;

                        Expression = ExpressionHelper.AndExpression(Expression, x => x.Id <= GoToReply.Id);

                        ShowingReplies = !ShowingReplies;

                        Replies = await GetItemsService.GetNOrderedItems(x => x.CreatorId, false, Expression);
                    }
                    else
                    {
                        await ActivateComment();
                    }
                }
                else if (CommentObj.CommentReplied != null && NavigationManager.Uri.Contains("/reply/" + CommentObj.Id))
                {
                    await ActivateComment();
                }
            }
            else if(NavigationManager.Uri.Contains("/reply/" + CommentObj.Id) || 
                NavigationManager.Uri.Contains("/comment/" + CommentObj.Id))
            {
                await ActivateComment();
            }
        }

        private async Task ActivateComment()
        {
            await JsService.ScrollToElementById("comment" + CommentObj.Id);

            IsActive = true;

            StateHasChanged();

            await Task.Delay(3000);

            IsActive = false;

            StateHasChanged();
        }

        private async Task DeleteComment()
        {
            await CommentService.DeleteComment(CommentObj.Id, FeedData.FeedOpts.CommentsSoftDelete, FeedData.FeedOpts.NestedReplies);

            if (FeedData.FeedOpts.CommentsSoftDelete)
            {
                CommentObj.IsDeleted = true;
                Replies = null;
                CommentObj.RecentReplies = null;
            }

            await OnDelete.InvokeAsync(CommentObj);
        }               
        
        private async Task OnEditTrigger()
        {
            Editing = true;

            Message = await JsService.GetInnerHTMLElementById($"message-{CommentObj.Id}");
        }

        private async Task UpdateComment(string messageText)
        {
            await CommentService.UpdateComment(CommentObj, messageText);
            
            Editing = false;
        }

        private async Task ShowReplies()
        {
            ShowingReplies = !ShowingReplies;            

            Replies ??= await GetItemsService.GetOrderedItems(x => x.CreatedAt, false, Expression);

            StateHasChanged();

            await JsService.ScrollToElementById("comment" + CommentObj.Id);
        }

        private async Task GetMoreReplies()
        {
            await GetItemsService.GetMoreOrderedItems(Replies, x => x.CreatedAt, false);
        }        

        private async Task Reply((T commentToReply, T? originalComment) data)
        {
            if (CommentObj.ReplyToId != null)
            {
                await OnReply.InvokeAsync(data);
            }
            else
            {
                data.originalComment = CommentObj;

                await OnReply.InvokeAsync(data);
            }
        }

        private void OnReplyDelete(T comment)
        {
            CommentObj.RepliesCount--;

            if (!FeedData.FeedOpts.CommentsSoftDelete)
            {
                Replies!.Remove(comment);
            }            

            StateHasChanged();
        }

        private async Task Share()
        {
            var value = string.Empty;

            if (FeedData.FeedOpts.NestedReplies && CommentObj.ReplyToId != null)
            {
                value += "/" + CommentObj.ReplyToId + "/reply/" + CommentObj.Id;
            }
            else
            {
                value += "/" + CommentObj.Id;
            }

            var index = NavigationManager.Uri.IndexOf("/comment");

            StringBuilder url = new();

            if (index > 0)
            {
                url.Append(NavigationManager.Uri[..index]);
            }
            else
            {
                url.Append(NavigationManager.Uri);
            }

            url.Append("/comment" + value);

            await JsService.CopyToClipBoard(url.ToString());
        }

        private void ConfirmModal(ConfirmTypes type)
        {
            ConfirmType = type;

            if (type == ConfirmTypes.DeleteComment)
            {
                ModalTitle = FeedConsts.ConfirmModal.DeleteCommentTitle;
                ModalDescription = FeedConsts.ConfirmModal.DeleteCommentDescription;
                ConfirmBtn = GenericsConst.Delete;                
            }
            else
            {
                ModalTitle = FeedConsts.ConfirmModal.DiscardChangesTitle;
                ModalDescription = string.Empty;
                ConfirmBtn = GenericsConst.Discard;                
            }

            ModalRef!.OpenModal();
        }

        private async Task OnModalConfirm()
        {
            if (ConfirmType == ConfirmTypes.DeleteComment)
            {
                await DeleteComment();
            }
            else
            {
                Editing = false;                      
            }
        }

        private enum ConfirmTypes
        {
            DeleteComment,
            DiscardDraft
        }
    }
}