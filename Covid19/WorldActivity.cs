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
using Covid19.Model;
using Newtonsoft.Json;

namespace Covid19
{
    [Activity(Label = "@string/app_name")]
    public class WorldActivity : Activity
    {
        ListView mainList;
        Covid covid = new Covid();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.world);
            mainList = (ListView)FindViewById<ListView>(Resource.Id.mainlistview);
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
            new GetData(this, covid).Execute(Common.Common.APIRequest(""));
            //new GetData(this, covid).Execute();
        }

        private class GetData : AsyncTask<string, Java.Lang.Void, string>
        {
            private WorldActivity activity;
            Covid covid;
            List<Content> conlist = new List<Content>();
            public GetData(WorldActivity activity, Covid obj)
            {
                this.activity = activity;
                this.covid = obj;
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


                try
                {
                    base.OnPostExecute(result);

                    if (result.Contains("Error:"))
                    {
                        return;
                    }
                    covid = JsonConvert.DeserializeObject<Covid>(result);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Exception : " + ex.Message);
                    return;
                }
                //Add Data
                foreach (var item in covid.Countries)
                {
                    Content obj = new Content();
                    obj.country = item.country.ToString();
                    obj.totalconfirmed = item.TotalConfirmed.ToString();
                    obj.newconfirmed = item.NewConfirmed.ToString();
                    obj.totaldeaths = item.TotalDeaths.ToString();
                    obj.newdeaths = item.NewDeaths.ToString();
                    obj.totalrecovered = item.TotalRecovered.ToString();
                    obj.newrecovered = item.NewRecovered.ToString();
                    conlist.Add(obj);
                }
                DataAdapter adapter = new DataAdapter(activity, conlist);
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

        public class Content
        {

            public string country { get; set; }

            public string totalconfirmed { get; set; }

            public string newconfirmed { get; set; }

            public string totaldeaths { get; set; }
            public string newdeaths { get; set; }
            public string totalrecovered { get; set; }
            public string newrecovered { get; set; }


        }
        public class DataAdapter : BaseAdapter<Content>
        {
            public List<Content> sList;
            private Context sContext;
            public DataAdapter(Context context, List<Content> list)
            {
                sList = list;
                sContext = context;
            }
            public override Content this[int position]
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
                        row = LayoutInflater.From(sContext).Inflate(Resource.Layout.grid_layout, null, false);
                    }

                    TextView txtcountry = row.FindViewById<TextView>(Resource.Id.txtcountry);
                    TextView txttotal = row.FindViewById<TextView>(Resource.Id.txtotal);
                    TextView txtnewcases = row.FindViewById<TextView>(Resource.Id.txtnewcase);
                    TextView txttotdeth = row.FindViewById<TextView>(Resource.Id.txttotdeth);
                    TextView txtnewdeth = row.FindViewById<TextView>(Resource.Id.txtnewdeth);
                    TextView txtrecovered = row.FindViewById<TextView>(Resource.Id.txtrecovered);
                    TextView txtnewrecoverd = row.FindViewById<TextView>(Resource.Id.txtnewrecoverd);

                    if (!sList[position].totalconfirmed.Equals("0"))
                    {
                        txtcountry.Text = sList[position].country.Trim();
                        txttotal.Text = sList[position].totalconfirmed;
                        txtnewcases.Text = sList[position].newconfirmed;
                        txttotdeth.Text = sList[position].totaldeaths;
                        txtnewdeth.Text = sList[position].newdeaths;
                        txtrecovered.Text = sList[position].totalrecovered;
                        txtnewrecoverd.Text = sList[position].newrecovered;
                    }

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