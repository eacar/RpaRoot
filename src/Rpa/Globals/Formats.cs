using System.Globalization;

namespace Rpa.Globals
{
    public static class Formats
    {
        public static string DateTime1 => "yyyy-MM-dd HH:mm:ss";
        public static string DateTime2 => "yyyy-MM-dd";
        public static string DateTime3 => "yyyy-MM-dd HH:mm";
        public static string DateTime4 => "yyyy-MM-ddTHH:mm:ss.fffZ";
        public static string DateTime5 => "yyyyMMdd";
        public static string DateTime6 => "MM/dd/yyyy HH:mm";
        public static string DateTime7 => "MM/dd/yyyy";
        public static string DateTime8 => "MMM";
        public static string DateTime9 => "dd/MM/yyyy";
        public static string DateTime10 => "yyyy-MMM-dd HH:mm:ss";
        public static string Double1 => "#.##";
        public static string Money1 => "C0";
        public static CultureInfo CultureInfoEn => CultureInfo.GetCultureInfo("en-US");
        public static CultureInfo CultureInfoTr => CultureInfo.GetCultureInfo("tr-TR");
        public static string DefaultTimeZone => "GMT Standard Time";
        public static string TurkishTimeZone => "Turkey Standard Time";
    }
}