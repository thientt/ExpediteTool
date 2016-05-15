using ExpediteTool.Model.Abstracts;
using ExpediteTool.Utilities;
using ExpediteTool.Web.Constants;
using ExpediteTool.Web.Models;
using Ninject;
using Ninject.Web;
using System;
using System.Web.UI;
using System.Net.Mail;
using System.Text;

namespace ExpediteTool.Web
{
    public partial class RecoverPassword : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //1. check QueryString has email and guid
            string email = Request.QueryString.Get("email");
            string guid = Request.QueryString.Get("guid");
            if (!String.IsNullOrEmpty(email) && !String.IsNullOrEmpty(guid))
            {
                //2. If (has) then update database and redirect user to Login page.
                var result = UserRepository.ConfirmRecoverPassword(email, guid);
                switch (result)
                {
                    case Model.ActionResult.SUCCESS:
                        Response.Redirect("~/User/Login.aspx", true);
                        break;
                    default:
                        break;
                }
            }
        }

        public void Recover(RecoverPasswordViewModel item)
        {
            TryUpdateModel(item);
            if (ModelState.IsValid)
            {
                //1.  General password hash temp and guid
                string guid = Guid.NewGuid().ToString() + Guid.NewGuid().ToString();
                string plainText = guid.Substring(0, 6);
                string password = AppCipher.EncryptCipher(plainText);

                //2. Save password temp and field isRecoverPassword to database
                var result = UserRepository.RequestRecoverPassword(item.Email, guid, password);
                switch (result)
                {
                    case Model.ActionResult.SUCCESS:
                        //3. send mail in the attached password temp
                        SendMain(item.Email, guid, plainText);
                        Response.Redirect("Login.aspx");
                        break;
                    default:
                        ScriptManager.RegisterClientScriptBlock(this, typeof(string), "recoverFail", "recoverFail();", true);
                        break;
                }
            }

            ScriptManager.RegisterClientScriptBlock(this, typeof(string), "closeWaitingDialog", "closeWaitingDialog();", true);
        }

        private void SendMain(string recepient, string guid, string password)
        {
            string from = ConfigManager.FROM;
            string host = ConfigManager.HOST;
            int port = ConfigManager.PORT;
            string username = ConfigManager.USERNAME;
            string pass = ConfigManager.PASSWORD;
            var hostAndPath = String.Format("{0}?email={1}&guid={2}", Request.Url.AbsoluteUri, recepient, guid);

            StringBuilder strBuilderBody = new StringBuilder();
            strBuilderBody.AppendLine("Password: " + password);
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
            strBuilderBody.AppendLine("<td align='center' bgcolor='#3bca96'><a href=" + hostAndPath + " style='font-size:13px;color:#ffffff;background-color:#3bca96;text-decoration:none;text-decoration:none;padding:11px 44px 10px;border:1px solid #3bca96;display:inline-block' target='_blank'>Recover Password</a></td>");
            strBuilderBody.AppendLine("</tr>");
            strBuilderBody.AppendLine("</tbody>");
            strBuilderBody.AppendLine("</table>");
            strBuilderBody.AppendLine("</td>");
            strBuilderBody.AppendLine("</tr>");
            strBuilderBody.AppendLine("</tbody>");
            strBuilderBody.AppendLine("</table>");
            string body = strBuilderBody.ToString();
            SmtpMailSender.Email(host, recepient, body, "[ExpediteTool] Recover Password", from, "ExpediteTool", username, pass);
        }

        [Inject]
        public IUserRepository UserRepository { get; set; }
    }
}