using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EORTBuildReportDataFilesForDownload.Models
{
    public class RawDataRecord
    {
        public string Username { get; set; }
        public string AssetTitle { get; set; }
        public string OriginalAssetID { get; set; }
        public string FormattedAssetID { get; set; }
        public string AssetType { get; set; }
        public string AssetSubType { get; set; }
        public string FormattedAccessDate { get; set; }
        public string Curriculum { get; set; }
        public string Series { get; set; }

    }
}
