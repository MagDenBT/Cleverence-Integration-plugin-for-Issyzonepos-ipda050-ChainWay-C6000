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
    public class PluginBarcodeMobileComputer
    {
        private PluginBarcodeScannerConnection connection;

        private string serviceName;

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

        private Context context;

        private PluginBarcodeMobileComputer(string serviceName, PluginBarcodeScannerConnection connection, Context context)
        {
            this.serviceName = serviceName;
            this.connection = connection;
            this.context = context;
        }

        public static PluginBarcodeMobileComputer ConnectPluginMobileComputer(string serviceName, Context context)
        {
            //var conn = BarcodeDevicePluginsManager.GetRemoteBarcodeScannerService(serviceName);

            var manager = new BarcodeDevicePluginsManager();

            var conn = manager.GetRemoteBarcodeScannerService(serviceName);

            if (conn != null)
            {
                return new PluginBarcodeMobileComputer(serviceName, conn, context);
            }

            return null;
        }

        private void DisconnectPlugin()
        {
            //BarcodeDevicePluginsManager.RemoveRemoteBarcodeScannerService(serviceName);
            BarcodeDevicePluginsManager.RemoveService(serviceName);
        }

        protected string CleanupDeviceId(string strDeviceId)
        {
            if (IsConnected)
            {
                return connection.CleanupDeviceId(strDeviceId);
            }

            return string.Empty;
        }

        public int GetBatteryLifePercent()
        {
            if (IsConnected)
            {
                return connection.BatteryLifePercent;
            }

            return 0;
        }

        public string GetDeviceName()
        {
            if (IsConnected)
            {
                return connection.GetDeviceName();
            }

            return string.Empty;
        }

        protected string GetManufacturer()
        {
            if (IsConnected)
            {
                return connection.GetManufacturer();
            }

            return string.Empty;
        }

        protected string GetModelAndCode()
        {
            if (IsConnected)
            {
                return connection.GetModelAndCode();
            }

            return string.Empty;
        }

        public bool HasArrows
        {
            get
            {
                if (IsConnected)
                {
                    return connection.HasArrows;
                }

                return false;
            }
        }

        public bool HasKeyboard
        {
            get
            {
                if (IsConnected)
                {
                    return connection.HasKeyboard;
                }

                return false;
            }
        }

        public bool HasNumericKeyboard
        {
            get
            {
                if (IsConnected)
                {
                    return connection.HasNumericKeyboard;
                }

                return false;
            }
        }

        public void Initialize()
        {
            if (IsConnected)
            {
                connection.DeviceInitialize();
            }
        }

        public void Reinitialize()
        {
            if (IsConnected)
            {
                connection.DeviceReinitialize();
            }
        }

        public void Wakeup()
        {
            if (IsConnected)
            {
                connection.Wakeup();
            }
        }

        public bool ForceHideActionBar
        {
            get
            {
                if (IsConnected)
                {
                    return connection.ForceHideActionBar;
                }

                return false;
            }
        }

        private PluginBarcodeScanner scanner = null;

        //private BluetoothInterface bluetooth = null;

        public PluginBarcodeScanner Scanner
        {
            get
            {
                if (scanner == null)
                {
                    scanner = new PluginBarcodeScanner(serviceName, connection, context);// Plugins.PluginBarcodeScanner.ConnectPluginScanner("cleverence.com.plugin.newlandn2sbarcodescanner");
                }

                return scanner;
            }
        }

        /// <summary>
        /// Получить идентификатор мобильного устройства.
        /// </summary>
        /// <returns></returns>
		public virtual string GetDeviceId()
        {
            string id = GetModelAndCode();

            string manufacturer = GetManufacturer();

            string strDeviceId;
            if (string.IsNullOrEmpty(manufacturer))
            {
                strDeviceId = "@" + id;
            }
            else
            {
                strDeviceId = "@" + manufacturer + "-" + id;
            }

            return CleanupDeviceId(strDeviceId);
        }

        /// <summary>
        /// Событие успешного сканирования штрихкода.
        /// </summary>
        public event ScanEventHandler Scan
        {
            add
            {
                if (this.Scanner != null)
                    this.Scanner.Scan += value;
            }
            remove
            {
                if (this.Scanner != null)
                    this.Scanner.Scan -= value;
            }
        }

        /// <summary>
        /// Включить режим сканирования сканера штрихкодов мобильного устройства.
        /// </summary>
        public void TurnOnScanner()
        {
            if (this.Scanner != null)
                this.Scanner.TurnOn();
        }

        /// <summary>
        /// Отключить режим сканирования сканера штрихкодов мобильного устройства.
        /// </summary>
        public void TurnOffScanner()
        {
            if (this.Scanner != null)
                this.Scanner.TurnOff();
        }

        /*public override BluetoothInterface Bluetooth
        {
            get
            {
                if (bluetooth == null)
                {
                    bluetooth = new BluetoothInterface();
                }

                return bluetooth;
            }
        }*/

        public void Dispose()
        {
            if (scanner != null)
            {
                scanner.Dispose();
                scanner = null;
            }

            //if (bluetooth != null)
            //{
            //    bluetooth.Dispose();
            //    bluetooth = null;
            //}

            DisconnectPlugin();
        }
    }
}