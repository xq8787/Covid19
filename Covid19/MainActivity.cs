using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Covid19.Model;
using System;
using System.Collections.Generic;
using Android.Content;
using Android.Views;


namespace Covid19
{
    [Activity(Label = "@string/app_name")]
    public class MainActivity : AppCompatActivity
    {
        Button btnworld, btncountry; 
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            btnworld = FindViewById<Button>(Resource.Id.btnworld);
            btnworld.Click += (object sender, EventArgs e) =>
            {
                BtnWorld_Click(sender, e);
            };

            btncountry = FindViewById<Button>(Resource.Id.btncountry);
            btncountry.Click += (object sender, EventArgs e) =>
            {
                BtnCountry_Click(sender, e);
            };
        }

        private void BtnCountry_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(SelectCountry));
            Finish();
        }

        private void BtnWorld_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(WorldActivity));
            Finish();
        }
    }
}