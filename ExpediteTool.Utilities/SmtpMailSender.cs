using System.Diagnostics;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ExpediteTool.Utilities
{
    public class SmtpMailSender
    {
        /// <summary>
        /// Sends and email
        /// </summary>
        /// <param name="to">Message to address</param>
        /// <param name="body">Text of message to send</param>
        /// <param name="subject">Subject line of message</param>
        /// <param name="fromAddress">Message from address</param>
        /// <param name="fromDisplay">Display name for "message from address"</param>
        /// <param name="credentialUser">User whose credentials are used for message send</param>
        /// <param name="credentialPassword">User password used for message send</param>
        public static void Email(string host,
                                 string toAddress,
                                 string body,
                                 string subject,
                                 string fromAddress,
                                 string fromDisplay,
                                 string credentialUser,
                                 string credentialPassword,
                                 MailPriority mailpriority = MailPriority.Normal)
        {
            body = UpgradeEmailFormat(body);
            //  This  method is not completely implemented
            try
            {
                MailMessage mail = new MailMessage() { Body = body, IsBodyHtml = true };

                string[] toArray = toAddress.Split(';');

                foreach (string to in toArray)
                {
                    mail.To.Add(new MailAddress(to.Trim()));
                }

                mail.From = new MailAddress(fromAddress, fromDisplay, Encoding.UTF8);
                mail.Subject = subject;
                mail.SubjectEncoding = Encoding.UTF8;
                mail.Priority = mailpriority;

                using (SmtpClient smtp = new SmtpClient())
                {
                    // smtp.EnableSsl = true;
                    // This is necessary for gmail
                    //smtp.Credentials = new System.Net.NetworkCredential(credentialUser, credentialPassword);
                    smtp.Host = host;
                    smtp.Send(mail);
                }
            }
            catch
            {
                StringBuilder sb = new StringBuilder(1024);
                sb.Append("\\nTo:" + toAddress);
                sb.Append("\\nbody:" + body);
                sb.Append("\\nsubject:" + subject);
                sb.Append("\\nfromAddress:" + fromAddress);
                sb.Append("\\nfromDisplay:" + fromDisplay);
                sb.Append("\\ncredentialUser:" + credentialUser);
                sb.Append("\\ncredentialPasswordto:" + credentialPassword);
                sb.Append("\\nHosting:" + host);
                Debug.Print(sb.ToString());
            }
        }
        private static string UpgradeEmailFormat(string body)
        {
            // This has to be implemented as needed. Currently doing nothing!
            return body;
        }

    }
}
