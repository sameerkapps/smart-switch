// Copyright (c) 2015 Sameer Khandekar
// Provided as is with MIT License
using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

using Cirrious.MvvmCross.Droid.Views;

using Powerup.core.ViewModels;

namespace PowerUp.Droid.Views
{
    [Activity(Label = "Smart Switch")]
    public class PowerupView : MvxActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Powerup);
            
            // Listen to click event to launch web browser
            var txthack = FindViewById<TextView>(Resource.Id.txtViewHack);
            // This can be also be done using MvvmCross plugin.
            // https://github.com/slodge/MvvmCross/tree/v3/Plugins/Cirrious/WebBrowser
            txthack.Click += txthack_Click;
        }

        void txthack_Click(object sender, EventArgs e)
        {
            var viewModel = (PowerupViewModel)DataContext;
            var androidUri = Android.Net.Uri.Parse(viewModel.HacksterUrl);
            var intent = new Intent(Intent.ActionView, androidUri);
            StartActivity(intent);
        }
    }
}