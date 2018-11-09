using System.Configuration;
using System.Globalization;
using TotalBase.Enums;

namespace TotalPortal.Configuration
{
    public static class Settings
    {
        public static string BaseServiceUrl
        {
            get { return ConfigurationManager.AppSettings["BaseServiceUrl"]; }
        }

        public static int MinLenght2 = 2;
        public static int AutoCompleteMinLenght = 2;
        public static int AutoCompleteMinLenght03 = 3;

        public static string DateFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
        public static string TimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
        public static string DateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern + " " + CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
        public static string YearMonthPattern = CultureInfo.CurrentCulture.DateTimeFormat.YearMonthPattern;


        public static string NumberFormat = "{0:n0}"; //MUST FIND AND REPLACE BY CORRECT FORMAT FOR QUANTITY/ AMOUNT/ WEIGHT/ ... ==> MUST HAVE DEIFFENT TEMPLATE FOR QUANTITY/ AMOUNT/ WEIGHT/ .... INSTEAD OF ONLY ONE SIGLE DECIMAL TEMPLATE FOR ALL QUANTITY/ AMOUNT/ WEIGHT/

        public static string kfmN0 { get { return "{0:n" + GlobalEnums.rndN0.ToString("N0") + "}"; } }
        public static string kfmQuantity { get { return "{0:n" + GlobalEnums.rndQuantity.ToString("N0") + "}"; } }
        public static string kfmAmount { get { return "{0:n" + GlobalEnums.rndAmount.ToString("N0") + "}"; } }
        public static string kfmDiscountPercent { get { return "{0:n" + GlobalEnums.rndDiscountPercent.ToString("N0") + "}%"; } }

        public static string kfmWeight { get { return "{0:n" + GlobalEnums.rndWeight.ToString("N0") + "}"; } }



        public static int GridPopupHeight = 263;
        public static int GridPopupNoTabHeight = 330;

        public static int PopupHeight = 482;
        public static int PopupHeightSmall = 399;
        public static int PopupHeightWithTab = 518;
        public static int PopupHeightLarge = 518;
        public static int PopupHeightVoid = 269;

        public static int PopupWidth = 1068;
        public static int PopupWidthLarge = 1118;
        public static int PopupWidthMedium = 900;
        public static int PopupWidthSmall = 900;
        public static int PopupWidthVerySmall = 600;                
        public static int PopupWidthVoid = 600;

        public static int PopupContentHeight = 360;
        public static int PopupContentHeightSmall = 281;
        public static int PopupContentHeightLarge = 397;

        public static string MonthDayPattern
        {
            get
            {
                string shortDatePattern = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
                while (shortDatePattern[0] != 'd' && shortDatePattern[0] != 'M')
                {
                    shortDatePattern = shortDatePattern.Substring(1);
                    if (shortDatePattern.Length == 0)
                        return CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
                }
                while (shortDatePattern[shortDatePattern.Length - 1] != 'd' && shortDatePattern[shortDatePattern.Length - 1] != 'M')
                {
                    shortDatePattern = shortDatePattern.Substring(0, shortDatePattern.Length - 1);
                    if (shortDatePattern.Length == 0)
                        return CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
                }
                return shortDatePattern;
            }
        }
    
    }

    public class MySettings
    {
        public string BaseServiceUrl { get { return Settings.BaseServiceUrl; } }

        public int AutoCompleteMinLenght { get { return Settings.AutoCompleteMinLenght; } }
        public string DateFormat { get { return Settings.DateFormat; } }
        public string TimeFormat { get { return Settings.TimeFormat; } }
        public string DateTimeFormat { get { return Settings.DateTimeFormat; } }
        public string NumberFormat { get { return Settings.NumberFormat; } }

        public string kfmN0 { get { return Settings.kfmN0; } }
        public string kfmQuantity { get { return Settings.kfmQuantity; } }
        public string kfmAmount { get { return Settings.kfmAmount; } }
        public string kfmDiscountPercent { get { return Settings.kfmDiscountPercent; } }
        public string kfmWeight { get { return Settings.kfmWeight; } }

        public string YearMonthPattern { get { return Settings.YearMonthPattern; } }
        public string MonthDayPattern { get { return Settings.MonthDayPattern; } }
        public int GridPopupHeight { get { return Settings.GridPopupHeight; } }
        public int GridPopupNoTabHeight { get { return Settings.GridPopupNoTabHeight; } }
    }

}