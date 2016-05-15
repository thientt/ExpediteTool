using ExpediteTool.Model;
using ExpediteTool.Model.Abstracts;
using ExpediteTool.Model.DataTransfer;
using Ninject;
using Ninject.Web;
using System;
using System.Web.UI;

namespace ExpediteTool.Web
{
    /// <summary>
    /// 
    /// </summary>
    public partial class EditProfile : PageBase
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                GetInfoUser();
        }

        /// <summary>
        /// Gets the information user.
        /// </summary>
        private void GetInfoUser()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var userInfo = User.Identity.GetUserInfo(UserRepository);
                FirstName = userInfo.Firstname;
                UserID = userInfo.UserId;
                LastName = userInfo.Lastname;
                Email = userInfo.Email;
            }
        }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public int UserID
        {
            get
            {
                return int.Parse(txtUserId.Text);
            }
            set
            {
                txtUserId.Text = value.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName
        {
            get
            {
                return txtLastName.Text;
            }
            set
            {
                txtLastName.Text = value;
            }
        }

        ///// <summary>
        ///// Gets or sets the first name.
        ///// </summary>
        ///// <value>
        ///// The first name.
        ///// </value>
        public string FirstName
        {
            get
            {
                return txtFirstName.Text;
            }
            set
            {
                txtFirstName.Text = value;
            }
        }

        ///// <summary>
        ///// Gets or sets the email.
        ///// </summary>
        ///// <value>
        ///// The email.
        ///// </value>
        public string Email
        {
            get { return txtEmail.Text; }
            set { txtEmail.Text = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [Inject]
        public IUserRepository UserRepository
        {
            get;
            set;
        }

        /// <summary>
        /// Handles the Click event of the btnSubmit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            UsersInfoDto user = new UsersInfoDto()
            {
                UserId = UserID,
                Firstname = FirstName,
                Lastname = LastName,
                Email = Email,
            };
            ActionResult result = UserRepository.UserUpdate(user);
            switch (result)
            {
                case ActionResult.SUCCESS:
                    ScriptManager.RegisterStartupScript(this.panelMain, GetType(), "updateSuccess", "updateSuccess();", true);
                    break;
                case ActionResult.FAIL:
                    ScriptManager.RegisterStartupScript(this.panelMain, GetType(), "updateFail", "updateFail();", true);
                    break;
            }
        }
    }
}