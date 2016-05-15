using ExpediteTool.Model;
using ExpediteTool.Model.Abstracts;
using ExpediteTool.Web.Constants;
using ExpediteTool.Web.Models;
using Ninject;
using Ninject.Web;
using System;
using System.Web;
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
    public partial class Login : PageBase
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.UnobtrusiveValidationMode = System.Web.UI.UnobtrusiveValidationMode.WebForms;
        }

        /// <summary>
        /// Authencations the specified user name.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        private void Authencation(string userName)
        {
            FormsAuthenticationTicket formAuthenTicket;
            string cookiestr;
            HttpCookie httpCookie;
            formAuthenTicket = new FormsAuthenticationTicket(1, userName, DateTime.Now, DateTime.Now.AddDays(1), false, userName);
            cookiestr = FormsAuthentication.Encrypt(formAuthenTicket);
            httpCookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookiestr);
            httpCookie.Path = FormsAuthentication.FormsCookiePath;
            Response.Cookies.Add(httpCookie);

            if (!String.IsNullOrEmpty(userName))
            {
                RoleType roleType = UserRepository.Role(userName);
                switch (roleType)
                {
                    case RoleType.SuperAdmin:
                        IdentityExtension.IsAdmin = true;
                        IdentityExtension.IsContributor = false;
                        break;
                    case RoleType.Contributor:
                        IdentityExtension.IsContributor = true;
                        IdentityExtension.IsAdmin = false;
                        break;
                    case RoleType.NormalUser:
                        IdentityExtension.IsContributor = false;
                        IdentityExtension.IsAdmin = false;
                        break;
                }
            }
            Response.Redirect("~/Lots/ExpediteLot_List.aspx", true);
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

        /// <summary>
        /// Handles the Click event of the btnLogin control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                ActionResult login = UserRepository.Login(txtUserName.Text, txtPassword.Text);
                switch (login)
                {
                    case ActionResult.SUCCESS:
                        Authencation(txtUserName.Text);
                        break;
                    case ActionResult.NOTEXIST:
                        FailureText.Text = Message.MSG_USER_NON_EXITST;
                        ErrorMessage.Visible = true;
                        break;
                    case ActionResult.DEACIVATE:
                        FailureText.Text = Message.MSG_USER_DE_ACTIVATE;
                        ErrorMessage.Visible = true;
                        break;
                    case ActionResult.LOCKED:
                        FailureText.Text = Message.MSG_USER_LOCKED;
                        ErrorMessage.Visible = true;
                        break;
                    case ActionResult.CONNECTION:
                        FailureText.Text = Message.MSG_CAN_NOT_DATABASE;
                        ErrorMessage.Visible = true;
                        break;
                    default:
                        FailureText.Text = Message.MSG_USER_INVALID;
                        ErrorMessage.Visible = true;
                        break;
                }
            }
        }
    }
}