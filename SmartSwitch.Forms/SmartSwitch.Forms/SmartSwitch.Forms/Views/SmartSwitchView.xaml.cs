// Copyright (c) 2015 Sameer Khandekar
// Provided as is with MIT License

using System;
using System.Linq;

using Xamarin.Forms;

using SmartSwitch.Forms.ViewModels;

namespace SmartSwitch.Forms.Views
{
    public partial class SmartSwitchView : ContentPage
    {
        public SmartSwitchView()
        {
            InitializeComponent();

            // Subscribe to binding context changes
            this.BindingContextChanged += SmartSwitchView_BindingContextChanged;
        }

        void SmartSwitchView_BindingContextChanged(object sender, EventArgs e)
        {
            var viewModel = (SmartSwitchViewModel)this.BindingContext;

            // trap the property change event in the view model
            viewModel.PropertyChanged += viewModel_PropertyChanged;
        }

        private void viewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // if devices has changed, update the devices
            if (e.PropertyName == "Devices")
            {
                var viewModel = (SmartSwitchViewModel)this.BindingContext;

                UpdatedDevices(viewModel);            
            }
        }

        private void UpdatedDevices(SmartSwitchViewModel viewModel)
        {
            // The items have to be added programatically as there is no BindableProperty for Picker Items
            deviceNamesPicker.Items.Clear();
            foreach (var item in viewModel.Devices.Select((item) => item.DisplayName))
	        {
		         deviceNamesPicker.Items.Add(item);
	        }

            // select the first one using BindableProperty
            viewModel.SelectedDeviceIndex = 0;
        }
    }
}
