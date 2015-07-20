// Copyright (c) 2015 Sameer Khandekar
// Provided as is with MIT License

using System;
using System.Windows;
using Windows.System;

using Cirrious.MvvmCross.WindowsPhone.Views;

using Powerup.core.ViewModels;

namespace Powerup.WinPhone.Views
{
    /// <summary>
    /// Code behind for Powerup view
    /// </summary>
    public partial class PowerupView : MvxPhonePage
    {
        public PowerupView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// When hackster link is clicked
        /// Launch the url provided by view model.
        /// Alternative could be to use WebBrowser plug-in for MvvmCross
        /// https://github.com/slodge/MvvmCross/tree/v3/Plugins/Cirrious/WebBrowser
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e"></param>
        private async void HacksterLink_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (PowerupViewModel)DataContext;
            
            await Launcher.LaunchUriAsync(new Uri(viewModel.HacksterUrl, UriKind.Absolute));
        }
    }
}