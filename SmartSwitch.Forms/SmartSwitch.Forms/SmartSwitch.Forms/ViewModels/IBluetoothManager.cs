// Copyright (c) 2015 Sameer Khandekar
// Provided as is with MIT License

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartSwitch.Forms.ViewModels
{
    /// <summary>
    /// This encapsulates the functionality that needs to be supported
    /// by bluetooth manager on each platform.If an exception occurs, it should be 
    /// thrown with appropriate message. So view model will catch it and dispaly error message to 
    /// the user.
    /// </summary>
    public interface IBluetoothManager
    {
        /// <summary>
        /// True when Bluetooth manager is connected to device
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Obtains the list of Paired devices on the phone.
        /// </summary>
        /// <returns>List of Paired Devices</returns>
        Task<List<DeviceInfo>> GetPairedDevices();

        /// <summary>
        /// Connects to ta device given device host name. 
        /// </summary>
        /// <param name="deviceHostName">This is the hostName</param>
        /// <returns>Success or failure</returns>
        Task<bool> ConnectAsync(string deviceHostName);

        /// <summary>
        /// Sends on/off message to the device
        /// </summary>
        /// <param name="isOn">Flag indicating on/off</param>
        /// <returns>Number of bytes sent.</returns>
        Task<uint> SendOnOffAsync(bool isOn);

        /// <summary>
        /// Disconnect from the bluetooth
        /// </summary>
        void Disconnect();
    }
}
