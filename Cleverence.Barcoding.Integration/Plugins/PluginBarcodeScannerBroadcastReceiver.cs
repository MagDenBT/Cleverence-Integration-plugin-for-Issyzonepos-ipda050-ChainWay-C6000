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
using Cleverence.Barcoding;

namespace Cleverence.Warehouse.Compact.Plugins
{
    public class PluginBarcodeScannerBroadcastReceiver : BroadcastReceiver
    {
        private PluginBarcodeScanner scanner;

        public PluginBarcodeScannerBroadcastReceiver(PluginBarcodeScanner scanner)
        {
            this.scanner = scanner;
        }

        public override void OnReceive(Context context, Intent intent)
        {
            string action = intent.Action;

            if (action == "com.cleverence.plugins.barcodeScannerScan")
            {
                if (scanner != null)
                {

                    var barcode = intent.GetStringExtra("ScannedBarcode");

                    //var type = (BarcodeType)intent.GetIntExtra("ScannedBarcodeType", (int)BarcodeType.UNKNOWN_TYPE);

                    scanner.OnScannedCallback(barcode, BarcodeType.UNKNOWN_TYPE);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            scanner = null;

            base.Dispose(disposing);
        }
    }
}