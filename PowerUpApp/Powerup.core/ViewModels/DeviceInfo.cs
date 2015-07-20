// Copyright (c) 2015 Sameer Khandekar
// Provided as is with MIT License

using System;

namespace Powerup.core.ViewModels
{
    /// <summary>
    /// This view model is for individual device that is
    /// displayed as item in the list box.
    /// </summary>
    public class DeviceInfo
    {
        /// <summary>
        /// Display name of the device
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Raw host address
        /// </summary>
        public string HostName { get; set; }

		public override string ToString ()
		{
			return DisplayName;
		}
    }
}
