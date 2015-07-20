// Copyright (c) 2015 Sameer Khandekar
// Provided as is with MIT License

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.ViewModels;

namespace Powerup.core.ViewModels
{
    /// <summary>
    /// This view model is common across all views. It provides 
    /// 1. Properties for UI elements.
    /// 2. Commands for operations
    /// 3. Workflow common across all platforms
    /// 4. Encapsulates Bluetooth manager functionlity.
    /// 5. Catches any exceptions thrown by bluetooth manager and displays as error mesage
    /// </summary>
    public class PowerupViewModel
        : MvxViewModel
    {
        // Link to my project
        private const string HacksterURL = "https://www.hackster.io/sameerk/smart-switch";
        // App requirements
        private const string ProjectDescription = "This app requires Arduino + Bluetooth + Relay Hardware. Details can be found at:";

        // Bluetooth manager
        private IBluetoothManager _btManager;

        public PowerupViewModel()
        {
            // instantiate/retrieve Bluetooth manager
           _btManager = MvxSimpleIoCContainer.Instance.Resolve<IBluetoothManager>();

           _devices = new List<DeviceInfo>();
           
           _devices.Add(SelectedDevice);

            // refresh command
            Refresh = new MvxCommand(async () =>
            {
                try
                {
                    // disconnect the Bluetooth mnager from any existing connections
                    _btManager.Disconnect();
                    ErrorMessage = string.Empty;
                    // get the paired devices and select the first one
                    Devices = await _btManager.GetPairedDevices();
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message;
                }

                if (Devices.Count > 0)
                {
                    SelectedDevice = Devices[0];
                }

                // Notify the listeners of the affected properties
                RaisePropertyChanged(() => CanConnect);
                RaisePropertyChanged(() => IsConnected);
            });

            // Connect command
            Connect = new MvxCommand(async () => await ConnectToBT(), () => CanConnect);

            // there is no command for OnOff as WindowsPhone does not support it for toggleSwitch
            // instead IsOn property is used.

            // if you wish to use plug-in and a command to launch web browser, this may be helpful
            // http://stackoverflow.com/questions/16616774/mvvmcross-how-to-navigate-to-something-besides-a-viewmodel
        }

        /// <summary>
        /// Display any error message TODO -> Rename to DisplayMessage
        /// </summary>
        private string _errorMessage;
        public string ErrorMessage 
        { 
            get
            {
                return _errorMessage;
            } 
            set
            {
                _errorMessage = value;
                RaisePropertyChanged(() => ErrorMessage);
            } 
        }

        /// <summary>
        /// List of devices
        /// </summary>
        private List<DeviceInfo> _devices;
        public List<DeviceInfo> Devices
        {
            get { return _devices; }
            set { _devices = value; RaisePropertyChanged(() => Devices); }
        }

        /// <summary>
        /// If SelectedDevice is initially null, MvvmCross does not bind it.
        /// To remediate, make it user friendly by displaying Click Refresh instead
        /// </summary>
        private DeviceInfo _selectedDevice = new DeviceInfo() { DisplayName = "Click Refresh" };
        public DeviceInfo SelectedDevice
        {
            get { return _selectedDevice; }
            set { _selectedDevice = value; Connect.RaiseCanExecuteChanged(); }
        }

        /// <summary>
        /// To enable/disable Connect button
        /// </summary>
        public bool CanConnect
        {
            get
            {                                       
                return (SelectedDevice != null) && 
                       (!string.IsNullOrEmpty(SelectedDevice.HostName)); // This prevents eable when "Click Refresh" is selected.
            }
        }

        /// <summary>
        /// Is already connected
        /// </summary>
        public bool IsConnected
        {
            get { return _btManager.IsConnected; }
        }

        /// <summary>
        /// State of the toggle switch.
        /// Also used to send command to Bluetooth manager.
        /// </summary>
        private bool _isOn;

        public bool IsOn
        {
            get { return _isOn; }
            
            set 
            {
                if (_isOn != value)
                {
                    _isOn = value;
                    // Send Async here and not in any command, as WP8 does not support command for ToggleSwitch
                    Task.Run(() => SendMessageAsync());
                }
            }
        }

        /// <summary>
        /// This represents the text on the connect button. It will change as per state.
        /// Ideally, this should be done using ConnectionState enum and a converter.
        /// If you are willing to do it, please create a pull request.
        /// </summary>
        private string _connectText = "Connect";
        public string ConnectText 
        { 
            get
            {
                return _connectText;
            }

            set
            {
                if (_connectText != value)
                {
                    _connectText = value;
                    RaisePropertyChanged(() => ConnectText);
                }
            }
        }
        /// <summary>
        /// Description of App requirements
        /// </summary>
        public string Description
        {
            get
            {
                return ProjectDescription;
            }
        }

        /// <summary>
        /// Url to the project
        /// </summary>
        public string HacksterUrl
        {
            get
            {
                return HacksterURL;
            }
        }

        /// <summary>
        /// Refresh command
        /// </summary>
        public IMvxCommand Refresh { get; private set; }

        /// <summary>
        /// Connect command
        /// </summary>
        public IMvxCommand Connect { get; set; }

        /// <summary>
        /// Performs connect operation on bluetooth.
        /// Catches any exceptions thrown.
        /// Displays result/exception to the user
        /// </summary>
        /// <returns>Task to be awaited on</returns>
        private async Task ConnectToBT()
        {
            try
            {
                ConnectText = "Connecting...";
                await _btManager.ConnectAsync(SelectedDevice.HostName);
                ErrorMessage = IsConnected ? "Connected successfully" : "Failed to connect!";
                RaisePropertyChanged(() => IsConnected);
            }
            catch (Exception ex)
            {
                // catch any exception and display it
                ErrorMessage = ex.Message;
            }
            finally
            {
                ConnectText = "Connect";
            }
        }

        /// <summary>
        /// Sends message to the bluetooth device.
        /// Displays the number of bytes sent/erroor
        /// </summary>
        /// <returns>Tasks that can be awaited</returns>
        private async Task SendMessageAsync()
        {
            try
            {
                var count = await _btManager.SendOnOffAsync(_isOn);
                ErrorMessage = "Sent " + count + " bytes.";
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}
