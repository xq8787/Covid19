using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Covid19.Common
{
    class Common
    {
        //public static string API_KEY = "66f74b1d150e4e6e8057b45086065096";
        public static string API_LINK = "https://api.covid19api.com/summary";//
        public static string API_LINK1 = "https://api.covid19api.com/country/"; 
        //public static string API_LINK2 = "/status/confirmed/live";
        public static string APIRequest(string category)
        {
            StringBuilder sb;
            if (category.Equals(""))
            {
                sb = new StringBuilder(API_LINK);
            }
            else
            {
                sb = new StringBuilder(API_LINK1);
                sb.AppendFormat("{0}/status/confirmed/live", category);
            }
            
            //sb.AppendFormat("{0}&apiKey={1}", category, API_KEY);
            return sb.ToString();
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            dt = dt.AddSeconds(unixTimeStamp).ToLocalTime();
            return dt;
        }
    }
}