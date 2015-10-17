// APACHE 2.0 License - https://github.com/XLabs/Xamarin-Forms-Labs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Text.Util;
using SmartSwitch.Forms.Controls;
using SmartSwitch.Forms.Droid.Renderers;

[assembly: ExportRenderer(typeof(HyperLinkLabel), typeof(HyperLinkLabelRenderer))]

namespace SmartSwitch.Forms.Droid.Renderers
{
    /// <summary>
    /// Class HyperLinkLabelRenderer.
    /// </summary>
    public class HyperLinkLabelRenderer : LabelRenderer
    {
        /// <summary>
        /// Called when [element changed].
        /// </summary>
        /// <param name="e">The e.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {

                var nativeEditText = (global::Android.Widget.TextView)Control;

                Linkify.AddLinks(nativeEditText, MatchOptions.All);
            }
        }
    }
}