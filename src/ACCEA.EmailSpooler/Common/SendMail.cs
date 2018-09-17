﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Net.Mime;
using ACCEA.EmailSpooler.Entities;
using ACCEA.EmailSpooler.Common;

namespace ACCEA.EmailSpooler
{
    public class SendMail
    {
        public SendMail()
        {

        }
        public bool TriggerEmail(Email email)
        {
            bool sendStatus = false;
            try
            {
               
                string from = Program.appConfig[Constants.SenderAddress];
                string userName = Program.appConfig[Constants.UserName];
                string password = Program.appConfig[Constants.Password];
                string smtpHost = Program.appConfig[Constants.SMTPHost];
                int port = Convert.ToInt32(Program.appConfig[Constants.SMTPPort]);
                string to = email.To;
                string subject = email.Subject;
                string body = email.Body;
                string attachmentName = email.AttachmentName;
                byte[] attachmentData = email.AttachmentData;

                MailMessage message = new MailMessage();
                message.IsBodyHtml = true;
                message.From = new MailAddress(from, "");
                message.To.Add(new MailAddress(to));
                if (!string.IsNullOrEmpty(email.Bcc))
                {
                    message.Bcc.Add(email.Bcc);
                }
                message.Subject = subject;

                message.Body = body;
                message.IsBodyHtml = false;
                if (!string.IsNullOrEmpty(attachmentName) && attachmentData != null)
                {
                    MemoryStream dataStream = new MemoryStream(attachmentData);
                    Attachment attachment = new Attachment(dataStream, attachmentName);
                    ContentType content = attachment.ContentType;
                    content.MediaType = MediaTypeNames.Text.RichText;
                    message.Attachments.Add(attachment);
                }
                using (var client = new System.Net.Mail.SmtpClient(smtpHost, port))
                {
                    client.Credentials = new NetworkCredential(userName, password);
                    client.EnableSsl = true;
                    Console.WriteLine("Attempting to send email...");
                    client.Send(message);
                    LoggingUtility.WriteLog(ELogLevel.INFO, "Email sent to " + email.To.ToString());
                    sendStatus = true;
                    Console.WriteLine("Email sent to " + email.To.ToString());
                }
            }
            catch (Exception ex)
            {
                sendStatus = false;
                LoggingUtility.WriteLog(ELogLevel.ERROR, "Email was not sent" + ex.Message);                
                Console.WriteLine("The email was not sent : " + ex.Message);
                throw ex;
            }
            return sendStatus;
        }
    }
}
