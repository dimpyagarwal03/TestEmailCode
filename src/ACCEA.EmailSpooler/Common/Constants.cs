using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ACCEA.EmailSpooler.Common
{
    public static class Constants
    {
        public const string ConnectionString = "ACCEAConnection";
        public const string SMTPHost = "SMTPHost";
        public const string SMTPPort = "SMTPPort";
        public const string UserName = "UserName";
        public const string Password = "Password";
        public const string SubjectPrefix = "SubjectPrefix";
        public const string SenderAddress = "SenderAddress";
        public const string ACCEASecretariat = "ACCEASecretariat";
        public const string MaxRetryAttempts = "MaxRetryAttempts";
    }
}
