using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EORTBuildReportDataFilesForDownload.Models
{
    public class DurationRawDataRecord : RawDataRecord
    {
        public string TimesAccessed { get; set; }
        public string ExpectedDuration { get; set; }
        public string ActualDuration { get; set; }
        public string PageReads { get; set; }
    }
}
