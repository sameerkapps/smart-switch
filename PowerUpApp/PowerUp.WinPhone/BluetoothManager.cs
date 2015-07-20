// Copyright (c) 2015 Sameer Khandekar
// Provided as is with MIT License

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

using Windows.Networking;
using Windows.Networking.Proximity;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

using Powerup.core;
using Powerup.core.ViewModels;

namespace Powerup.WinPhone
{
    /// <summary>
    /// This provides bluetooth functionality required for Windows Phone
    /// If an error occurs, it throws an exception. So it can be caught by View Model.
    /// </summary>
    public class BluetoothManager : IBluetoothManager
    {
        // Bluetooth socket to send message
        private StreamSocket _socket;
        // writer to wite on the socket
        private DataWriter _writer;
        // instance for singleton
        private static BluetoothManager _instance = new BluetoothManager();

        /// <summary>
        /// Private constructor for singleton pattern
        /// </summary>
        private BluetoothManager()
        {
        }

        /// <summary>
        /// Instance of the Bluetooth manager
        /// </summary>
        public static BluetoothManager Instance
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
                return (_writer != null);
            }
        }

        /// <summary>
        /// Returns list of paired devices
        /// </summary>
        /// <returns>List of paired devices</returns>
        public async Task<List<DeviceInfo>> GetPairedDevices()
        {
            List<DeviceInfo> devices = new List<DeviceInfo>();
            try
            {
                // find all the Paired devices
                PeerFinder.AlternateIdentities["Bluetooth:Paired"] = "";
                var pairedDevices = await PeerFinder.FindAllPeersAsync();

                // Covert devices to ViewModel
                foreach (var device in pairedDevices)
                {
                    devices.Add(new DeviceInfo()
                    {
                        DisplayName = device.DisplayName,
                        HostName = device.HostName.RawName
                    });
                }
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2147023729)
                {
                    throw new Exception("The Bluetooth settings is off. Please turn it on.");
                }

                throw;
            }

            return devices;
        }

        /// <summary>
        /// Connect to the device given host name
        /// </summary>
        /// <param name="deviceHostName">Raw host name of the device</param>
        /// <returns>True if connected successfully. Else False</returns>
        public async Task<bool> ConnectAsync(string deviceHostName)
        {
            // dispose of any existing socket
            if (_socket != null)
            {
                _socket.Dispose();
                _socket = null;
            }

            if (_writer != null)
            {
                _writer.Dispose();
                _writer = null;
            }

            try
            {
                // create hostname
                HostName host = new HostName(deviceHostName);

                // create new socket and attempt to connect
                _socket = new StreamSocket();

                // if connect fails, go to Bluetooth manager settings
                // connect it manually and disconnect
                // then try again
                
               await _socket.ConnectAsync(host, "1");

                // create a writer based on the socket
                _writer = new DataWriter(_socket.OutputStream);

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                throw;
            }
        }

        /// <summary>
        /// sends on/off command to the device
        /// </summary>
        /// <param name="isOn">Flag Indicating on/off</param>
        /// <returns>Number of bytes sent</returns>
        public async Task<uint> SendOnOffAsync(bool isOn)
        {
            return await SendCommandAsync(isOn ? CommandStrings.On : CommandStrings.Off);
        }

        /// <summary>
        /// Disconnects from the existing device
        /// </summary>
        public void Disconnect()
        {
            if (_writer != null)
            {
                _writer.Dispose();
                _writer = null;
            }

            if (_socket != null)
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
        private async Task<uint> SendCommandAsync(string command)
        {
            if (_writer == null)
            {
                // if writer is null, it means bluetooth manager is not connected.
                // so this should not get called
                throw new InvalidOperationException("Bluetooth manager not connected.");
            }

            uint sentCommandSize = 0;

            try
            {
                // send the size of the command
                uint commandSize = _writer.MeasureString(command);
                _writer.WriteByte((byte)commandSize);

                // now write the actual command
                sentCommandSize = _writer.WriteString(command);

                // this ensures that command is actually sent
                await _writer.FlushAsync();
                await _writer.StoreAsync();

                return sentCommandSize;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
