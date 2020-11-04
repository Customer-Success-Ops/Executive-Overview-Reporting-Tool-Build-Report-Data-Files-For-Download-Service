using CsvHelper;
using EORTBuildReportDataFilesForDownload.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EORTBuildReportDataFilesForDownload.Services
{
    public class FileServices
    {
        public static void GenerateRawDataZipFile(int runID, int userID)
        {
            string tempFolderPath = AppSettingServices.GetAppSetting("tempFileLocation");
            string zipFilePath = tempFolderPath + "Raw_Data_Run-" + runID + ".zip";
            string rawDataFolderPath = tempFolderPath + "Raw_Data_Run-" + runID;
            string mobileAnalysisChartDataFilePath = rawDataFolderPath + "\\Mobile_Analysis_Chart_Raw_Data.csv";
            string durationRelatedReportDataFilePath = rawDataFolderPath + "\\Raw_Data.csv";
            string toEmailAddress = DataAccessServices.GetEmailAddressForUserID(userID);

            try
            {
                if (!DataAccessServices.CheckIfRawDataReportAlreadyExists(runID))
                {
                    if (Directory.Exists(rawDataFolderPath))
                    {
                        Directory.Delete(rawDataFolderPath, true);
                    }

                    Directory.CreateDirectory(rawDataFolderPath);
                    DataAccessServices.GetRawDataAndBuildReports(runID, mobileAnalysisChartDataFilePath, durationRelatedReportDataFilePath);
                    ZipFile.CreateFromDirectory(rawDataFolderPath, zipFilePath);
                    DataAccessServices.InsertRawDataReportsDetails(runID, zipFilePath);
                    Directory.Delete(rawDataFolderPath, true);
                }
                DataAccessServices.SetGeneratingRawDataReportFlagToFalse(runID);
                EmailServices.BuildSuccessEmail(runID, toEmailAddress);
            }
            catch (Exception ex)
            {
                ErrorServices.LogServicesError(ex);
                DataAccessServices.SetGeneratingRawDataReportFlagToFalse(runID);
                EmailServices.BuildFailEmail(runID, toEmailAddress, ex);
                Directory.Delete(rawDataFolderPath, true);
            }
        }

        public static void BuildMobileAnalysisChartRawDataReportCsvFile(List<MobileAnalysisChartRawDataRecord> rawDataRecords, string rawDataFilePath, bool requiresWrittingHeaders)
        {
            bool appendData = requiresWrittingHeaders ? false : true;
            using (TextWriter textWriter = new StreamWriter(rawDataFilePath, appendData))
            {
                using (CsvWriter csvWriter = new CsvWriter(textWriter))
                {
                    if (requiresWrittingHeaders)
                    {
                        csvWriter.WriteField("username");
                        csvWriter.WriteField("firstName");
                        csvWriter.WriteField("lastName");
                        csvWriter.WriteField("assetTitle");
                        csvWriter.WriteField("formattedAssetID");
                        csvWriter.WriteField("originalAssetID");
                        csvWriter.WriteField("assetType");
                        csvWriter.WriteField("assetSubType");
                        csvWriter.WriteField("formattedAccessDate");
                        csvWriter.WriteField("sourceType");
                        csvWriter.WriteField("deviceType");
                        csvWriter.WriteField("curriculum");
                        csvWriter.WriteField("series");
                        csvWriter.NextRecord();
                    }

                    foreach (MobileAnalysisChartRawDataRecord record in rawDataRecords)
                    {
                        csvWriter.WriteField(record.Username);
                        csvWriter.WriteField(record.FirstName);
                        csvWriter.WriteField(record.LastName);
                        csvWriter.WriteField(record.AssetTitle);
                        csvWriter.WriteField(record.FormattedAssetID);
                        csvWriter.WriteField(record.OriginalAssetID);
                        csvWriter.WriteField(record.AssetType);
                        csvWriter.WriteField(record.AssetSubType);
                        csvWriter.WriteField(record.FormattedAccessDate);
                        csvWriter.WriteField(record.SourceType);
                        csvWriter.WriteField(record.DeviceType);
                        csvWriter.WriteField(record.Curriculum);
                        csvWriter.WriteField(record.Series);
                        csvWriter.NextRecord();
                    }
                }
            }
        }

        public static void BuildDurationRelatedRawDataReportCsvFile(List<DurationRawDataRecord> rawDataRecords, string rawDataFilePath, bool requiresWrittingHeaders)
        {
            bool appendData = requiresWrittingHeaders ? false : true;
            using (TextWriter textWriter = new StreamWriter(rawDataFilePath, appendData))
            {
                using (CsvWriter csvWriter = new CsvWriter(textWriter))
                {
                    if (requiresWrittingHeaders)
                    {
                        csvWriter.WriteField("username");
                        csvWriter.WriteField("originalAssetID");
                        csvWriter.WriteField("formattedAssetID");
                        csvWriter.WriteField("formattedAccessDate");
                        csvWriter.WriteField("assetTitle");
                        csvWriter.WriteField("assetType");
                        csvWriter.WriteField("assetSubType");
                        csvWriter.WriteField("timesAccessed");
                        csvWriter.WriteField("expectedDuration");
                        csvWriter.WriteField("actualDuration");
                        csvWriter.WriteField("pageReads");
                        csvWriter.WriteField("curriculum");
                        csvWriter.WriteField("series");
                        csvWriter.NextRecord();
                    }

                    foreach (DurationRawDataRecord record in rawDataRecords)
                    {
                        csvWriter.WriteField(record.Username);
                        csvWriter.WriteField(record.OriginalAssetID);
                        csvWriter.WriteField(record.FormattedAssetID);
                        csvWriter.WriteField(record.FormattedAccessDate);
                        csvWriter.WriteField(record.AssetTitle);
                        csvWriter.WriteField(record.AssetType);
                        csvWriter.WriteField(record.AssetSubType);
                        csvWriter.WriteField(record.TimesAccessed);
                        csvWriter.WriteField(record.ExpectedDuration);
                        csvWriter.WriteField(record.ActualDuration);
                        csvWriter.WriteField(record.PageReads);
                        csvWriter.WriteField(record.Curriculum);
                        csvWriter.WriteField(record.Series);
                        csvWriter.NextRecord();
                    }
                }
            }
        }
    }
}
