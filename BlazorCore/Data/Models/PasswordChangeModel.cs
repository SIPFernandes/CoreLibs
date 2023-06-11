using System.ComponentModel.DataAnnotations;
using BlazorCore.Data.Consts;
using AppConstDotNetCore = DotNetCore.Consts.AppConst;

namespace BlazorCore.Data.Models
{
    public class PasswordChangeModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }
        public NewPasswordModel NewPasswordModel { get; set; }
    }
    
    public class NewPasswordModel
    {
        [Required]
        [StringLength(100, ErrorMessage = AppConst.Password.PasswordLengthErrorMessage,
            MinimumLength = AppConstDotNetCore.PasswordMinLength)]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).*$",
            ErrorMessage = "The {0} must contain at least 1 special character, 1 number, 1 upper case letter and 1 lower case letter")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }                

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
        
        public NewPasswordModel Clone()
        {
            return (NewPasswordModel)MemberwiseClone();
        }
    }
}
