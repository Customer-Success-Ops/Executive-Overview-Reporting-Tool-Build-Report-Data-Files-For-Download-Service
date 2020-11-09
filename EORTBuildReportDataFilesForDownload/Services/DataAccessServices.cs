using EORTBuildReportDataFilesForDownload.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace EORTBuildReportDataFilesForDownload.Services
{
    public class DataAccessServices
    {
        private static readonly string servicesDBConnectionString = SqlDataFactory.GetConnectionString();

        public static string GetEmailAddressForUserID(int userID)
        {
            string emailAddressForUser = string.Empty;
            string query = "SELECT TOP 1 email " +
                                   "FROM execOverviewTool_UserDetails " +
                                   "WHERE userID = @userID";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "userID", userID }
            };

            using (DbConnection connection = SqlDataFactory.CreateAndOpenConnection(servicesDBConnectionString))
            {
                emailAddressForUser = SharedMethods.AttemptToConvertToString(SqlDataFactory.CreateCommand(connection, query, parameters).ExecuteScalar());
            }

            return emailAddressForUser;
        }

        public static bool CheckIfRawDataReportAlreadyExists(int runID)
        {
            bool doesReportAlreadyExist = false;
            int returnedCount = 0;
            string query = "SELECT COUNT (rawDataReportID) " +
                                  "FROM execOverviewTool_RawDataReportDetails " +
                                  "WHERE runID = @runID ";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "runID", runID }
            };

            using (DbConnection connection = SqlDataFactory.CreateAndOpenConnection(servicesDBConnectionString))
            {
                returnedCount = SharedMethods.AttemptToConvertToInt(SqlDataFactory.CreateCommand(connection, query, parameters).ExecuteScalar());
            }

            if (returnedCount >= 1)
            {
                doesReportAlreadyExist = true;
                UpdateRawDataReportDetails(runID);
            }

            return doesReportAlreadyExist;
        }

        public static void InsertRawDataReportsDetails(int runID, string rawDataReportFilePath)
        {
            string insertStatement = "INSERT INTO execOverviewTool_RawDataReportDetails " +
                                   "(rawDataReportsFilePath, dateToDelete, runID) " +
                                   "VALUES (@filePath, @dateToDelete, @runID)";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "filePath", rawDataReportFilePath },
                { "dateToDelete", DateTime.Now.AddDays(1) },
                { "runID", runID }
            };

            using (DbConnection connection = SqlDataFactory.CreateAndOpenConnection(servicesDBConnectionString))
            {
                SqlDataFactory.CreateCommand(connection, insertStatement, parameters).ExecuteNonQuery();
            }
        }

        public static void UpdateRawDataReportDetails(int runID)
        {
            string updateStatement = "UPDATE execOverviewTool_RawDataReportDetails " +
                                  "SET dateToDelete = @dateToDelete " +
                                  "WHERE runID = @runID";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "dateToDelete", DateTime.Now.AddDays(1) },
                { "runID", runID }
            };

            using (DbConnection connection = SqlDataFactory.CreateAndOpenConnection(servicesDBConnectionString))
            {
                SqlDataFactory.CreateCommand(connection, updateStatement, parameters).ExecuteNonQuery();
            }
        }

        public static void SetGeneratingRawDataReportFlagToFalse(int runID)
        {
            string updateStatement = "UPDATE execOverviewTool_RunDetails " +
                      "SET isRawDataReportBeingGenerated = 0 " +
                      "WHERE runID = @runID";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "runID", runID }
            };

            using (DbConnection connection = SqlDataFactory.CreateAndOpenConnection(servicesDBConnectionString))
            {
                SqlDataFactory.CreateCommand(connection, updateStatement, parameters).ExecuteNonQuery();
            }
        }

        public static void GetRawDataAndBuildReports(int runID, string mobileAnalysisChartRawDataFilePath, string durationRelatedRawDataFilePath)
        {
            Dictionary<string, string[]> qdaAssetAndCurriculumSeriesData = GetQDAAssetsWithCurriculmAndSeriesData();

            GetMobileAnalysisChartRawDataAndBuildReport(runID, mobileAnalysisChartRawDataFilePath, qdaAssetAndCurriculumSeriesData);
            GetDurationRelatedRawDataAndBuildReport(runID, durationRelatedRawDataFilePath, qdaAssetAndCurriculumSeriesData);
        }

        private static void GetMobileAnalysisChartRawDataAndBuildReport(int runID, string rawDataFilePath, Dictionary<string, string[]> qdaAssetAndCurriculumSeriesData)
        {
            int recordCounter = 0;
            bool requiresWritingHeaders = true;
            string query = "SELECT rd.username, rd.firstName, rd.lastName, rd.assetTitle, rd.formattedAssetID, rd.originalAssetID, rd.assetType, " +
                      "rd.assetSubType, rd.formattedAssetDate, rd.sourceType, rd.deviceType " +
                      "FROM execOverviewTool_ReportData as rd " +
                      "WHERE runID = @runID";
            MobileAnalysisChartRawDataRecord rawDataRecord;
            List<MobileAnalysisChartRawDataRecord> rawDataRecords = new List<MobileAnalysisChartRawDataRecord>();
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "runID", runID }
            };

            using (DbConnection connection = SqlDataFactory.CreateAndOpenConnection(SqlDataFactory.GetConnectionString()))
            {
                DbDataReader reader = SqlDataFactory.CreateReader(SqlDataFactory.CreateCommand(connection, query, parameters));

                while (reader.Read())
                {
                    rawDataRecord = new MobileAnalysisChartRawDataRecord()
                    {
                        Username = reader["username"].ToString(),
                        FirstName = reader["firstName"].ToString(),
                        LastName = reader["lastName"].ToString(),
                        AssetTitle = reader["assetTitle"].ToString(),
                        FormattedAssetID = reader["formattedAssetID"].ToString(),
                        OriginalAssetID = reader["originalAssetID"].ToString(),
                        AssetType = reader["assetType"].ToString(),
                        AssetSubType = reader["assetSubType"].ToString(),
                        FormattedAccessDate = reader["formattedAssetDate"].ToString(),
                        SourceType = reader["sourceType"].ToString(),
                        DeviceType = reader["deviceType"].ToString()
                    };

                    if (qdaAssetAndCurriculumSeriesData.ContainsKey(rawDataRecord.FormattedAssetID))
                    {
                        rawDataRecord.Curriculum = qdaAssetAndCurriculumSeriesData[rawDataRecord.FormattedAssetID][0];
                        rawDataRecord.Series = qdaAssetAndCurriculumSeriesData[rawDataRecord.FormattedAssetID][1];
                    }
                    else if (qdaAssetAndCurriculumSeriesData.ContainsKey(rawDataRecord.OriginalAssetID))
                    {
                        rawDataRecord.Curriculum = qdaAssetAndCurriculumSeriesData[rawDataRecord.OriginalAssetID][0];
                        rawDataRecord.Series = qdaAssetAndCurriculumSeriesData[rawDataRecord.OriginalAssetID][1];
                    }
                    else if(rawDataRecord.FormattedAssetID.StartsWith("z", StringComparison.OrdinalIgnoreCase))
                    {
                        rawDataRecord.Curriculum = "Legal Compliance";
                        rawDataRecord.Curriculum = "Compliance Content";
                    }

                    rawDataRecords.Add(rawDataRecord);

                    recordCounter++;
                    if (recordCounter % 100000 == 0)
                    {
                        FileServices.BuildMobileAnalysisChartRawDataReportCsvFile(rawDataRecords, rawDataFilePath, requiresWritingHeaders);
                        requiresWritingHeaders = false;
                        rawDataRecords = new List<MobileAnalysisChartRawDataRecord>();
                    }
                }
            }

            if (rawDataRecords.Count > 0)
            {
                FileServices.BuildMobileAnalysisChartRawDataReportCsvFile(rawDataRecords, rawDataFilePath, requiresWritingHeaders);
            }
        }

        private static void GetDurationRelatedRawDataAndBuildReport(int runID, string rawDataFilePath, Dictionary<string, string[]> qdaAssetAndCurriculumSeriesData)
        {
            int recordCounter = 0;
            bool requiresWritingHeaders = true;
            string query = "SELECT rd.username, rd.originalAssetID, rd.formattedAssetID, rd.formattedAssetDate, rd.assetTitle, rd.assetType, rd.assetSubType, rd.timesAccessed, rd.expectedDuration, " +
                       "rd.actualDuration, rd.pageReads " +
                       "FROM execOverviewTool_DurationRelatedReportData as rd " +
                       "WHERE runID = @runID";
            DurationRawDataRecord rawDataRecord;
            List<DurationRawDataRecord> rawDataRecords = new List<DurationRawDataRecord>();
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "runID", runID }
            };

            using (DbConnection connection = SqlDataFactory.CreateAndOpenConnection(SqlDataFactory.GetConnectionString()))
            {
                DbDataReader reader = SqlDataFactory.CreateReader(SqlDataFactory.CreateCommand(connection, query, parameters));

                while (reader.Read())
                {
                    rawDataRecord = new DurationRawDataRecord()
                    {
                        Username = reader["username"].ToString(),
                        AssetTitle = reader["assetTitle"].ToString(),
                        OriginalAssetID = reader["originalAssetID"].ToString(),
                        FormattedAssetID = reader["formattedAssetID"].ToString(),
                        FormattedAccessDate = reader["formattedAssetDate"].ToString(),
                        AssetType = reader["assetType"].ToString(),
                        AssetSubType = reader["assetSubType"].ToString(),
                        TimesAccessed = reader["timesAccessed"].ToString(),
                        ExpectedDuration = reader["expectedDuration"].ToString(),
                        ActualDuration = reader["actualDuration"].ToString(),
                        PageReads = reader["pageReads"].ToString()
                    };

                    if (qdaAssetAndCurriculumSeriesData.ContainsKey(rawDataRecord.FormattedAssetID))
                    {
                        rawDataRecord.Curriculum = qdaAssetAndCurriculumSeriesData[rawDataRecord.FormattedAssetID][0];
                        rawDataRecord.Series = qdaAssetAndCurriculumSeriesData[rawDataRecord.FormattedAssetID][1];
                    }
                    else if (qdaAssetAndCurriculumSeriesData.ContainsKey(rawDataRecord.OriginalAssetID))
                    {
                        rawDataRecord.Curriculum = qdaAssetAndCurriculumSeriesData[rawDataRecord.OriginalAssetID][0];
                        rawDataRecord.Series = qdaAssetAndCurriculumSeriesData[rawDataRecord.OriginalAssetID][1];
                    }
                    else if (rawDataRecord.FormattedAssetID.StartsWith("z", StringComparison.OrdinalIgnoreCase))
                    {
                        rawDataRecord.Curriculum = "Legal Compliance";
                        rawDataRecord.Series = "Compliance Content";
                    }

                    rawDataRecords.Add(rawDataRecord);

                    recordCounter++;
                    if (recordCounter % 100000 == 0)
                    {
                        FileServices.BuildDurationRelatedRawDataReportCsvFile(rawDataRecords, rawDataFilePath, requiresWritingHeaders);
                        requiresWritingHeaders = false;
                        rawDataRecords = new List<DurationRawDataRecord>();
                    }
                }
            }

            if (rawDataRecords.Count > 0)
            {
                FileServices.BuildDurationRelatedRawDataReportCsvFile(rawDataRecords, rawDataFilePath, requiresWritingHeaders);
            }

        }

        private static Dictionary<string, string[]> GetQDAAssetsWithCurriculmAndSeriesData()
        {
            Dictionary<string, string[]> qdaAssetsCurriculumAndSeriesData = new Dictionary<string, string[]>();
            string query = "SELECT assetID, curriculum, series FROM qda_AssetDetails GROUP BY assetID, curriculum, series";

            using (DbConnection connection = SqlDataFactory.CreateAndOpenConnection(servicesDBConnectionString))
            {
                DbDataReader reader = SqlDataFactory.CreateReader(SqlDataFactory.CreateCommand(connection, query));

                while (reader.Read())
                {
                    if (!qdaAssetsCurriculumAndSeriesData.ContainsKey(reader["assetID"].ToString()))
                    {
                        qdaAssetsCurriculumAndSeriesData.Add(reader["assetID"].ToString(), new string[] { reader["curriculum"].ToString(), reader["series"].ToString() });
                    }
                }
            }

            return qdaAssetsCurriculumAndSeriesData;
        }
    }
}