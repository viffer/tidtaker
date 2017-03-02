using System;
using System.Globalization;

namespace UtleiraTidtaker.Lib.Utilities
{
    public static class RaceTimeParser
    {
        public static string GetTimestring(DateTime date)
        {
            // Sat Sep 12 2015 11:15:00 GMT+0100 (CET)
            return string.Format(CultureInfo.InvariantCulture,
                "{0:ddd} {0:MMM} {0:dd} {0:yyyy} {0:HH:mm:ss} GMT+0100 (CET)",
                date);
        }

        public static string GetRaceupdateTimestring(DateTime date)
        {
            // Sat Jun 18 2016 13:44:36 GMT 0200 (CEST)
            return string.Format(CultureInfo.InvariantCulture,
                "{0:ddd} {0:MMM} {0:dd} {0:yyyy} {0:HH:mm:ss} GMT 0200 (CEST)",
                date);
        }

        public static string GetTimestringOld(DateTime date)
        {
            // Sat Sep 12 2015 11:15:00 GMTpluss0100 (CET)
            return string.Format(CultureInfo.InvariantCulture,
                "{0:ddd} {0:MMM} {0:dd} {0:yyyy} {0:HH:mm:ss} GMTpluss0100 (CET)",
                date);
        }
    }
}
