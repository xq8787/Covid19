using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Covid19.Model;
using Newtonsoft.Json;

namespace Covid19
{
    [Activity(Label = "@string/app_name")]
    public class CountryActivty : Activity
    {
        ListView mainList;
        CountryStatus cdata = new CountryStatus();
        string country_name,country_id;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            country_id = Intent.GetStringExtra("CountryID");
            country_name = Intent.GetStringExtra("CountryName");
            SetContentView(Resource.Layout.layout_country);
            mainList = (ListView)FindViewById<ListView>(Resource.Id.countrylistview);
            TextView txtdiscountry = (TextView)FindViewById<TextView>(Resource.Id.txtdiscountry);
            txtdiscountry.Text = country_name;
            loadData();
        }

        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Back)
            {
                var intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
                Finish();
                return true;
            }

            return base.OnKeyDown(keyCode, e);
        }


        private void loadData()
        {
            new GetData(this, cdata).Execute(Common.Common.APIRequest(country_id));
            //new GetData(this, covid).Execute();
        }

        public class CountryList
        {

            public string country { get; set; }

            public string date { get; set; }

            public string cases { get; set; }

            public string status { get; set; }

        }
        private class GetData : AsyncTask<string, Java.Lang.Void, string>
        {
            private CountryActivty activity;
            CountryStatus cdata;
            List<CountryList> conlist = new List<CountryList>();
            public GetData(CountryActivty activity, CountryStatus obj)
            {
                this.activity = activity;
                this.cdata = obj;
            }

            protected override void OnPreExecute()
            {
                base.OnPreExecute();
            }
            protected override string RunInBackground(params string[] @params)
            {
                string stream = null;
                string urlString = @params[0];
                Helper.Helper http = new Helper.Helper();
                stream = http.GetHTTPData(urlString);
                return stream;
            }
            protected override void OnPostExecute(string result)
            {
                //var dataobj = new CountryStatus();
                //List<string> datelist = new List<string>();
                //List<string> caseslist = new List<string>();
                try
                {
                    base.OnPostExecute(result);

                    if (result.Contains("Error:"))
                    {
                        return;
                    }
                    var parsed = JsonConvert.DeserializeObject<List<CountryStatus>>(result);
                    string cas = parsed[0].Country;
                    for (int i = 0; i < parsed.Count-1; i++)
                    {
                        CountryList obj = new CountryList();
                        obj.country = parsed[i].Country;
                        string [] strarr = parsed[i].Date.ToString().Split(" ",2);
                        obj.date = strarr[0];
                        obj.cases = parsed[i].Cases.ToString();
                        obj.status = parsed[i].Status.ToString();
                        conlist.Add(obj);
                  
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Exception : " + ex.Message);
                    return;
                }
                CountryAdapter adapter = new CountryAdapter(activity, conlist);
                activity.mainList.Adapter = adapter;
                activity.mainList.ItemClick += listView_ItemClick;
            }

            private void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
            {
                var select = conlist[e.Position].country;
                //var intent = new Intent(activity, typeof(WebNews));
                //intent.PutExtra("URL", select);
                //intent.PutExtra("Category", activity.cat);
                //activity.StartActivity(intent);
                //activity.Finish();
                Android.Widget.Toast.MakeText(activity, select, Android.Widget.ToastLength.Long).Show();
            }
        }

        public class CountryAdapter : BaseAdapter<CountryList>
        {
            public List<CountryList> sList;
            private Context sContext;
            public CountryAdapter(Context context, List<CountryList> list)
            {
                sList = list;
                sContext = context;
            }
            public override CountryList this[int position]
            {
                get
                {
                    return sList[position];
                }
            }
            public override int Count
            {
                get
                {
                    return sList.Count;
                }
            }

            public override long GetItemId(int position)
            {
                return position;
            }

            public override View GetView(int position, View convertView, ViewGroup parent)
            {
                View row = convertView;
                try
                {
                    if (row == null)
                    {
                        row = LayoutInflater.From(sContext).Inflate(Resource.Layout.country_grid, null, false);
                    }

                    TextView txtdate = row.FindViewById<TextView>(Resource.Id.txtcdate);
                    TextView txttotal = row.FindViewById<TextView>(Resource.Id.txtccase);

                    txtdate.Text = sList[position].date;
                    txttotal.Text = sList[position].cases;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
                finally { }
                return row;
            }
        }
    }
}