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
using Cleverence.Warehouse.Compact;
using Com.Barcode;

namespace Cleverence.Barcoding.Integration.Plugin
{
    public class CustomScanner : RemoteBarcodeScannerBase
    {

        private BarcodeUtility barcodeUtility = null;
        private BarcodeCallback barcodeDataReceiver = null;
        private bool _isTurnedOn = false;
        private bool isOpen = false;

        public CustomScanner(Context context)
            : base(context)
        {
            barcodeUtility = BarcodeUtility.Instance;
            InitScannerModule();
        }

        
        public override bool IsTurnedOn => this._isTurnedOn;

        
        public override void TurnOn()
        {
            InitScannerModule();
            _isTurnedOn = true;
           // barcodeUtility.StartScan(context, BarcodeUtility.ModuleType.Barcode2d);

        }

        public override void TurnOff()
        {
            if (_isTurnedOn)
            {
                // barcodeUtility.StopScan(context, BarcodeUtility.ModuleType.Barcode2d);
                DeInitScannerModule();
                _isTurnedOn = false;
            }
        }

        public override void BackupSymbologySettings()
        {
            base.BackupSymbologySettings();
        }

        public override void RestoreSymbologySettings()
        {
            base.RestoreSymbologySettings();
        }

        public override void DisableBarcodeType(BarcodeType type)
        {
            base.DisableBarcodeType(type);
        }

        public override void EnableBarcodeType(BarcodeType type, bool enableOnlyThis)
        {
            base.EnableBarcodeType(type, enableOnlyThis);
        }

        public override void EnableDefaultBarcodeTypes()
        {
            base.EnableDefaultBarcodeTypes();
        }

        public override bool ConnectToDecoder()
        {
            InitScannerModule();
            return true;
        }

        public override bool DisconnectFromDecoder()
        {
            DeInitScannerModule();
            return true;
            //return base.DisconnectFromDecoder();
        }

        public override void StartScannerModule()
        {
            base.StartScannerModule();
        }

        public override void StopScannerModule()
        {
            base.StopScannerModule();
        }

        public override bool ShowScanButton => base.ShowScanButton;

        public override bool IsQRCodeEnabled
        {
            get => base.IsQRCodeEnabled;
            set => base.IsQRCodeEnabled = value;
        }

        protected override void OnGlobalKeyDown(View.KeyEventArgs e)
        {
            base.OnGlobalKeyDown(e);
        }

        protected override void OnGlobalKeyPress(View.KeyEventArgs e)
        {
            base.OnGlobalKeyPress(e);
        }

        protected override void OnGlobalKeyUp(View.KeyEventArgs e)
        {
            base.OnGlobalKeyUp(e);
        }

        protected override void OnGlobalTouchEvent(View.TouchEventArgs e)
        {
            base.OnGlobalTouchEvent(e);
        }

        /// <summary>
        /// Получить список поддерживаемых кодировок сканера штрихкодов.
        /// </summary>
        /// <returns></returns>
        //public override BarcodeType[] GetSupportedSymbologies()
        //{
        //    return new BarcodeType[0];
        //}

        /// <summary>
        /// Освободить ресурсы объекта сканера штрихкодов.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            DeInitScannerModule();
            base.Dispose(disposing);
        }

        public override bool IsSettingsAvailable
        {
            get
            {
                return false;
            }
        }

        public override void ShowSettings()
        {
            
        }

        private void DeInitScannerModule()
        {
            if (barcodeUtility != null)
            {
                barcodeUtility.Close(context, BarcodeUtility.ModuleType.Barcode2d);
                isOpen = false;
            }

        }

        private void InitScannerModule()
        {
            if (!isOpen && barcodeUtility != null)
            {

                barcodeUtility.SetOutputMode(context, 2);//Установить широковещательный прием данных
                barcodeUtility.SetScanResultBroadcast(context, "com.scanner.broadcast", "data");//Установите трансляцию для получения данных
                barcodeUtility.Open(context, BarcodeUtility.ModuleType.Barcode2d);//Открыть 2D
                barcodeUtility.SetContinuousScanTimeOut(context, 60);
                barcodeUtility.SetContinuousScanIntervalTime(context, 5);
                barcodeUtility.EnableEnter(context, false);//Закрыть возврат каретки
       

                if (barcodeDataReceiver == null)
                {
                    barcodeDataReceiver = new BarcodeCallback(this);
                    IntentFilter intentFilter = new IntentFilter();
                    intentFilter.AddAction("com.scanner.broadcast");
                    context.RegisterReceiver(barcodeDataReceiver, intentFilter);
                }
            }

        }
        public class BarcodeCallback : BroadcastReceiver
        {

            CustomScanner scanActivity;

            public BarcodeCallback(CustomScanner scanActivity)
            {
                this.scanActivity = scanActivity;
            }

            public override void OnReceive(Context context, Intent intent)
            {
                String barCode = intent.GetStringExtra("data");
                if (barCode != null && !barCode.Equals(""))
                {
                    scanActivity.OnScan(barCode);
                }
                else
                {
                    barCode = "Scan fail";
                }

            }
        }

    }
}