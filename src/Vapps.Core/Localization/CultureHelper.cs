using Abp.Localization;
using System.Collections.Generic;
using System.Globalization;

namespace Vapps.Localization
{
    public static class CultureHelper
    {
        public static CultureInfo[] AllCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);

        public static bool UsingLunarCalendar = CultureInfo.CurrentUICulture.DateTimeFormat.Calendar.AlgorithmType == CalendarAlgorithmType.LunarCalendar;


        public static bool IsRtl
        {
            get { return CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft; }
        }

        public static CultureInfo GetCultureInfoByChecking(string name)
        {
            try
            {
                return CultureInfo.GetCultureInfo(name);
            }
            catch (CultureNotFoundException)
            {
                return CultureInfo.CurrentCulture;
            }
        }
    }
}
