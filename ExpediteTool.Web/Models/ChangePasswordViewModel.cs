using System.ComponentModel.DataAnnotations;

/// <summary>
/// 
/// </summary>
namespace ExpediteTool.Web.Models
{
    public class ChangePasswordViewModel
    {
        /// <summary>
        /// Gets or sets the old password.
        /// </summary>
        /// <value>
        /// The old password.
        /// </value>
        [Display(Name = "OldPassword")]
        public string OldPassword { get; set; }

        /// <summary>
        /// Gets or sets the new password.
        /// </summary>
        /// <value>
        /// The new password.
        /// </value>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "NewPassword")]
        public string NewPassword { get; set; }

        /// <summary>
        /// Gets or sets the password confirm.
        /// </summary>
        /// <value>
        /// The password confirm.
        /// </value>
        [DataType(DataType.Password)]
        [Display(Name = "PasswordConfirm")]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string PasswordConfirm { get; set; }
    }
}