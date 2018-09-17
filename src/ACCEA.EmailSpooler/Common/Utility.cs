using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCEA.EmailSpooler
{
    public static class LoggingUtility
    {
        #region Members
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region Methods

        /// <summary>
        /// Method to Write the logs in the log file 
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static void WriteLog(ELogLevel logLevel, String log)
        {
            if (logLevel.Equals(ELogLevel.DEBUG))
            {
                logger.Debug(log);
            }
            else if (logLevel.Equals(ELogLevel.ERROR))
            {
                logger.Error(log);
            }
            else if (logLevel.Equals(ELogLevel.FATAL))
            {
                logger.Fatal(log);
            }
            else if (logLevel.Equals(ELogLevel.INFO))
            {
                logger.Info(log);
            }
            else if (logLevel.Equals(ELogLevel.WARN))
            {
                logger.Warn(log);
            }
        }

        #endregion
    }

    public enum ELogLevel
    {
        DEBUG = 1,
        ERROR,
        FATAL,
        INFO,
        WARN
    }

}
