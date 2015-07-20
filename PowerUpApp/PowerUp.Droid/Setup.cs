// Copyright (c) 2015 Sameer Khandekar
// Provided as is with MIT License
using System;

using Android.Content;

using Cirrious.CrossCore;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.ViewModels;

using Powerup.core.ViewModels;
using Powerup.Droid;

#if SimulateBluetooth
using Powerup.Common;
#endif

namespace PowerUp.Droid
{
    /// <summary>
    /// Class created by MvvmCross for setup
    /// </summary>
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
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
            Mvx.RegisterSingleton<IBluetoothManager>(BluetoothLEManager.Instance);
#endif
            return new Powerup.core.App();
        }
		
        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }
    }
}