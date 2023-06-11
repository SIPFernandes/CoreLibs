using BlazorCore.Areas.Helpers;
using BlazorCore.Areas.Interfaces;
using BlazorCore.Components.Dropdown;
using BlazorCore.Data.Consts;
using BlazorCore.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace BlazorCore.Components.CommentFeed
{
    public class UserReferenceComponent : ComponentBase
    {
        [JSInvokable]
        public async Task OnInputChange(string input) => await OnUserSearchChange(input);
        [Inject]
        protected IJSInteropService JSInteropService { get; set; } = default!;
        protected ElementReference CommentEditInput;
        protected DropdownTrigger? UserReferenceDropdown;
        private string UserReferenceInput = string.Empty;
        protected bool SpaceInputed = true;
        protected FeedUsersList? UsersListRef;

        protected async Task OnKeyboardInput(KeyboardEventArgs args)
        {
            if (!UserReferenceDropdown!.IsDropdownOpen())
            {
                if (args.Key == KeyboardConsts.At && SpaceInputed)
                {
                    await OpenUserReference();
                }
                else
                {
                    SpaceInputed = args.Code == KeyboardConsts.Space;
                }
            }
        }

        protected async Task OnKeyUp(KeyboardEventArgs args)
        {
            if (UserReferenceDropdown!.IsDropdownOpen())
            {
                if (args.Key == KeyboardConsts.Esc || args.Key == KeyboardConsts.Enter)
                {
                    if (args.Key == KeyboardConsts.Enter)
                    {
                        await UsersListRef!.SelectUser();
                    }

                    await CloseUserReference();
                }
            }
        }

        protected async Task OpenUserReference()
        {
            await UserReferenceDropdown!.OpenDropdown();

            var reference = DotNetObjectReference.Create(this);

            await JSInteropService.SubscribeToTextAfterCharacter(reference, CommentEditInput, KeyboardConsts.At);
        }

        protected async Task CloseUserReference()
        {
            SpaceInputed = true;

            if (string.IsNullOrEmpty(UserReferenceInput))
            {
                await JSInteropService.ReplaceInnerHTML(CommentEditInput, KeyboardConsts.At, string.Empty);
            }
            else
            {
                UserReferenceInput = string.Empty;
            }

            if (UserReferenceDropdown!.IsDropdownOpen())
            {
                await UserReferenceDropdown.CloseDropdown();
            }

            await JSInteropService.UnsubscribeToTextAfterCharacter(CommentEditInput);
        }

        protected async void OnUserSelect(AppUserModel user)
        {
            var userName = $"{user.FirstName} {user.LastName}";

            var content = CommentHelpers.SetUserReferenceComment(user.Id, userName);

            await JSInteropService.ReplaceInnerHTML(CommentEditInput, KeyboardConsts.At + UserReferenceInput,
                HtmlConsts.Space + content + HtmlConsts.Space);

            await CloseUserReference();
        }

        private async Task OnUserSearchChange(string searchString)
        {
            if (string.IsNullOrEmpty(UserReferenceInput) && (string.IsNullOrEmpty(searchString) || searchString.Length > 1))
            {
                await CloseUserReference();
            }
            else
            {
                UserReferenceInput = searchString;

                UsersListRef!.OnSearchChange(UserReferenceInput);
            }
        }
    }
}
