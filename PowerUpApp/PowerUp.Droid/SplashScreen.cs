// Copyright (c) 2015 Sameer Khandekar
// Provided as is with MIT License
using System;

using Android.App;
using Android.Content.PM;

using Cirrious.MvvmCross.Droid.Views;

namespace PowerUp.Droid
{
    [Activity(
		Label = "Smart Switch"
		, MainLauncher = true
		, Icon = "@drawable/icon"
		, Theme = "@style/Theme.Splash"
		, NoHistory = true
		, ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : MvxSplashScreenActivity
    {
        public SplashScreen()
            : base(Resource.Layout.SplashScreen)
        {
        }
    }
}