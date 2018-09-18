using ACCEA.EmailSpooler.Common;
using ACCEA.EmailSpooler.Entities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Sockets;
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
            bool IsConnectionSuccess=TestConnection();
            LoggingUtility.WriteLog(ELogLevel.INFO, "Emails processing started");
            ProcessEmailNotifications(IsConnectionSuccess);
            ProcessEmailNotificationsWithAttachments(IsConnectionSuccess);
            LoggingUtility.WriteLog(ELogLevel.INFO, "Emails processing completed");
        }

        private bool TestConnection()
        {
            bool result = false;
            try {
                SendMail mailObject = new SendMail();
                result= mailObject.TestSMTPConnection();
            }
            catch(Exception ex)
            {
                dbContext.UpdateEmailErrorLogs(0, ex.Message.ToString(), result);

            }
            return result;
        }

        public void ProcessEmailNotifications(bool IsConnectionSuccess)
        {
            LoggingUtility.WriteLog(ELogLevel.INFO, "Processing non attachment emails started");
            Email email;
            DataTable emailNotificationsData = dbContext.GetEmailNotifications();
            if (emailNotificationsData != null && emailNotificationsData.Rows.Count > 0)
            {
                foreach (DataRow notification in emailNotificationsData.Rows)
                {
                    int notificationId = 0;
                    try
                    {
                        notificationId = Convert.ToInt32(notification["NOTIFICATIONID"]);
                        email = new Email();
                        email.To = Convert.ToString(notification["MailTo"]);
                        if (!string.IsNullOrEmpty(Convert.ToString(notification["EmailTEMPLATEID"])) && !string.IsNullOrEmpty(Convert.ToString(notification["EVENTDATE"])))
                        {
                            email.Bcc = Program.appConfig[Constants.ACCEASecretariat].ToString();
                        }
                        email.Subject = Convert.ToString(notification["MailSubject"]);
                        email.Body = Convert.ToString(notification["MailBody"]);
                        bool result = sendMail.TriggerEmail(email);
                        dbContext.UpdateMailAttemptsAndStatus(notificationId, result, Convert.ToInt32(Program.appConfig[Constants.MaxRetryAttempts]));
                    }
                    catch (Exception ex)
                    {
                        LoggingUtility.WriteLog(ELogLevel.ERROR, "ProcessEmailNotifications() | Error in sending emails : " + notificationId.ToString() + " " + ex.Message.ToString());
                        dbContext.UpdateEmailErrorLogs(notificationId, ex.Message.ToString(), IsConnectionSuccess);
                    }
                }
            }
            else LoggingUtility.WriteLog(ELogLevel.INFO, "No pending emails for process");
        }

        public void ProcessEmailNotificationsWithAttachments(bool IsConnectionSuccess)
        {
            LoggingUtility.WriteLog(ELogLevel.INFO, "Processing attachment emails started");
            Email email;
            DataTable emailNotificationsData = dbContext.GetEmailNotificationsWithAttachment();
            if (emailNotificationsData != null && emailNotificationsData.Rows.Count > 0)
            {
                foreach (DataRow notification in emailNotificationsData.Rows)
                {
                    int notificationId = 0;
                    try
                    {
                        notificationId = Convert.ToInt32(notification["NOTIFICATIONID"]);
                        email = new Email();
                        email.To = Convert.ToString(notification["MailTo"]);
                        if (!string.IsNullOrEmpty(Convert.ToString(notification["EmailTEMPLATEID"])) && !string.IsNullOrEmpty(Convert.ToString(notification["EVENTDATE"])))
                        {
                            email.Bcc = Program.appConfig[Constants.ACCEASecretariat].ToString();
                        }
                        email.Subject = Convert.ToString(notification["MailSubject"]);
                        email.Body = Convert.ToString(notification["MailBody"]);
                        email.AttachmentName = Convert.ToString(notification["COMMFILENAME"]);
                        email.AttachmentData = (byte[])(notification["Communicationfile"]);
                        bool result = sendMail.TriggerEmail(email);
                        dbContext.UpdateMailAttemptsAndStatus(notificationId, result,Convert.ToInt32(Program.appConfig[Constants.MaxRetryAttempts]));
                    }
                    catch (Exception ex)
                    {
                        LoggingUtility.WriteLog(ELogLevel.ERROR, "ProcessEmailNotificationsWithAttachments() | Error in sending emails : " + notificationId.ToString() + " " + ex.Message.ToString());
                        dbContext.UpdateEmailErrorLogs(notificationId, ex.Message.ToString(), IsConnectionSuccess);
                    }
                }
            }
            else LoggingUtility.WriteLog(ELogLevel.INFO, "No pending emails with attachment for process");
        }

    }
}
