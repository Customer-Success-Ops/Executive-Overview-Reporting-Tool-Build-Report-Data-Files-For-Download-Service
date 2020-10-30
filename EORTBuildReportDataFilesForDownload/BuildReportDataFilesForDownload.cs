using EORTBuildReportDataFilesForDownload.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Messaging;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace EORTBuildReportDataFilesForDownload
{
    public partial class BuildReportDataFilesForDownload : ServiceBase
    {
        public BuildReportDataFilesForDownload()
        {
            InitializeComponent();
            string queuePath = AppSettingServices.GetAppSetting("buildRawDataReportZipQueue");
            MessageQueue queue = null;
            XmlMessageFormatter formatter = new XmlMessageFormatter(new Type[1] { typeof(string) });
            try
            {
                //check if the queue exists
                if (!MessageQueue.Exists(queuePath))
                {
                    //create the queue since it doesn't exist and assign it to the queue object.
                    queue = MessageQueue.Create(queuePath);
                    queue.Formatter = formatter;
                }
                else
                {
                    //create the queue object.
                    queue = new MessageQueue() { Path = queuePath, Formatter = formatter };
                }

                //set up the method to call when a message is recieved. 
                queue.ReceiveCompleted += new ReceiveCompletedEventHandler(BuildReportDataFilesForDownload_ReceiveCompleted);
                //start listening for messages.
                queue.BeginReceive();
            }
            catch (MessageQueueException msgEx)
            {
                ErrorServices.LogMessagingServicesError(msgEx);
            }
            catch (Exception ex)
            {
                ErrorServices.LogServicesError(ex);
            }
        }

        private void BuildReportDataFilesForDownload_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            MessageQueue queue;
            Message message = null;
            int runID;
            int userID;
            string messageBody;
            string[] splitMessageBody;
            try
            {
                //assign the queue and get the message.
                queue = (MessageQueue)sender;
                message = queue.EndReceive(e.AsyncResult);

                messageBody = message.Body.ToString();
                splitMessageBody = messageBody.Split(new string[] { "~/~" }, StringSplitOptions.RemoveEmptyEntries);

                if (int.TryParse(splitMessageBody[0], out runID) && int.TryParse(splitMessageBody[1], out userID))
                {
                    FileServices.GenerateRawDataZipFile(runID, userID);
                }
                else
                {
                    string errorMessage = "Message was missing the run ID that is required to Build Raw Data Reports. Check the message body data for more specifics.";
                    ErrorServices.LogMessagingServicesError(message, errorMessage);
                }
                //clear out the message after processing.
                message = null;

                //start listening for the next message.
                queue.BeginReceive();
            }
            catch (MessageQueueException msgEx)
            {
                //if the message is not cleared out then log it for review.
                if (message != null)
                {
                    ErrorServices.LogMessagingServicesError(msgEx, message);
                }
                else
                {
                    ErrorServices.LogMessagingServicesError(msgEx);
                }
            }
            catch (Exception ex)
            {
                //if the message is not cleared out then log it for review.
                if (message != null)
                {
                    ErrorServices.LogMessagingServicesError(message, ex.Message);
                }
                ErrorServices.LogServicesError(ex);
            }
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
        }

        protected override void OnStop()
        {
            base.OnStop();
        }
    }
}
