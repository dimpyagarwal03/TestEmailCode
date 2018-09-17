﻿using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace ACCEA.EmailSpooler
{
    class Program
    {
        static void Main(string[] args)
        {
            LoggingUtility.WriteLog(ELogLevel.INFO, "Application Start");
            GetAppSettings();
            BL bl = new BL();
            //bl.ProcessEmailNotifications();
            bl.ProcessEmailNotificationsWithAttachments();
            Console.ReadKey();
            LoggingUtility.WriteLog(ELogLevel.INFO, "Application Complete");
        }

        public static NameValueCollection appConfig;
        static void GetAppSettings()
        {
            appConfig = ConfigurationManager.AppSettings;
        }
    }
}
