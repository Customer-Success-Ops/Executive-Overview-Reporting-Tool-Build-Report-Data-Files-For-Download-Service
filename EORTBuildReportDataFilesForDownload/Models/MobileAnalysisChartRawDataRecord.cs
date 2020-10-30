using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EORTBuildReportDataFilesForDownload.Models
{
    public class MobileAnalysisChartRawDataRecord : RawDataRecord
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SourceType { get; set; }
        public string DeviceType { get; set; }
    }
}
