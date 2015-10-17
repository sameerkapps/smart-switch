// Copyright (c) 2015 Sameer Khandekar
// Provided as is with MIT License
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android;
using Android.Bluetooth;

using Java.Util;
using SmartSwitch.Forms.ViewModels;
using Xamarin.Forms;
using SmartSwitch.Forms.Droid;

[assembly: Dependency(typeof(BluetoothLEManager))]

namespace SmartSwitch.Forms.Droid
{
    /// <summary>
    /// Bluetooth manager for Android platform
    /// Requires: 4.3 Jelly Bean - API 18
    /// </summary>
    public class BluetoothLEManager : Java.Lang.Object, IBluetoothManager
    {
        // Bluetooth adapter
        private BluetoothAdapter _adapter;

        // socket connected to the adapter
        private BluetoothSocket _socket;

        // device that has been selected
        private BluetoothDevice SelectedDevice { get; set; }

        // instance for singleton
        private static BluetoothLEManager _instance = new BluetoothLEManager();

        /// <summary>
        /// Private constructor for singleton pattern
        /// </summary>
        public BluetoothLEManager()
        {
            this._adapter = BluetoothAdapter.DefaultAdapter;
        }

        /// <summary>
        /// Instance of the Bluetooth manager
        /// </summary>
        public static BluetoothLEManager Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// If bluetooth manager is connected to device or not
        /// </summary>
        public bool IsConnected 
        {
            get
            {
                return _socket != null && _socket.IsConnected;
            }
        }

        /// <summary>
        /// Returns list of paired devices
        /// </summary>
        /// <returns>List of paired devices</returns>
        public async Task<List<DeviceInfo>> GetPairedDevices()
        {
            List<DeviceInfo> deviceInfo = new List<DeviceInfo>();
            if (!_adapter.IsEnabled)
            {
                throw new Exception("Bluetooth is off. Please turn it on from settings.");
            }

            return await Task.Run<List<DeviceInfo>>(() =>
            {
                // create the list
                deviceInfo = new List<DeviceInfo>();

                foreach (var device in _adapter.BondedDevices)
                {
                    DeviceInfo di = new DeviceInfo() { DisplayName = device.Name, HostName = device.Address };
                    deviceInfo.Add(di);
                }

                return deviceInfo;
            });
        }

        /// <summary>
        /// Connect to the device given host name
        /// </summary>
        /// <param name="deviceHostName">Raw host name of the device</param>
        /// <returns>True if connected successfully. Else False</returns>
        public async Task<bool> ConnectAsync(string deviceHostName)
        {
            if (_socket != null)
            {
                _socket.Dispose();
                _socket = null;
            }

            if (!_adapter.IsEnabled)
            {
                throw new Exception("Bluetooth is off. Please turn it on from settings.");
            }

            SelectedDevice = _adapter.BondedDevices.FirstOrDefault(d => d.Address == deviceHostName);
           
            if (SelectedDevice != null)
            {
                var uuids = SelectedDevice.GetUuids();
                if (uuids != null && uuids.Length > 0)
                {
                    return await Task.Run<bool>(() => 
                        {
                            _socket = SelectedDevice.CreateRfcommSocketToServiceRecord(UUID.FromString(uuids[0].ToString()));
                            _socket.Connect();
                            return IsConnected;
                        });
                }
            }

            return IsConnected;
        }

        /// <summary>
        /// sends on/off command to the device
        /// </summary>
        /// <param name="isOn">Flag Indicating on/off</param>
        /// <returns>Number of bytes sent</returns>
        public Task<uint> SendOnOffAsync(bool isOn)
        {
            return SendAsync(isOn ? CommandStrings.On : CommandStrings.Off);
        }

        /// <summary>
        /// Disconnects from the existing device
        /// </summary>
        public void Disconnect()
        {
            if (SelectedDevice != null)
            {
                SelectedDevice = null;
            }

            if(_socket != null)
            {
                _socket.Dispose();
                _socket = null;
            }
        }

        /// <summary>
        /// This sends given data to the bluetooth device
        /// </summary>
        /// <param name="command">Command</param>
        /// <returns>Numbe of bytes sent</returns>
        private async Task<uint> SendAsync(string data)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            var count = (uint)bytes.Count();
            _socket.OutputStream.WriteByte((byte)count);
            await _socket.OutputStream.WriteAsync(bytes, 0, bytes.Count());
            await _socket.OutputStream.FlushAsync();

            return count;
        }
    }
}

