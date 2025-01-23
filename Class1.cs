using MimeKit;
using MailKit.Net.Smtp;
using System;

namespace libMailkitEmail
{
    public class LibEmail
    {
        public static void sendMail(string emailServer, int emailPort, string emailCredName, string emailCredPwd, bool emailEnableSSL, string emailTo, string emailCC, string emailBCC, string emailFrom, string emailReplyTo, string emailSubject, string emailBody, string sAttachment, bool emailDebug, string debugEmailTo = "akagan@leadingedgeadmin.com")
        {
            MimeMessage msg = new MimeMessage();
            string tmpFromAdd;
            tmpFromAdd = emailCredName.Contains("@") ? emailCredName : string.Concat(emailCredName, "@leadingedgeadmin.com");
            msg.From.Add(new MailboxAddress("Leading Edge Eservices", tmpFromAdd));
            /* add split function to TO/CC/BCC/ATTACHMENT */
            string[] mTo = emailTo.Split(',');
            string[] mCC = emailCC.Split(',');
            string[] mBCC = emailBCC.Split(',');
            string[] attachments = sAttachment == null ? null : sAttachment.Split(',');
            if (emailDebug)
            {
                msg.To.Add(new MailboxAddress("debugEmail", debugEmailTo));
            }
            else
            {
                try
                {
                    if (mTo.Length > 0)
                        foreach (string m in mTo)
                            if (m.Contains('@')) msg.To.Add(MailboxAddress.Parse(m));

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"To Error: {ex.Message}");

                }
                //use split to check if string is 0 length or greater
                try
                {
                    if (mCC.Length > 0)
                        foreach (string cc in mCC)
                            if (cc.Contains('@')) msg.Cc.Add(MailboxAddress.Parse(cc));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"CCerror: {ex.Message}");
                }
                try
                {
                    if (mBCC.Length > 0)
                        foreach (string bcc in mBCC)
                            if (bcc.Contains('@')) msg.Bcc.Add(MailboxAddress.Parse(bcc));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"BCCerror: {ex.Message}");
                }
                msg.Bcc.Add(MailboxAddress.Parse("autonotifies@leadingedgeadmin.com"));
            }
            msg.Subject = $"{(emailDebug ? "DEBUG: " : "")}{emailSubject} for {DateTime.Today.ToShortDateString()}";
            BodyBuilder bdy = new BodyBuilder();
            bdy.HtmlBody = emailBody;
            if (attachments != null)
                foreach (string attachment in attachments)
                    bdy.Attachments.Add(attachment);
            msg.Body = bdy.ToMessageBody();

            try
            {
                using (var smtp = new SmtpClient())
                {
                    smtp.Connect(emailServer, emailPort, emailEnableSSL);
                    smtp.Authenticate(emailCredName, emailCredPwd);
                    smtp.Send(msg);
                    smtp.Disconnect(true);
                    Console.WriteLine("Message sent");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"sendmail error: {ex.Message}");
            }
        }
    }
}
