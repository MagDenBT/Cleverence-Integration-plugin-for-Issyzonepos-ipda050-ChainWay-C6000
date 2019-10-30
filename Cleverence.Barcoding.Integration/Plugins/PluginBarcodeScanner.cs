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
using Cleverence.Barcoding.Remote;

namespace Cleverence.Warehouse.Compact.Plugins
{
    public class PluginBarcodeScanner
    {
        private PluginBarcodeScannerConnection connection;

        private PluginBarcodeScannerBroadcastReceiver receiver;

        private string serviceName;

        protected void OnGlobalKeyDown(object sender, View.KeyEventArgs e)
        {
            if (IsConnected)
            {
                connection.OnGlobalKeyDown(e);
            }
        }

        protected void OnGlobalKeyPress(object sender, View.KeyEventArgs e)
        {
            if (IsConnected)
            {
                connection.OnGlobalKeyPress(e);
            }
        }

        protected void OnGlobalKeyUp(object sender, View.KeyEventArgs e)
        {
            if (IsConnected)
            {
                connection.OnGlobalKeyUp(e);
            }
        }

        protected void OnGlobalTouchEvent(object sender, View.TouchEventArgs e)
        {
            if (IsConnected)
            {
                connection.OnGlobalTouchEvent(e);
            }
            //base.OnGlobalTouchEvent(sender, e);
        }

        public bool IsConnected
        {
            get
            {
                if (connection != null)
                {
                    return connection.IsConnected;
                }

                return false;
            }
        }

        /// <summary>
        /// Получить список поддерживаемых кодировок сканера штрихкодов.
        /// </summary>
        /// <returns></returns>
        public virtual BarcodeType[] GetSupportedSymbologies()
        {
            return new BarcodeType[0];
        }

        private Context context;

        internal PluginBarcodeScanner(string serviceName, PluginBarcodeScannerConnection connection, Context context)
        {
            this.serviceName = serviceName;
            this.connection = connection;
            this.context = context;

            if (PluginManagerBase.CommonContext == null)
            {
                PluginManagerBase.CommonContext = context;
            }


            //SetCallback();

            receiver = new PluginBarcodeScannerBroadcastReceiver(this);

            var filter = new IntentFilter();
            filter.AddAction("com.cleverence.plugins.barcodeScannerScan");

            context.RegisterReceiver(receiver, filter);
        }

        public void Dispose()
        {
            if (receiver != null)
            {
                context.UnregisterReceiver(receiver);

                receiver.Dispose();
                receiver = null;
            }

            //base.Dispose();
        }

        public bool IsTurnedOn
        {
            get
            {
                if (connection != null)
                {
                    if (connection.IsConnected)
                    {
                        return connection.IsTurnedOn;
                    }
                }

                return false;
            }
        }

        public void TurnOff()
        {
            if (connection != null)
            {
                if (connection.IsConnected)
                {
                    connection.TurnOff();
                }
            }
        }

        public void TurnOn()
        {
            if (connection != null)
            {
                if (connection.IsConnected)
                {
                    connection.TurnOn();
                }
            }
        }

        public void BackupSymbologySettings()
        {
            if (connection != null)
            {
                if (connection.IsConnected)
                {
                    connection.BackupSymbologySettings();
                }
            }
        }

        public void RestoreSymbologySettings()
        {
            if (connection != null)
            {
                if (connection.IsConnected)
                {
                    connection.RestoreSymbologySettings();
                }
            }
        }

        public void EnableDefaultBarcodeTypes()
        {
            if (connection != null)
            {
                if (connection.IsConnected)
                {
                    connection.EnableDefaultBarcodeTypes();
                }
            }
        }

        public void EnableBarcodeType(BarcodeType type, bool enableOnlyThis)
        {
            if (connection != null)
            {
                if (connection.IsConnected)
                {
                    connection.EnableBarcodeType(type, enableOnlyThis);
                }
            }
        }

        public void DisableBarcodeType(BarcodeType type)
        {
            if (connection != null)
            {
                if (connection.IsConnected)
                {
                    connection.DisableBarcodeType(type);
                }
            }
        }

        public bool DisconnectFromDecoder()
        {
            if (connection != null)
            {
                if (connection.IsConnected)
                {
                    return connection.DisconnectFromDecoder();
                }
            }

            return false;
        }

        public bool ConnectToDecoder()
        {
            if (connection != null)
            {
                if (connection.IsConnected)
                {
                    return connection.ConnectToDecoder();
                }
            }

            return false;
        }

        public void Initialize()
        {
            if (connection != null)
            {
                if (connection.IsConnected)
                {
                    connection.ScannerInitialize();
                }
            }
        }

        public bool IsQRCodeEnabled
        {
            get
            {
                if (connection != null)
                {
                    if (connection.IsConnected)
                    {
                        return connection.IsQRCodeEnabled;
                    }
                }

                return false;
            }

            set
            {
                if (connection != null)
                {
                    if (connection.IsConnected)
                    {
                        connection.IsQRCodeEnabled = value;
                    }
                }
            }
        }

        public bool IsSettingsAvailable
        {
            get
            {
                if (connection != null)
                {
                    if (connection.IsConnected)
                    {
                        return connection.IsSettingsAvailable;
                    }
                }

                return false;
            }
        }

        public void StartScannerModule()
        {
            if (connection != null)
            {
                if (connection.IsConnected)
                {
                    connection.StartScannerModule();
                }
            }
        }

        public void StopScannerModule()
        {
            if (connection != null)
            {
                if (connection.IsConnected)
                {
                    connection.StopScannerModule();
                }
            }
        }

        public bool ShowScanButton
        {
            get
            {
                if (connection != null)
                {
                    if (connection.IsConnected)
                    {
                        return connection.ShowScanButton;
                    }
                }

                return false;
            }
        }

        public void ShowSettings()
        {
            if (connection != null)
            {
                if (connection.IsConnected)
                {
                    connection.ShowSettings();
                }
            }
        }

        internal void OnScannedCallback(string text, BarcodeType type)
        {
            var strBarcode = text;

            //switch (type)
            //{
            //    case BarcodeType.EAN128:
            //        var strEAN128 = Barcoding.Ean128.Format(strBarcode);
            //        if (!string.IsNullOrEmpty(strEAN128))
            //        {
            //            strBarcode = strEAN128;
            //        }

            //        break;
            //}

            OnScan(strBarcode);
        }

        /// <summary>
        /// Вызывается при успешном сканировании штрихкода.
        /// </summary>
		public event ScanEventHandler Scan;



        /// <summary>
        /// Обработка события сканирования сканера штрихкодов.
        /// </summary>
        /// <param name="text">Отсканированный текст.</param>
        public virtual void OnScan(string text)
        {
            if (string.IsNullOrEmpty(text))
                return;

            var ev = Scan;

            if (ev != null)
            {
                ScanArgs sa = new ScanArgs(text);
                ev(sa);
            }
        }
    }
}