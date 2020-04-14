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

namespace Covid19.Model
{
    //[Serializable]
    public class CountryStatus
    {
        public string Country { get; set; }
        public string Province { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public DateTime Date { get; set; }
        public int Cases { get; set; }
        public string Status { get; set; }
    }


    //public class CountryStatus
    //{
    //    public List<RootObject> Clist { get; set; }
    //}
}