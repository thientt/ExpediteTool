using ExpediteTool.Model.Abstracts;
using Ninject;
using Ninject.Web;
using System;
namespace ExpediteTool.Web.User
{
    public partial class ConfirmEmail : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string guid = Request.QueryString.Get("guid");
            var result = UserRepository.ConfirmRegister(guid);
            switch (result)
            {
                case Model.ActionResult.SUCCESS:
                    Response.Redirect("~/User/Login.aspx", true);
                    break;
                default:
                    break;
            }
        }

        [Inject]
        public IUserRepository UserRepository { get; set; }
    }
}