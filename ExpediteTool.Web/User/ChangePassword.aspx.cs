using ExpediteTool.Model;
using ExpediteTool.Model.Abstracts;
using ExpediteTool.Web.Models;
using Ninject;
using Ninject.Web;
using System;
using System.Web.Security;
using System.Web.UI;

/// <summary>
/// 
/// </summary>
namespace ExpediteTool.Web
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ChangePassword : PageBase
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Users the change password.
        /// </summary>
        /// <param name="item">The item.</param>
        public void UserChangePassword(ChangePasswordViewModel item)
        {
            TryUpdateModel(item);
            if (ModelState.IsValid)
            {
                //2. update password for user
                ActionResult result = User.Identity.ChangePassword(UserRepository, item.OldPassword, item.PasswordConfirm);
                if (result == ActionResult.SUCCESS)
                {
                    FormsAuthentication.SignOut();
                    FormsAuthentication.RedirectToLoginPage();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "registerFail", "registerFail();", true);
                }
            }
        }

        /// <summary>
        /// Gets or sets the user repository.
        /// </summary>
        /// <value>
        /// The user repository.
        /// </value>
        [Inject]
        public IUserRepository UserRepository
        {
            get;
            set;
        }
    }
}