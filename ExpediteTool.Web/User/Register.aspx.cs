using ExpediteTool.Model;
using ExpediteTool.Model.Abstracts;
using ExpediteTool.Model.DataTransfer;
using ExpediteTool.Web.Models;
using Ninject;
using Ninject.Web;
using System;
using System.Web.UI;
using ExpediteTool.Web.Constants;
using ExpediteTool.Utilities;
using System.Net.Mail;
using System.Text;
using System.Web.Services;

/// <summary>
/// 
/// </summary>
namespace ExpediteTool.Web
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Register : PageBase
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
        /// Registers the user.
        /// </summary>
        /// <param name="item">The item.</param>
        public void RegisterUser(RegisterViewModel item)
        {
            TryUpdateModel(item);

            if (ModelState.IsValid)
            {
                string guid = Guid.NewGuid().ToString() + Guid.NewGuid().ToString();
                UsersInfoDto user = new UsersInfoDto()
                {
                    UserName = item.UserName,
                    Firstname = item.Firstname,
                    Lastname = item.Lastname,
                    LastLogin = DateTime.Now,
                    Password = item.Password,
                    Email = item.Email,
                    IsConfirmed = false,
                    Guid = guid,
                    RoleId = 1,//Normal user
                    Status = 0,//De-activate
                };
                ActionResult statusIdentity = UserRepository.Register(user);
                switch (statusIdentity)
                {
                    case ActionResult.SUCCESS:
                        SendMail(item.Email, guid);
                        Response.Redirect("Login.aspx");
                        break;
                    case ActionResult.EXIST:
                    case ActionResult.UNKNOWN:
                        ScriptManager.RegisterStartupScript(this, GetType(), "registerFail", "registerFail();", true);
                        break;
                }
            }
            ScriptManager.RegisterClientScriptBlock(this, typeof(string), "closeWaitingDialog", "closeWaitingDialog();", true);
        }

        private void SendMail(string recepient, string guid)
        {
            string from = ConfigManager.FROM;
            string host = ConfigManager.HOST;
            int port = ConfigManager.PORT;
            string username = ConfigManager.USERNAME;
            string pass = ConfigManager.PASSWORD;
            var hostAndPath = Request.Url.AbsoluteUri.Replace("User/Register.aspx", "User/ConfirmEmail.aspx?guid=" + guid);

            StringBuilder strBuilderBody = new StringBuilder();
            strBuilderBody.AppendLine("<table border='0' cellpadding='0' cellspacing='0' style='border-collapse:collapse;background-color:#fff;border:solid 1px #ededed!important' width='100%'>'");
            strBuilderBody.AppendLine("<tbody>");
            strBuilderBody.AppendLine("<tr>");
            strBuilderBody.AppendLine("<td>");
            strBuilderBody.AppendLine("<table align='center' border='0' cellpadding='0' cellspacing='0' style='border-collapse:collapse' width='100%'>");
            strBuilderBody.AppendLine("<tbody>");
            strBuilderBody.AppendLine("<tr>");
            strBuilderBody.AppendLine("<td align='center' style='padding:55px 10px 30px'>");
            strBuilderBody.AppendLine("<h2 style='margin:0;font-size:23px;font-weight:400;color:#2e3645'>Email Confirmation</h2>");
            strBuilderBody.AppendLine("</td>");
            strBuilderBody.AppendLine("</tr>");
            strBuilderBody.AppendLine("<tr>");
            strBuilderBody.AppendLine("<td align='center' style='padding-bottom:30px'>");
            strBuilderBody.AppendLine("<p style='font-size:15px;font-weight:300;color:#a5a5a5;line-height:1.4;margin:0 10px'>Please confirm your email address and agree to our Terms of Service. By clicking on the following link, you are agreeing to Expedite-Tool </p>");
            strBuilderBody.AppendLine("</td>");
            strBuilderBody.AppendLine("</tr>");
            strBuilderBody.AppendLine("</tbody>");
            strBuilderBody.AppendLine("</table>");
            strBuilderBody.AppendLine("</td>");
            strBuilderBody.AppendLine("</tr>");
            strBuilderBody.AppendLine("<tr>");
            strBuilderBody.AppendLine("<td style='padding-bottom:40px;border-bottom:1px solid #ededed'>");
            strBuilderBody.AppendLine("<table align='center' border='0' cellpadding='0' cellspacing='0' style='height:45px;margin:0 auto;table-layout:fixed' width='280'>");
            strBuilderBody.AppendLine("<tbody>");
            strBuilderBody.AppendLine("<tr>");
            strBuilderBody.AppendLine("<td align='center' bgcolor='#3bca96'><a href=" + hostAndPath + " style='font-size:13px;color:#ffffff;background-color:#3bca96;text-decoration:none;text-decoration:none;padding:11px 44px 10px;border:1px solid #3bca96;display:inline-block' target='_blank'>Confirm Your Email Address</a></td>");
            strBuilderBody.AppendLine("</tr>");
            strBuilderBody.AppendLine("</tbody>");
            strBuilderBody.AppendLine("</table>");
            strBuilderBody.AppendLine("</td>");
            strBuilderBody.AppendLine("</tr>");
            strBuilderBody.AppendLine("</tbody>");
            strBuilderBody.AppendLine("</table>");
            string body = strBuilderBody.ToString();
            try
            {
                SmtpMailSender.Email(host, recepient, body, "[ExpediteTool] Confirm Email", from, "ExpediteTool", username, pass);
                //smtpMailer.Send();
                LogService.Info("Send mail compeleted");
            }
            catch (Exception ex)
            {
                LogService.Error(ex.Message, ex);
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

        [Inject]
        public ILogService LogService { get; set; }
    }
}