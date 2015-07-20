// Copyright (c) 2015 Sameer Khandekar
// Provided as is with MIT License

using System;

using Cirrious.CrossCore.IoC;

namespace Powerup.core
{
    public class App : Cirrious.MvvmCross.ViewModels.MvxApplication
    {
        /// <summary>
        /// initialization for MvvmCross
        /// </summary>
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();
				
            RegisterAppStart<ViewModels.PowerupViewModel>();
        }
    }
}