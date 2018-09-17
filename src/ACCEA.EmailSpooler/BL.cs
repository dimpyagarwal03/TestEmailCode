using ACCEA.EmailSpooler.Entities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace ACCEA.EmailSpooler
{
    public class BL
    {
        DAL dbContext;
        SendMail sendMail;
        public BL()
        {
            dbContext = new DAL();
            sendMail = new SendMail();
        }

        public void ProcessEmails()
        {

        }

        public void ProcessEmailNotifications()
        {
            Email email;
            DataTable emailNotificationsData = dbContext.GetEmailNotifications();
            if (emailNotificationsData != null && emailNotificationsData.Rows.Count > 0)
            {
                foreach (DataRow notification in emailNotificationsData.Rows)
                {
                    try
                    {
                        email = new Email();
                        email.To = Convert.ToString(notification["MailTo"]);
                        email.Subject = Convert.ToString(notification["MailSubject"]);
                        email.Body = Convert.ToString(notification["MailBody"]);
                        bool result=sendMail.TriggerEmail(email);
                        dbContext.UpdateMailAttemptsAndStatus(Convert.ToInt32(notification["NOTIFICATIONID"]),result,5);

                    }
                    catch (Exception ex)
                    {
                        dbContext.UpdateEmailErrorLogs(Convert.ToInt32(notification["NOTIFICATIONID"]), ex.Message.ToString());

                    }
                }
            }
        }

        public void ProcessEmailNotificationsWithAttachments()
        {
            Email email;
            DataTable emailNotificationsData = dbContext.GetEmailNotificationsWithAttachment();
            if (emailNotificationsData != null && emailNotificationsData.Rows.Count > 0)
            {
                foreach (DataRow notification in emailNotificationsData.Rows)
                {
                    try {
                        email = new Email();
                        email.To = Convert.ToString(notification["MailTo"]);
                        email.Subject = Convert.ToString(notification["MailSubject"]);
                        email.Body = Convert.ToString(notification["MailBody"]);
                        email.AttachmentName = Convert.ToString(notification["COMMFILENAME"]);
                        email.AttachmentData = (byte[])(notification["Communicationfile"]);
                        bool result = sendMail.TriggerEmail(email);
                        dbContext.UpdateMailAttemptsAndStatus(Convert.ToInt32(notification["NOTIFICATIONID"]), result,5);
                    }
                    catch(Exception ex)
                    {
                        dbContext.UpdateEmailErrorLogs(Convert.ToInt32(notification["NOTIFICATIONID"]), ex.Message.ToString());

                    }

                }
            }
        }


    }
}
