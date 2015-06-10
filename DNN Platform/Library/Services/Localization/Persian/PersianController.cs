

using System;
using System.Globalization;
using System.Reflection;

namespace DotNetNuke.Services.Localization.Persian
{
    internal class PersianController
    {
        public static CultureInfo GetPersianCultureInfo()
        {
            var persianCultureInfo = new CultureInfo("fa-IR");

            SetPersianDateTimeFormatInfo(persianCultureInfo.DateTimeFormat);
            SetNumberFormatInfo(persianCultureInfo.NumberFormat);

            var cal = new PersianCalendar();

            FieldInfo fieldInfo = persianCultureInfo.GetType().GetField("calendar", BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo != null)
                fieldInfo.SetValue(persianCultureInfo, cal);

            FieldInfo info = persianCultureInfo.DateTimeFormat.GetType().GetField("calendar", BindingFlags.NonPublic | BindingFlags.Instance);
            if (info != null)
                info.SetValue(persianCultureInfo.DateTimeFormat, cal);

            return persianCultureInfo;
        }

        public static void SetPersianDateTimeFormatInfo(DateTimeFormatInfo persianDateTimeFormatInfo)
        {
            persianDateTimeFormatInfo.MonthNames = new[] { "\u0641\u0631\u0648\u0631\u062F\u06CC\u0646", "\u0627\u0631\u062F\u06CC\u0628\u0647\u0634\u062A", "\u062E\u0631\u062F\u0627\u062F", "\u062A\u06CC\u0631", "\u0645\u0631\u062F\u0627\u062F", "\u0634\u0647\u0631\u06CC\u0648\u0631", "\u0645\u0647\u0631", "\u0622\u0628\u0627\u0646", "\u0622\u0630\u0631", "\u062F\u06CC", "\u0628\u0647\u0645\u0646", "\u0627\u0633\u0641\u0646\u062F", "" };
            persianDateTimeFormatInfo.MonthGenitiveNames = persianDateTimeFormatInfo.MonthNames;
            persianDateTimeFormatInfo.AbbreviatedMonthNames = persianDateTimeFormatInfo.MonthNames;
            persianDateTimeFormatInfo.AbbreviatedMonthGenitiveNames = persianDateTimeFormatInfo.MonthNames;

            persianDateTimeFormatInfo.DayNames = new[] { "\u06CC\u06A9\u0634\u0646\u0628\u0647", "\u062F\u0648\u0634\u0646\u0628\u0647", "\uFEB3\uFEEA\u0634\u0646\u0628\u0647", "\u0686\u0647\u0627\u0631\u0634\u0646\u0628\u0647", "\u067E\u0646\u062C\u0634\u0646\u0628\u0647", "\u062C\u0645\u0639\u0647", "\u0634\u0646\u0628\u0647" };
            persianDateTimeFormatInfo.AbbreviatedDayNames = new[] { "\u06CC", "\u062F", "\u0633", "\u0686", "\u067E", "\u062C", "\u0634" };
            persianDateTimeFormatInfo.ShortestDayNames = persianDateTimeFormatInfo.AbbreviatedDayNames;
            persianDateTimeFormatInfo.FirstDayOfWeek = DayOfWeek.Saturday;

            persianDateTimeFormatInfo.AMDesignator = "\u0642.\u0638";
            persianDateTimeFormatInfo.PMDesignator = "\u0628.\u0638";

            persianDateTimeFormatInfo.DateSeparator = "/";
            persianDateTimeFormatInfo.TimeSeparator = ":";

            persianDateTimeFormatInfo.FullDateTimePattern = "tt hh:mm:ss yyyy mmmm dd dddd";
            persianDateTimeFormatInfo.YearMonthPattern = "yyyy, MMMM";
            persianDateTimeFormatInfo.MonthDayPattern = "dd MMMM";

            persianDateTimeFormatInfo.LongDatePattern = "dddd, dd MMMM,yyyy";
            persianDateTimeFormatInfo.ShortDatePattern = "yyyy/MM/dd";

            persianDateTimeFormatInfo.LongTimePattern = "hh:mm:ss tt";
            persianDateTimeFormatInfo.ShortTimePattern = "hh:mm tt";
        }

        public static void SetNumberFormatInfo(NumberFormatInfo persianNumberFormatInfo)
        {
            persianNumberFormatInfo.NumberDecimalSeparator = "/";
            persianNumberFormatInfo.DigitSubstitution = DigitShapes.NativeNational;
            persianNumberFormatInfo.NumberNegativePattern = 0;
        }
    }
}
