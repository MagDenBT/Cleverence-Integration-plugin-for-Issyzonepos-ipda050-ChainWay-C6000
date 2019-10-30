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
using Cleverence.Barcoding.Remote;

namespace Cleverence.Barcoding.Integration.Plugin
{
    [IntentFilter(new string[] { "com.cleverence.intent.barcodeDevicePlugin" })]
    [IntentFilter(new string[] { "com.cleverence.intent.barcodeSdkDevicePlugin" })]
    [Service(Exported = true, Permission = "com.cleverence.PluginPermission", Name = "cleverence.com.plugin.ipda050barcodescanner")]
    public class CustomService : RemoteBarcodeDeviceService
    {
        protected override RemoteBarcodeDeviceBase CreateRemoteBarcodeDevice()
        {
            return new CustomMobileComputer();
        }

        protected override RemoteBarcodeScannerBase CreateRemoteBarcodeScanner()
        {
            return new CustomScanner(this);
        }
    }
}