using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EORTBuildReportDataFilesForDownload.Services
{
    public class AppSettingServices
    {
        public static string GetConnectionString(string key)
        {
            if (ConfigurationManager.ConnectionStrings[key] != null)
            {

                return ConfigurationManager.ConnectionStrings[key].ConnectionString;
            }
            else
            {
                throw new Exception("A Connection String with the Key: " + key + " was not found, so processing has stopped");
            }
        }

        public static string GetAppSetting(string key)
        {
            if (ConfigurationManager.AppSettings[key] != null)
            {

                return ConfigurationManager.AppSettings[key];
            }
            else
            {
                throw new Exception("App Setting with the Key: " + key + " was not found, so processing has stopped");
            }
        }
    }
}
