using BlazorCore.Areas.Interfaces;
using BlazorCore.Data.Consts;
using BlazorCore.Data.Consts.ENConsts;
using BlazorCore.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorCore.Components.User
{
    public partial class UserPassword : ComponentBase
    {
        [Parameter, EditorRequired]
        public NewPasswordModel NewPasswordModel { set; get; } = default!;
        [Parameter]
        public bool FormIsValid { set; get; }
        [Parameter]
        public bool CanContinue { set; get; }
        [Parameter]
        public string NewPasswordLabel { set; get; } = UserConsts.Password;
        [Parameter]
        public EventCallback<bool> CanContinueChanged { set; get; }
        [Inject]
        private IJSInteropService JsInteropService { get; set; } = default!;
        private EditContext EditContext { set; get; } = default!;
        private bool IsValidInitial;
        private bool _firstInputChanged;
        private NewPasswordModel _newPasswordModel = default!;

        protected override async Task OnInitializedAsync()
        {
            _newPasswordModel = NewPasswordModel.Clone();
            
            EditContext = new EditContext(NewPasswordModel);

            EditContext.OnFieldChanged += async (sender, ev) =>
            {
                if (sender != null) await EditContext_OnFieldChanged(sender, ev);
            };

            if (CanContinue != FormIsValid)
            {
                await CanContinueChanged.InvokeAsync(FormIsValid);
            }

            IsValidInitial = FormIsValid;
        }

        protected override void OnParametersSet()
        {
            if (_newPasswordModel.Password != NewPasswordModel.Password
                || _newPasswordModel.ConfirmPassword != NewPasswordModel.ConfirmPassword)
            {
                _newPasswordModel.Password = NewPasswordModel.Password;
                
                _newPasswordModel.ConfirmPassword = NewPasswordModel.ConfirmPassword;
            
                FormIsValid = EditContext.Validate();

                IsValidInitial = FormIsValid;
            }
        }

        private async Task EditContext_OnFieldChanged(object sender, FieldChangedEventArgs e)
        {
            _firstInputChanged = true;
            
            IsValidInitial = true;
            
            FormIsValid = EditContext.Validate();

            if (CanContinue != FormIsValid)
            {
                CanContinue = FormIsValid;

                _newPasswordModel = NewPasswordModel.Clone();

                await CanContinueChanged.InvokeAsync(CanContinue);
            }
        }

        private async Task ValidatePassword(string inputValue)
        {
            await JsInteropService.ValidatePasswordById(inputValue, PasswordConsts.PasswordMatchId);
        }

        private async Task ValidateMatchPassword(string inputValue)
        {
            await JsInteropService.ValidatePasswordMatch(inputValue, PasswordConsts.PasswordId);
        }
    }
}