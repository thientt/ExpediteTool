using System.ComponentModel.DataAnnotations;

/// <summary>
/// 
/// </summary>
namespace ExpediteTool.Web.Models
{
    public class RegisterViewModel
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the confirm password.
        /// </summary>
        /// <value>
        /// The confirm password.
        /// </value>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Gets or sets the firstname.
        /// </summary>
        /// <value>
        /// The firstname.
        /// </value>
        [Required]
        [DataType(DataType.Text)]
        [StringLength(25,ErrorMessage="")]
        [MinLength(0,ErrorMessage="")]
        [Display(Name="FirstName")]
        public string Firstname { get; set; }

        /// <summary>
        /// Gets or sets the lastname.
        /// </summary>
        /// <value>
        /// The lastname.
        /// </value>
        [Required]
        [DataType(DataType.Text)]
        [StringLength(25, ErrorMessage = "")]
        [MinLength(0, ErrorMessage = "")]
        [Display(Name = "LastName")]
        public string Lastname { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(50, ErrorMessage = "")]
        [MinLength(0, ErrorMessage = "")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}