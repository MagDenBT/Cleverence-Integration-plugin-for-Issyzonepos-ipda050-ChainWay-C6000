using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Cleverence.Warehouse.Compact.Plugins;
using System.Threading;

namespace Cleverence.Barcoding.Integration
{
    [Activity(Label = "Cleverence Integration Test", MainLauncher = true, Icon = "@drawable/Icon")]
    public class MainActivity : Activity
    {
        //private CustomMobileComputer mobileComputer;
        private PluginBarcodeMobileComputer mobileComputer;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.LayoutTest);

            // Get our button from the layout resource,
            // and attach an event to it
            var buttonStop = FindViewById<Button>(Resource.Id.buttonStopScan);

            buttonStop.Enabled = false;
            buttonStop.Click += ButtonStop_Click;

            var buttonStart = FindViewById<Button>(Resource.Id.buttonStartScan);

            buttonStart.Enabled = false;
            buttonStart.Click += ButtonStart_Click;

            var buttonCreate = FindViewById<Button>(Resource.Id.buttonCreate);

            buttonCreate.Click += ButtonCreate_Click;

            var buttonDispose = FindViewById<Button>(Resource.Id.buttonDispose);

            buttonDispose.Enabled = false;
            buttonDispose.Click += ButtonDispose_Click;

            var buttonSettings = FindViewById<Button>(Resource.Id.buttonSettings);

            buttonSettings.Enabled = false;
            buttonSettings.Click += ButtonSettings_Click;

            var buttonEan = FindViewById<Button>(Resource.Id.buttonEan13);

            buttonEan.Enabled = false;
            buttonEan.Click += ButtonEan_Click;

            var buttonPdf417 = FindViewById<Button>(Resource.Id.buttonPdf417);

            buttonPdf417.Enabled = false;
            buttonPdf417.Click += ButtonPdf417_Click;

            var buttonDataMatrix = FindViewById<Button>(Resource.Id.buttonDataMatrix);

            buttonDataMatrix.Enabled = false;
            buttonDataMatrix.Click += ButtonDataMatrix_Click;


            var buttonExit = FindViewById<Button>(Resource.Id.buttonExit);

            buttonExit.Click += ButtonExit_Click;

