using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.Net.Mail;
//2025-1-23: added encryption parameter to route to correct email server/barracuda
//2025-1-23: added debug parameter to send to debug email address
//2025-1-23: added smtpException to throw exception if email fails to send

namespace libMailkitEmail
{
    public class LibEmail
    {
        public static void sendMail(string emailServer, int emailPort, string emailCredName, string emailCredPwd, bool emailEnableSSL, string emailTo, string emailCC, string emailBCC, string emailFrom, string emailReplyTo, string emailSubject, string emailBody, string sAttachment, bool emailDebug, string debugEmailTo = "akagan@leadingedgeadmin.com")
        {
            string smtpServer = emailServer;
            int smtpPort = emailPort;
            string smtpUser = emailCredName;
            string smtpPwd = emailCredPwd;
            bool smtpSSL = emailEnableSSL;
            string smtpSubject = emailTo;
            string smtpBody = emailBody;
            string smtpAttachment = sAttachment;
            bool smtpDebug = emailDebug;
            string smtpDebugEmail = debugEmailTo;
            string smtpFrom = emailFrom;
            string smtpReplyTo = emailReplyTo;
            string smtpCC = emailCC;
            string smtpBCC = emailBCC;
            if (emailSubject.ToLower().Contains("[encrypt]"))
            {
                smtpServer = "smtp.outlook.com";
                smtpPort = 587;
                smtpUser = "eservices@leadingedgeadmin.com";
                smtpPwd = "Torn8o427#";
                smtpSSL = false;
                //remove "encrypt" from subject
                emailSubject = emailSubject.Replace("[encrypt]", "");
            }
            else
            {
                smtpServer = "mail.smtp2go.com";
                smtpPort = 2525;
                smtpUser = "LEA-eservices";
                smtpPwd = "MBHA2ai3u0xx1Mhd";
                smtpSSL = false;
            }


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
                //msg.Bcc.Add(MailboxAddress.Parse("autonotifies@leadingedgeadmin.com")); <--2025-1-23: no longer adding to autonotifies mailbox
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
                using (var smtp = new MailKit.Net.Smtp.SmtpClient())
                {
                    smtp.Connect(smtpServer, smtpPort, smtpSSL);
                    smtp.Authenticate(smtpUser, smtpPwd);
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
