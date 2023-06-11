using BlazorCore.Areas.Helpers;
using BlazorCore.Areas.Interfaces;
using BlazorCore.Data.Consts;
using BlazorCore.Data.Models.FeedModels;
using DotNetCore.Entities.MessageAggregate.CommentAggregate;
using DotNetCore.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text;

namespace BlazorCore.Components.CommentFeed
{
    public partial class CommentInput
    {
        [Parameter, EditorRequired]
        public FeedDataModel FeedData { set; get; } = default!;
        [Parameter]
        public CommentReplied? ReplyingTo { set; get; }
        [Parameter]
        public EventCallback<CommentReplied> ReplyingToChanged { set; get; }
        [Parameter]
        public string Message { set; get; } = string.Empty;
        [Parameter]
        public bool IsEdit { set; get; }
        [Parameter]
        public EventCallback<string> OnSend { set; get; }
        [Parameter]
        public EventCallback OnCancel { set; get; }
        [Parameter]
        public bool IsConnected { set; get; }
        [Parameter]
        public bool IsReply { set; get; }        
        [Inject]
        ICommentFeedService CommentFeedService { get; set; } = default!;        
        [Inject]
        NavigationManager NavigationManager { get; set; } = default!;
        [JSInvokable]
        public Task OnClickOutside() => OnFocusOut();               
        private ElementReference InputBoxRef { set; get; }
        private DotNetObjectReference<CommentInput>? Reference;
        private CommentReplied? _replyingTo { set; get; }
        private List<FileUpload>? _uploadedtFiles { set; get; }
        private FeedFileComponent? FeedFileRef;
        private string? ReplyToUser;
        private bool IsBoldActive;        
        private bool Focused;
        private bool DropdownOpened;
        private bool IsSendingMessage;

        protected override void OnInitialized()
        {
            if (NavigationManager.UrlEndsWith("comment"))
            {
                Focused = true;
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            if(_replyingTo != ReplyingTo)
            {
                ReplyToUser = null;
                _replyingTo = ReplyingTo;

                if(_replyingTo != null)
                {
                    if (_replyingTo.ReplyToReply)
                    {
                        ReplyToUser = CommentHelpers.SetUserReferenceComment(ReplyingTo.CreatorId, CommentFeedService.GetUserName(ReplyingTo.CreatorId));
                    }                    

                    Focused = true; 

                    await CommentEditInput.FocusAsync();
                }
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (IsEdit)
                {
                    await JSInteropService.SetInnerHTMLElement(CommentEditInput, Message);
                }

                if (IsReply || IsEdit)
                {
                    await OnFocusIn();
                }
                
                if (Focused)
                {
                    await JSInteropService.SetContentEditableFocusEnd(CommentEditInput);
                }
            }
        }

        private async Task RemoveReply()
        {
            _replyingTo = null;

            ReplyToUser = null;

            await ReplyingToChanged.InvokeAsync(_replyingTo);

            Focused = false;
        }

        private async Task RefreshFile(FileUpload file)
        {
            _uploadedtFiles!.Remove(file);
            
            await FeedFileRef!.RefreshFile(file);
        }
        
        private void OnFileUpload(FileUpload file)
        {
            _uploadedtFiles ??= new();

            _uploadedtFiles.Add(file);                       
        }

        private async Task RemoveFile(FileUpload file)
        {            
            _uploadedtFiles!.Remove(file);

            await FeedFileRef!.OnUploadDelete(file);
        }

        private async Task SendMessage()
        {
            if (!IsSendingMessage && (_uploadedtFiles is null || _uploadedtFiles!.Count == FeedFileRef!.GetCompletedUploads()))
            {
                await FeedFileRef!.OnConfirm();
                
                IsSendingMessage = true;

                var message = await JSInteropService.GetInnerHTMLElement(CommentEditInput);

                StringBuilder sb = new();

                if (!string.IsNullOrEmpty(message) || _uploadedtFiles != null && _uploadedtFiles.Count > 0)
                {
                    await JSInteropService.SetInnerHTMLElement(CommentEditInput, null);

                    Focused = false;

                    SpaceInputed = true;

                    IsBoldActive = false;

                    if (ReplyToUser != null)
                    {
                        sb.Append(ReplyToUser);
                        sb.Append(HtmlConsts.Space);                        
                    }

                    if (_uploadedtFiles != null)
                    {
                        foreach (var file in _uploadedtFiles)
                        {                            
                            var commentFile = CommentHelpers.SetFileReferenceComment(file.FileId, file.File.Name);

                            sb.Append(commentFile);
                            sb.Append(HtmlConsts.Space);                            
                        }

                        _uploadedtFiles = null;
                    }

                    sb.Append(message);

                    await OnSend.InvokeAsync(sb.ToString());
                }

                IsSendingMessage = false;
            }            
        }        

        private async Task OnReactionClick(string reaction)
        {            
            var smile = (string?)typeof(CommentsConsts.Reactions).GetField(reaction)?.GetValue(null);

            if (smile != null)
            {
                await JSInteropService.AppendToInnerHTML(CommentEditInput, smile);

                await JSInteropService.SetContentEditableFocusEnd(CommentEditInput);
            }            
        }

        private async void OnBoldClick()
        {
            await JSInteropService.SetContentEditableFocusEnd(CommentEditInput);
            
            IsBoldActive = !IsBoldActive;

            await JSInteropService.EditText("bold");
            
            StateHasChanged();
        }

        private async Task OnFocusIn()
        {
            Focused = true;

            if (Reference == null)
            {
                Reference = DotNetObjectReference.Create(this);
            }

            await JSInteropService.OnElementClickOutside(Reference, InputBoxRef); 
        }

        private async Task OnFocusOut()
        {
            if (!DropdownOpened)
            {
                var message = await JSInteropService.GetInnerHTMLElement(CommentEditInput);

                if (string.IsNullOrEmpty(message))
                {
                    Focused = false;

                    StateHasChanged();

                    if (IsReply)
                    {
                        await OnCancel.InvokeAsync();
                    }
                }
            }
        }
    }
}
