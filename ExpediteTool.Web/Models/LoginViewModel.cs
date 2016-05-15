using System.ComponentModel.DataAnnotations;
/// <summary>
/// 
/// </summary>
namespace ExpediteTool.Web.Models
{
    public class LoginViewModel
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        [DataType(DataType.Text)]
        [StringLength(25, ErrorMessage = "User name than 25 chars")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "User name not empty")]
        public string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        [DataType(DataType.Password)]
        [StringLength(25, MinimumLength = 6, ErrorMessage = "Length password must than 6 chars")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password not empty")]
        public string Password
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the status non exist.
        /// </summary>
        /// <value>
        /// The status non exist.
        /// </value>
        [StringLength(0, ErrorMessage = "User not exist in system")]
        public string StatusNonExist
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the status deactivate.
        /// </summary>
        /// <value>
        /// The status deactivate.
        /// </value>
        [StringLength(0, ErrorMessage = "Account de-activate, contact Administrators please!")]
        public string StatusDeactivate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the status locked.
        /// </summary>
        /// <value>
        /// The status locked.
        /// </value>
        [StringLength(0, ErrorMessage = "Account lock-out, contact Administrators please!")]
        public string StatusLocked
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the status summary.
        /// </summary>
        /// <value>
        /// The status summary.
        /// </value>
        [StringLength(0, ErrorMessage = "Invalid login attempt")]
        public string StatusSummary
        {
            get;
            set;
        }
    }
}