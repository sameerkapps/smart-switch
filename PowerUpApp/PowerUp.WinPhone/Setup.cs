// Copyright (c) 2015 Sameer Khandekar
// Provided as is with MIT License
using System;

using Microsoft.Phone.Controls;

using Cirrious.CrossCore;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.WindowsPhone.Platform;

using Powerup.core.ViewModels;
using Powerup.WinPhone;

namespace PowerUp.WinPhone
{
    /// <summary>
    /// Class created by MvvmCross for setup
    /// </summary>
    public class Setup : MvxPhoneSetup
    {
        public Setup(PhoneApplicationFrame rootFrame) : base(rootFrame)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            // Initialize and register IOC provider
            if (MvxSingleton<IMvxIoCProvider>.Instance == null)
            {
                var iocProvider = MvxSimpleIoCContainer.Initialize();
                Mvx.RegisterSingleton(iocProvider);
            }

            // Register Bluetooth manager
            // You can use simulated Bluetooth manager, if testing on simulator
#if SimulateBluetooth
            Mvx.RegisterSingleton<IBluetoothManager>(SimulateBluetooth.Instance);
#else
            Mvx.RegisterSingleton<IBluetoothManager>(BluetoothManager.Instance);
#endif
            return new Powerup.core.App();
        }
		
        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }
    }
}