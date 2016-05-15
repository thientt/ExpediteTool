using ExpediteTool.Model;
using ExpediteTool.Model.Abstracts;
using Ninject;
using System;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace ExpediteTool.Web
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Context != null)
            {
                if (Context.User != null)
                {
                    if (Context.User.Identity != null)
                    {
                        RoleType roleType = Context.User.IsRole(UserRepository);
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
                }
            }
        }

        protected void LoggingOut(object sender, LoginCancelEventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("~/Lots/ExpediteLot_List_HOME.aspx", true);
        }

        [Inject]
        public IUserRepository UserRepository { get; set; }
    }
}