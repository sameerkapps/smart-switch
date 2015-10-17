// Copyright (c) 2015 Sameer Khandekar
// Provided as is with MIT License

using System;
using Xamarin.Forms.Platform.WinPhone;
using Xamarin.Forms.Xaml;
using Microsoft.Phone.Controls;

namespace SmartSwitch.Forms.WinPhone
{
    public partial class MainPage : global::Xamarin.Forms.Platform.WinPhone.FormsApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
            SupportedOrientations = SupportedPageOrientation.PortraitOrLandscape;

            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new SmartSwitch.Forms.App());
        }
    }
}
