using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Covid19
{
    [Activity(Label = "@string/app_name")]
    public class SelectCountry : Activity
    {
        private List<KeyValuePair<string, string>> countries;
        private Button btnSearch;
        private Spinner spSelectCountry;
        string countryID,countryName;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.layout_selectcountry);
            populateList();
            btnSearch = FindViewById<Button>(Resource.Id.btnsearch);
            btnSearch.Click += BtnSearch_Click;
            spSelectCountry = FindViewById<Spinner>(Resource.Id.spSelectCountry);
            List<string> countryNames = new List<string>();
            foreach (var item in countries)
                countryNames.Add(item.Key);
            var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, countryNames);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spSelectCountry.Adapter = adapter;
            spSelectCountry.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            
        }
        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            countryID = countries[e.Position].Value;
            countryName = string.Format("{0}",spinner.GetItemAtPosition(e.Position));
            Toast.MakeText(this, countryName, ToastLength.Long).Show();
        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(CountryActivty));
            intent.PutExtra("CountryID", countryID);
            intent.PutExtra("CountryName", countryName);
            StartActivity(intent);
            Finish();
        }

        private void populateList()
        {
            countries = new List<KeyValuePair<string, string>>();
            Stream myStream = Assets.Open("country_list.txt");
            using (StreamReader sr = new StreamReader(myStream))
            {

                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    String[] data = line.Split(',');
                    countries.Add(new KeyValuePair<string, string>(data[0], data[1]));
                    //txthintqes.Text = data[1].ToString();
                }
            }
            myStream.Close();
        }
    }
}