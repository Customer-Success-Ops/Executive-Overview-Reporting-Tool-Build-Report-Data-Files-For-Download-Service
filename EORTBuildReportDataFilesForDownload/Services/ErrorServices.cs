using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace EORTBuildReportDataFilesForDownload.Services
{
    public class ErrorServices
    {
        public static void LogServicesError(Exception ex)
        {
            string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;
            message += string.Format("Message: {0}", ex.Message);
            message += Environment.NewLine;
            if (ex.StackTrace != null)
            {
                message += string.Format("StackTrace: {0}", ex.StackTrace);
                message += Environment.NewLine;
            }
            if (ex.InnerException != null)
            {
                message += string.Format("Source: {0}", ex.InnerException.Source);
                message += Environment.NewLine;
                if (ex.InnerException.TargetSite != null)
                {
                    message += string.Format("TargetSite: {0}", ex.InnerException.TargetSite.ToString());
                    message += Environment.NewLine;
                }
            }
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;

            using (StreamWriter writer = new StreamWriter(BuildErrorFilePath(), true))
            {
                writer.WriteLine(message);
                writer.Close();
            }
        }

        public static void LogMessagingServicesError(MessageQueueException ex)
        {
            string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;
            message += string.Format("Message Queue Error Code: {0}", ex.MessageQueueErrorCode);
            message += Environment.NewLine;
            message += string.Format("Message Queue Data: {0}", ex.Data);
            message += Environment.NewLine;
            message += string.Format("Message: {0}", ex.Message);
            message += Environment.NewLine;
            if (ex.StackTrace != null)
            {
                message += string.Format("StackTrace: {0}", ex.StackTrace);
                message += Environment.NewLine;
            }
            if (ex.InnerException != null)
            {
                message += string.Format("Source: {0}", ex.InnerException.Source);
                message += Environment.NewLine;
                if (ex.InnerException.TargetSite != null)
                {
                    message += string.Format("TargetSite: {0}", ex.InnerException.TargetSite.ToString());
                    message += Environment.NewLine;
                }
            }
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;

            using (StreamWriter writer = new StreamWriter(BuildErrorFilePath(), true))
            {
                writer.WriteLine(message);
                writer.Close();
            }
        }

        public static void LogMessagingServicesError(MessageQueueException ex, Message messageThatFailed)
        {
            string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;
            message += string.Format("Message Queue Error Code: {0}", ex.MessageQueueErrorCode);
            message += Environment.NewLine;
            message += string.Format("Message Queue Data: {0}", ex.Data);
            message += Environment.NewLine;
            message += string.Format("Message: {0}", ex.Message);
            message += Environment.NewLine;
            if (ex.StackTrace != null)
            {
                message += string.Format("StackTrace: {0}", ex.StackTrace);
                message += Environment.NewLine;
            }
            if (ex.InnerException != null)
            {
                message += string.Format("Source: {0}", ex.InnerException.Source);
                message += Environment.NewLine;
                if (ex.InnerException.TargetSite != null)
                {
                    message += string.Format("TargetSite: {0}", ex.InnerException.TargetSite.ToString());
                    message += Environment.NewLine;
                }
            }
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += "Data from Message when exception occured:";
            message += Environment.NewLine;
            message += string.Format("Body Data: {0}", messageThatFailed.Body.ToString());

            using (StreamWriter writer = new StreamWriter(BuildErrorFilePath(), true))
            {
                writer.WriteLine(message);
                writer.Close();
            }
        }

        public static void LogMessagingServicesError(Message messageThatFailed, string reasonForError)
        {
            string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;
            message += string.Format("Reason for message error: {0}", reasonForError);
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += "Data from Message when exception occured:";
            message += Environment.NewLine;
            message += string.Format("Body Data: {0}", messageThatFailed.Body.ToString());

            using (StreamWriter writer = new StreamWriter(BuildErrorFilePath(), true))
            {
                writer.WriteLine(message);
                writer.Close();
            }
        }

        private static string BuildErrorFilePath()
        {
            Random rnd = new Random();
            string folderPath = AppSettingServices.GetAppSetting("folderPathForRunError");
            string rndNumAndExtention = rnd.Next().ToString() + ".txt";
            string errorLogFilePath = folderPath + "buildRawDataReports-error_win_service_" + rndNumAndExtention;
            while (File.Exists(errorLogFilePath))
            {
                rndNumAndExtention = rnd.Next().ToString() + ".txt";
                errorLogFilePath = folderPath + rndNumAndExtention;
            }

            return errorLogFilePath;
        }
    }
}