            if (PluginManagerBase.CommonContext == null)
            {
                PluginManagerBase.CommonContext = this;
            }
        }

        private bool dataMatrixOnly = false;

        private static void WaitUntilPluginScannerConnected(PluginBarcodeMobileComputer comp)
        {
            if (comp != null)
            {
                while (true)
                {
                    if (comp.IsConnected)
                    {
                        break;
                    }
                    Thread.Sleep(100);
                }
            }
        }

        private void ButtonDataMatrix_Click(object sender, EventArgs e)
        {
            if (mobileComputer != null)
            {
                try
                {
                    var buttonDataMatrix = FindViewById<Button>(Resource.Id.buttonDataMatrix);

                    var buttonPdf417 = FindViewById<Button>(Resource.Id.buttonPdf417);

                    var buttonEan13 = FindViewById<Button>(Resource.Id.buttonEan13);

                    if (!dataMatrixOnly)
                    {
                        mobileComputer.Scanner.EnableBarcodeType(BarcodeType.DataMatrix, true);

                        Toast.MakeText(this, "Mobile Computer EnableBarcodeType OK", ToastLength.Short).Show();

                        buttonDataMatrix.Text = "Push to Default Barcode settings";

                        buttonPdf417.Enabled = false;

                        buttonEan13.Enabled = false;

                        dataMatrixOnly = true;
                    }
                    else
                    {
                        mobileComputer.Scanner.EnableDefaultBarcodeTypes();

                        Toast.MakeText(this, "Mobile Computer EnableDefaultBarcodeTypes OK", ToastLength.Short).Show();

                        buttonDataMatrix.Text = "Push to DataMatix Only";

                        buttonPdf417.Enabled = true;

                        buttonEan13.Enabled = true;

                        dataMatrixOnly = false;
                    }

                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, "MobileComputer EnableBarcodeType error - " + ex.ToString(), ToastLength.Long).Show();
                }
            }
        }

        private bool pdf417Only = false;

        private void ButtonPdf417_Click(object sender, EventArgs e)
        {
            if (mobileComputer != null)
            {
                try
                {
                    var buttonDataMatrix = FindViewById<Button>(Resource.Id.buttonDataMatrix);

                    var buttonPdf417 = FindViewById<Button>(Resource.Id.buttonPdf417);

                    var buttonEan13 = FindViewById<Button>(Resource.Id.buttonEan13);

                    if (!pdf417Only)
                    {
                        mobileComputer.Scanner.EnableBarcodeType(BarcodeType.PDF417, true);

                        Toast.MakeText(this, "Mobile Computer EnableBarcodeType OK", ToastLength.Short).Show();

                        buttonPdf417.Text = "Push to Default Barcode settings";

                        buttonDataMatrix.Enabled = false;

                        buttonEan13.Enabled = false;

                        pdf417Only = true;
                    }
                    else
                    {
                        mobileComputer.Scanner.EnableDefaultBarcodeTypes();

                        Toast.MakeText(this, "Mobile Computer EnableDefaultBarcodeTypes OK", ToastLength.Short).Show();

                        buttonPdf417.Text = "Push to Pdf417 Only";

                        buttonDataMatrix.Enabled = true;

                        buttonEan13.Enabled = true;

                        pdf417Only = false;
                    }

                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, "MobileComputer EnableBarcodeType error - " + ex.ToString(), ToastLength.Long).Show();
                }
            }
        }

        private bool ean13Only = false;

        private void ButtonEan_Click(object sender, EventArgs e)
        {
            if (mobileComputer != null)
            {
                try
                {
                    var buttonDataMatrix = FindViewById<Button>(Resource.Id.buttonDataMatrix);

                    var buttonPdf417 = FindViewById<Button>(Resource.Id.buttonPdf417);

                    var buttonEan13 = FindViewById<Button>(Resource.Id.buttonEan13);

                    if (!ean13Only)
                    {
                        mobileComputer.Scanner.EnableBarcodeType(BarcodeType.EAN13, true);

                        Toast.MakeText(this, "Mobile Computer EnableBarcodeType OK", ToastLength.Short).Show();

                        buttonEan13.Text = "Push to Default Barcode settings";

                        buttonPdf417.Enabled = false;

                        buttonDataMatrix.Enabled = false;

                        ean13Only = true;
                    }
                    else
                    {
                        mobileComputer.Scanner.EnableDefaultBarcodeTypes();

                        Toast.MakeText(this, "Mobile Computer EnableDefaultBarcodeTypes OK", ToastLength.Short).Show();

                        buttonEan13.Text = "Push to Ean13 Only";

                        buttonDataMatrix.Enabled = true;

                        buttonPdf417.Enabled = true;

                        ean13Only = false;
                    }

                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, "MobileComputer EnableBarcodeType error - " + ex.ToString(), ToastLength.Long).Show();
                }
            }
        }

        private void ButtonExit_Click(object sender, EventArgs e)
        {
            Finish();
        }

        private void ButtonDispose_Click(object sender, EventArgs e)
        {
            try
            {
                if (mobileComputer != null)
                {
                    try
                    {
                        mobileComputer.Scan -= MobileComputer_Scan;
                        mobileComputer.Dispose();
                        mobileComputer = null;

                        Toast.MakeText(this, "Mobile Computer Dispose OK", ToastLength.Short).Show();
                    }
                    catch (Exception ex)
                    {
                        Toast.MakeText(this, "MobileComputer Dispose error - " + ex.ToString(), ToastLength.Long).Show();
                    }

                    mobileComputer = null;

                    var buttonCreate = FindViewById<Button>(Resource.Id.buttonCreate);

                    buttonCreate.Enabled = true;

                    var buttonDispose = FindViewById<Button>(Resource.Id.buttonDispose);

                    buttonDispose.Enabled = false;

                    var buttonStart = FindViewById<Button>(Resource.Id.buttonStartScan);

                    buttonStart.Enabled = false;

                    var buttonStop = FindViewById<Button>(Resource.Id.buttonStopScan);

                    buttonStop.Enabled = false;

                    var buttonEan = FindViewById<Button>(Resource.Id.buttonEan13);

                    buttonEan.Text = "Ean13";
                    buttonEan.Enabled = false;

                    var buttonPdf417 = FindViewById<Button>(Resource.Id.buttonPdf417);

                    buttonPdf417.Text = "PDF417";
                    buttonPdf417.Enabled = false;

                    var buttonDataMatrix = FindViewById<Button>(Resource.Id.buttonDataMatrix);

                    buttonDataMatrix.Text = "DataMatrix";
                    buttonDataMatrix.Enabled = false;

                    var buttonSettings = FindViewById<Button>(Resource.Id.buttonSettings);

                    buttonSettings.Text = "Settings";
                    buttonSettings.Enabled = false;

                    var textId = FindViewById<TextView>(Resource.Id.textDeviceId);

                    textId.Text = "Device Id:";

                    var textKeyboard = FindViewById<TextView>(Resource.Id.textHasKeyboard);

                    textKeyboard.Text = "Has Keyboard:";
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "MobileComputer Dispose error - " + ex.ToString(), ToastLength.Long).Show();
            }
        }

        private void ButtonSettings_Click(object sender, EventArgs e)
        {
            try
            {
                if (mobileComputer != null)
                {
                    if (mobileComputer.Scanner != null)
                    {
                        if (mobileComputer.Scanner.IsSettingsAvailable)
                        {
                            mobileComputer.Scanner.ShowSettings();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "MobileComputer ShowSettings error - " + ex.ToString(), ToastLength.Long).Show();
            }
        }

        private void ButtonCreate_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem((x) =>
            {



            try
            {
                mobileComputer = PluginBarcodeMobileComputer.ConnectPluginMobileComputer("cleverence.com.plugin.ipda050barcodescanner", this);// new CustomMobileComputer(this);

                WaitUntilPluginScannerConnected(mobileComputer);


                    RunOnUiThread(() => { Toast.MakeText(this, "Mobile Computer Constructor OK", ToastLength.Short).Show(); });

                    mobileComputer.Scan += MobileComputer_Scan;
                    mobileComputer.TurnOnScanner();

                    RunOnUiThread(() => { Toast.MakeText(this, "Mobile Computer TurnOnScanner OK", ToastLength.Short).Show(); });
                }
            catch (Exception ex)
            {
                    //RunOnUIThread(() => { Toast.MakeText(this, "MobileComputer Constructor error - " + ex.ToString(), ToastLength.Long).Show(); });
                    return;
            }

                RunOnUiThread(() =>
            {

                var buttonCreate = FindViewById<Button>(Resource.Id.buttonCreate);

                buttonCreate.Enabled = false;

                var buttonStart = FindViewById<Button>(Resource.Id.buttonStartScan);

                buttonStart.Enabled = false;

                var buttonStop = FindViewById<Button>(Resource.Id.buttonStopScan);

                buttonStop.Enabled = true;

                var buttonDispose = FindViewById<Button>(Resource.Id.buttonDispose);

                buttonDispose.Enabled = true;

                var buttonEan = FindViewById<Button>(Resource.Id.buttonEan13);

                var buttonPdf417 = FindViewById<Button>(Resource.Id.buttonPdf417);

                var buttonDataMatrix = FindViewById<Button>(Resource.Id.buttonDataMatrix);

                if (mobileComputer.Scanner != null)
                {
                    var supported = mobileComputer.Scanner.GetSupportedSymbologies();

                    if (supported != null)
                    {
                        if (supported.Length > 0)
                        {
                            buttonDataMatrix.Enabled = true;
                            buttonEan.Enabled = true;
                            buttonPdf417.Enabled = true;

                            buttonEan.Text = "Push to Ean13 Only";
                            buttonPdf417.Text = "Push to Pdf417 Only";

                            buttonDataMatrix.Text = "Push to DataMatrix Only";
                        }
                    }
                }

                var textActive = FindViewById<TextView>(Resource.Id.textScannerStatus);

                var buttonSettings = FindViewById<Button>(Resource.Id.buttonSettings);

                if (mobileComputer.Scanner != null)
                {
                    if (mobileComputer.Scanner.IsSettingsAvailable)
                    {
                        buttonSettings.Enabled = true;
                        buttonSettings.Text = "Settings avalable";
                    }
                    else
                    {
                        buttonSettings.Enabled = false;
                        buttonSettings.Text = "Settings not avalable";
                    }

                    var status = mobileComputer.Scanner.IsTurnedOn ? "Enabled" : "Disabled";

                    textActive.Text = "Scanner Status: " + status;
                }

                var textId = FindViewById<TextView>(Resource.Id.textDeviceId);

                textId.Text = "Device Id: " + mobileComputer.GetDeviceId();

                var textKeyboard = FindViewById<TextView>(Resource.Id.textHasKeyboard);

                textKeyboard.Text = "Has Keyboard: " + (mobileComputer.HasKeyboard ? "Yes" : "No");

                Toast.MakeText(this, "Mobile Computer HasKeyboard OK", ToastLength.Short);

            });

            });
        }

        protected override void OnDestroy()
        {
            try
            {
                if (mobileComputer != null)
                {
                    mobileComputer.Scan -= MobileComputer_Scan;
                    mobileComputer.Scanner.EnableDefaultBarcodeTypes();

                    Toast.MakeText(this, "Mobile Computer EnableDefaulBarcodeTypes OK", ToastLength.Short).Show();

                    mobileComputer.Dispose();
                    mobileComputer = null;

                    Toast.MakeText(this, "Mobile Computer Dispose OK", ToastLength.Short).Show();
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "MobileComputer Dispose error - " + ex.ToString(), ToastLength.Long).Show();
            }


            base.OnDestroy();

        }

        private void MobileComputer_Scan(ScanArgs e)
        {
            TextView textView = FindViewById<TextView>(Resource.Id.textResult);
            textView.Text = e.Text;
        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            try
            {
                mobileComputer.TurnOnScanner();

                Toast.MakeText(this, "Mobile Computer TurnOnScanner OK", ToastLength.Short).Show();
            }
            catch(Exception ex)
            {
                Toast.MakeText(this, "MobileComputer TurnOnScanner error - " + ex.ToString(), ToastLength.Long).Show();
            }

            Button buttonStop = FindViewById<Button>(Resource.Id.buttonStopScan);

            buttonStop.Enabled = true;

            Button buttonStart = FindViewById<Button>(Resource.Id.buttonStartScan);

            buttonStart.Enabled = false;

            var textStatus = FindViewById<TextView>(Resource.Id.textScannerStatus);

            textStatus.Text = "Scanner Status: " + (mobileComputer.Scanner.IsTurnedOn ? "Enabled" : "Disabled");
        }

        private void ButtonStop_Click(object sender, EventArgs e)
        {
            try
            {
                mobileComputer.TurnOffScanner();

                Toast.MakeText(this, "Mobile Computer TurnOffScanner OK", ToastLength.Short).Show();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "MobileComputer TurnOffScanner error - " + ex.ToString(), ToastLength.Long).Show();
            }

            Button buttonStop = FindViewById<Button>(Resource.Id.buttonStopScan);

            buttonStop.Enabled = false;

            Button buttonStart = FindViewById<Button>(Resource.Id.buttonStartScan);

            buttonStart.Enabled = true;

            var textStatus = FindViewById<TextView>(Resource.Id.textScannerStatus);

            textStatus.Text = "Scanner Status: " + (mobileComputer.Scanner.IsTurnedOn ? "Enabled" : "Disabled");
        }
    }
}

