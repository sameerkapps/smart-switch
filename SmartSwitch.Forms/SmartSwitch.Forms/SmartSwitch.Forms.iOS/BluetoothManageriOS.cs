using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

using Foundation;

using Xamarin.Forms;

using SmartSwitch.Forms.ViewModels;
using SmartSwitch.Forms.iOS;
using CoreBluetooth;
using CoreFoundation;

[assembly: Dependency(typeof(BluetoothManageriOS))]
namespace SmartSwitch.Forms.iOS
{
	/// <summary>
	/// Since iPhone does not support bluetooth SPP profile, it cannot be connected to JY-MCU
	/// This class is just a placeholder.
	/// </summary>	
	public class BluetoothManageriOS : IBluetoothManager
	{
		// list of devices
		List<DeviceInfo> _devices = new List<DeviceInfo>();

		public BluetoothManageriOS ()
		{
		}

		/// <summary>
		/// IsConnected flag
		/// </summary>
		public bool IsConnected 
		{ 
			get
			{ 
				return false;
			}
		}

		/// <summary>
		/// Returns list of paired devices
		/// </summary>
		/// <returns></returns>
		public async Task<List<DeviceInfo>> GetPairedDevices()
		{
			// iPhone does not pair with JY-MCU due to protocol limitations in iPhone
			// so left blank

			// Task.Run to avoid warning
			await Task.Run(() => _devices.Clear ());

			return  _devices;
		}

		/// <summary>
		/// iPhone cannot connect
		/// </summary>
		/// <param name="deviceHostName">HostName</param>
		/// <returns></returns>
		public async Task<bool> ConnectAsync(string deviceHostName)
		{
			return await Task.Run<bool>(() => { return false; });
		}

		/// <summary>
		/// Since there is no real connect, there is no disconnect
		/// </summary>
		public void Disconnect()
		{
		}
			
		public async Task<uint> SendOnOffAsync(bool isOn)
		{
			return await SendAsync(isOn ? CommandStrings.On : CommandStrings.Off);
		}

		/// <summary>
		/// Simulates sending data
		/// </summary>
		/// <param name="data">Data to be sent</param>
		/// <returns>Number of bytes sent</returns>
		private async Task<uint> SendAsync(string data)
		{
			// Task.Run to avoid warning
			await Task.Run (() =>
			{
			});

			if (string.IsNullOrEmpty(data))
			{
				return 0;
			}

			return (uint)data.Length;
		}
	}
}

