// Copyright (c) 2015 Sameer Khandekar
// Provided as is with MIT License

#if SimulateBluetooth

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

using Powerup.core.ViewModels;
using Powerup.core;

namespace Powerup.Common
{
    /// <summary>
    /// This is for simulating bluetooth functionality
    /// It can be used for testing on simulator.
    /// </summary>
    public class SimulateBluetooth : IBluetoothManager
    {
        /// <summary>
        /// Instance for singleton pattern
        /// </summary>
        private static SimulateBluetooth _instance = new SimulateBluetooth();

        /// <summary>
        /// Constructor made private for singleton
        /// </summary>
        private SimulateBluetooth()
        {
        }

        /// <summary>
        /// Singleton instance
        /// </summary>
        public static SimulateBluetooth Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// IsConnected flag
        /// </summary>
        public bool IsConnected { get; private set; }

        /// <summary>
        /// Returns list of paired devices
        /// </summary>
        /// <returns></returns>
        public async Task<List<DeviceInfo>> GetPairedDevices()
        {
            List<DeviceInfo> devices = new List<DeviceInfo>();
            devices.Add(new DeviceInfo() { DisplayName = "1", HostName = "11" });
            devices.Add(new DeviceInfo() { DisplayName = "2", HostName = "12" });

            return devices;
        }

        /// <summary>
        /// If deviceHostName is 12, simlates as connected
        /// </summary>
        /// <param name="deviceHostName">HostName</param>
        /// <returns></returns>
        public async Task<bool> ConnectAsync(string deviceHostName)
        {
            IsConnected = deviceHostName == "12";
            return IsConnected;
        }

        /// <summary>
        /// sets connected to false
        /// </summary>
        public void Disconnect()
        {
            IsConnected = false;
        }

        /// <summary>
        /// Simulates sending data
        /// </summary>
        /// <param name="data">Data to be sent</param>
        /// <returns>Number of bytes sent</returns>
        public async Task<uint> SendAsync(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return 0;
            }

            return (uint)data.Length;
        }

        public async Task<uint> SendOnOffAsync(bool isOn)
        {
            return await SendAsync(isOn ? MessageValues.On : MessageValues.Off);
        }
    }
}
#endif