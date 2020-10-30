using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EORTBuildReportDataFilesForDownload.Services
{
    public class SharedMethods
    {
        public static string AttemptToConvertToString(object value)
        {
            string convertedValue = string.Empty;

            if (!CheckObjectForNull(value))
            {
                convertedValue = value.ToString();
            }

            return convertedValue;
        }

        public static Guid? AttemptToConvertToNullableGuid(object value)
        {
            Guid? convertedValue = null;

            if (!CheckObjectForNull(value))
            {
                convertedValue = Guid.Parse(value.ToString());
            }

            return convertedValue;
        }

        public static bool AttemptToConvertToBool(object value)
        {
            bool convertedValue = false;

            if (!CheckObjectForNull(value))
            {
                convertedValue = Convert.ToBoolean(value);
            }

            return convertedValue;
        }


        public static int AttemptToConvertToInt(object value)
        {
            int convertedValue = 0;

            if (!CheckObjectForNull(value))
            {
                convertedValue = Convert.ToInt32(value);
            }

            return convertedValue;
        }

        public static int? AttemptToConvertToNullableInt(object value)
        {
            int? convertedValue = null;

            if (!CheckObjectForNull(value))
            {
                convertedValue = Convert.ToInt32(value);
            }

            return convertedValue;
        }

        public static long AttemptToConvertToLong(object value)
        {
            long convertedValue = 0;

            if (!CheckObjectForNull(value))
            {
                convertedValue = Convert.ToInt64(value);
            }

            return convertedValue;
        }

        public static double AttemptToConvertToDouble(object value)
        {
            double convertedValue = 0.0;

            if (!CheckObjectForNull(value))
            {
                convertedValue = Convert.ToDouble(value);
            }

            return convertedValue;
        }

        public static DateTime AttemptToConvertToDateTime(object value)
        {
            DateTime convertedValue = new DateTime();

            if (!CheckObjectForNull(value))
            {
                convertedValue = Convert.ToDateTime(value);
            }

            return convertedValue;
        }

        public static DateTime? AttemptToConvertToNullableDateTime(object value)
        {
            DateTime? convertedValue = null;

            if (!CheckObjectForNull(value))
            {
                convertedValue = (DateTime?)value;
            }

            return convertedValue;
        }

        private static bool CheckObjectForNull(object value)
        {
            bool isNull = true;

            if (value != null && value != DBNull.Value)
            {
                isNull = false;
            }

            return isNull;
        }
    }
}
