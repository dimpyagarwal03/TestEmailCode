using ACCEA.EmailSpooler.Common;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace ACCEA.EmailSpooler
{
    public class DAL
    {
        static string connectionString = string.Empty;
        public DAL()
        {
            connectionString = ConfigurationManager.ConnectionStrings[Constants.ConnectionString.ToString()].ConnectionString;
        }


        public DataTable GetEmailNotifications()
        {
            DataTable dataTable = null;
            try
            {
                OracleConnection connection = GetDatabaseConnection();
                using (connection)
                {
                    OracleDataAdapter adapter = new OracleDataAdapter();
                    OracleCommand command = new OracleCommand();
                    command.Connection = connection;
                    command.InitialLONGFetchSize = 1000;
                    command.CommandText = "ACE_EMAIL_SPOOLER_PKG.GetEmailNotifications";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("T_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    adapter.SelectCommand = command;
                    dataTable = new DataTable();
                    adapter.Fill(dataTable);

                }
            }
            catch (Exception ex)
            {
                LoggingUtility.WriteLog(ELogLevel.ERROR, "Error in GetEmailNotifications() " + ex.Message.ToString());
            }
            return dataTable;
        }

        public DataTable GetEmailNotificationsWithAttachment()
        {
            DataTable dataTable = null;
            try
            {
                OracleConnection connection = GetDatabaseConnection();
                using (connection)
                {
                    OracleDataAdapter adapter = new OracleDataAdapter();
                    OracleCommand command = new OracleCommand();
                    command.Connection = connection;
                    command.InitialLONGFetchSize = 1000;
                    command.CommandText = "ACE_EMAIL_SPOOLER_PKG.GetEmailWithAttachments";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("T_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    adapter.SelectCommand = command;
                    dataTable = new DataTable();
                    adapter.Fill(dataTable);

                }
            }
            catch (Exception ex)
            {
                LoggingUtility.WriteLog(ELogLevel.ERROR, "Error in GetEmailNotificationsWithAttachment() " + ex.Message.ToString());
            }
            return dataTable;
        }

        public void UpdateMailAttemptsAndStatus(int notificationId, bool mailStatus, int maxAtempts)
        {
            try
            {
                OracleConnection connection = GetDatabaseConnection();
                using (connection)
                {
                    OracleDataAdapter adapter = new OracleDataAdapter();
                    OracleCommand command = new OracleCommand();
                    command.Connection = connection;
                    command.InitialLONGFetchSize = 1000;
                    command.CommandText = "ACE_EMAIL_SPOOLER_PKG.UpdateEmailStatus";
                    command.CommandType = CommandType.StoredProcedure;

                    OracleParameter objcmdParameter = new OracleParameter("p_notificationId", notificationId);
                    objcmdParameter.DbType = DbType.Int32;
                    objcmdParameter.Direction = ParameterDirection.Input;
                    command.Parameters.Add(objcmdParameter);
                    objcmdParameter = new OracleParameter("mailStatus", Convert.ToInt16(mailStatus));
                    objcmdParameter.DbType = DbType.Int32;
                    objcmdParameter.Direction = ParameterDirection.Input;
                    command.Parameters.Add(objcmdParameter);
                    objcmdParameter = new OracleParameter("maxAttempts", maxAtempts);
                    objcmdParameter.DbType = DbType.Int32;
                    objcmdParameter.Direction = ParameterDirection.Input;
                    command.Parameters.Add(objcmdParameter);
                    connection.Open();
                    int returnValue = command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                LoggingUtility.WriteLog(ELogLevel.ERROR, "Error in UpdateMailAttemptsAndStatus() " + ex.Message.ToString());
            }
        }

        public void UpdateEmailErrorLogs(int notificationId, string emailerror)
        {
            try
            {
                OracleConnection connection = GetDatabaseConnection();
                using (connection)
                {
                    OracleDataAdapter adapter = new OracleDataAdapter();
                    OracleCommand command = new OracleCommand();
                    command.Connection = connection;
                    command.InitialLONGFetchSize = 1000;
                    command.CommandText = "ACE_EMAIL_SPOOLER_PKG.LogEmailErrors";
                    command.CommandType = CommandType.StoredProcedure;

                    OracleParameter objcmdParameter = new OracleParameter("p_notificationId", notificationId);
                    objcmdParameter.DbType = DbType.Int32;
                    objcmdParameter.Direction = ParameterDirection.Input;
                    command.Parameters.Add(objcmdParameter);
                    objcmdParameter = new OracleParameter("strErrMessage", emailerror);
                    objcmdParameter.DbType = DbType.String;
                    objcmdParameter.Direction = ParameterDirection.Input;
                    command.Parameters.Add(objcmdParameter);
                    connection.Open();
                    int returnValue = command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                LoggingUtility.WriteLog(ELogLevel.ERROR, "Error in UpdateEmailErrorLogs() " + ex.Message.ToString());
            }
        }

        private OracleConnection GetDatabaseConnection()
        {
            OracleConnection connection = null;
            try
            {
                connection = new OracleConnection(connectionString);
            }
            catch (Exception ex)
            {
                LoggingUtility.WriteLog(ELogLevel.ERROR, "Error in GetDatabaseConnection() " + ex.Message.ToString());
            }
            return connection;
        }

    }
}
