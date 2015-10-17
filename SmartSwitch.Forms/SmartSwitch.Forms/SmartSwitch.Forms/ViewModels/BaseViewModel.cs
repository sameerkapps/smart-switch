// Copyright (c) 2015 Sameer Khandekar
// Provided as is with MIT License

using System;
using System.ComponentModel;
using System.Diagnostics;

namespace SmartSwitch.Forms.ViewModels
{
    /// <summary>
    /// Base view model class that implemens INotifyProertyChanged
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Event for property notification
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises property changed event as applicable
        /// </summary>
        /// <param name="propName">Name of the property</param>
        protected void RaisePropertyChanged(string propName)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(propName), "Property name is invalid.");

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
