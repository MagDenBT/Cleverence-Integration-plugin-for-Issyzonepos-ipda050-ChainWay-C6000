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
using Com.Cleverence.Barcoding;

namespace Cleverence.Warehouse.Compact.Plugins
{
    public class PluginBarcodeScannerConnection : Java.Lang.Object, IPluginConnection
    {
        private IRemoteBarcodeDeviceInterface scannerInterface;
        //private RemoteBarcodeScannerBinder binder;

        //private Messenger messanger;

        public void OnGlobalKeyDown(View.KeyEventArgs e)
        {
            if (IsConnected)
            {
                var ret = scannerInterface.FormControlOnGlobalKeyDown(e.Event.DownTime, e.Event.EventTime, (int)(e.Event.Action), (int)(e.KeyCode), e.Event.RepeatCount, (int)(e.Event.MetaState), e.Event.DeviceId, e.Event.ScanCode, (int)(e.Event.Flags), (int)e.Event.Source);
                e.Handled = ret;
            }
        }

        public void OnGlobalKeyPress(View.KeyEventArgs e)
        {
            if (IsConnected)
            {
                var ret = scannerInterface.FormControlOnGlobalKeyPress(e.Event.DownTime, e.Event.EventTime, (int)(e.Event.Action), (int)(e.KeyCode), e.Event.RepeatCount, (int)(e.Event.MetaState), e.Event.DeviceId, e.Event.ScanCode, (int)(e.Event.Flags), (int)e.Event.Source);
                e.Handled = ret;
            }
        }

        public void OnGlobalKeyUp(View.KeyEventArgs e)
        {
            if (IsConnected)
            {
                var ret = scannerInterface.FormControlOnGlobalKeyUp(e.Event.DownTime, e.Event.EventTime, (int)(e.Event.Action), (int)(e.KeyCode), e.Event.RepeatCount, (int)(e.Event.MetaState), e.Event.DeviceId, e.Event.ScanCode, (int)(e.Event.Flags), (int)e.Event.Source);
                e.Handled = ret;
            }
        }

        public void OnGlobalTouchEvent(View.TouchEventArgs e)
        {

        }

        public bool IsConnected
        {
            get;
            private set;
        }

        public bool IsTurnedOn
        {
            get
            {
                if (IsConnected)
                {
                    return scannerInterface.GetIsTurnedOn();
                }

                return false;
            }
        }

        public bool IsQRCodeEnabled
        {
            get
            {
                if (IsConnected)
                {
                    return scannerInterface.GetIsQrCodeEnabled();
                }

                return false;
            }

            set
            {
                if (IsConnected)
                {
                    scannerInterface.SetIsQrCodeEnabled(value);
                }
            }
        }

        public bool IsSettingsAvailable
        {
            get
            {
                if (IsConnected)
                {
                    return scannerInterface.GetIsSettingsAvailable();
                }

                return false;
            }
        }

        public bool ShowScanButton
        {
            get
            {
                if (IsConnected)
                {
                    return scannerInterface.GetShowScanButton();
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
                    return scannerInterface.GetDeviceHasNumericKeyboard();
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
                    return scannerInterface.GetDeviceHasKeyboard();
                }

                return false;
            }
        }

        public bool HasArrows
        {
            get
            {
                if (IsConnected)
                {
                    return scannerInterface.GetDeviceHasArrows();
                }

                return false;
            }
        }

        public bool ForceHideActionBar
        {
            get
            {
                if (IsConnected)
                {
                    return scannerInterface.GetDeviceForceHideActionBar();
                }

                return false;
            }
        }

        public int BatteryLifePercent
        {
            get
            {
                if (IsConnected)
                {
                    return scannerInterface.GetDeviceBatteryLifePercent();
                }

                return 0;
            }
        }

        public void OnServiceConnected(ComponentName name, IBinder service)
        {
            scannerInterface = IRemoteBarcodeDeviceInterfaceStub.AsInterface(service);

            //binder = service. as RemoteBarcodeScannerBinder;

            //if (binder != null)
            {
                //messanger = new Messenger(service);

                //RemoteBarcodeScanner = binder.RemoteBarcodeScanner;
                IsConnected = service != null;
            }
        }

        public void OnServiceDisconnected(ComponentName name)
        {
            IsConnected = false;
            scannerInterface = null;
            //RemoteBarcodeScanner = null;
            //binder = null;
            //messanger = null;
        }

        public bool IsSupportCheckMark()
        {
            return false;
        }

        public void BackupSymbologySettings()
        {
            if (IsConnected)
            {
                scannerInterface.BackupSymbologySettings();
            }
        }

        public void RestoreSymbologySettings()
        {
            if (IsConnected)
            {
                scannerInterface.RestoreSymbologySettings();
            }
        }

        public void StopScannerModule()
        {
            if (IsConnected)
            {
                scannerInterface.StopScannerModule();
            }
        }

        public void StartScannerModule()
        {
            if (IsConnected)
            {
                scannerInterface.StartScannerModule();
            }
        }

        public void EnableDefaultBarcodeTypes()
        {
            if (IsConnected)
            {
                scannerInterface.EnableDefaultBarcodeTypes();
            }
        }

        public void EnableBarcodeType(BarcodeType type, bool enableOnlyThis)
        {
            if (IsConnected)
            {
                scannerInterface.EnableBarcodeType((int)type, enableOnlyThis);
            }
        }

        public void DisableBarcodeType(BarcodeType type)
        {
            if (IsConnected)
            {
                scannerInterface.DisableBarcodeType((int)type);
            }
        }

        public void TurnOn()
        {
            if (IsConnected)
            {
                scannerInterface.TurnOn();
            }
        }

        public void TurnOff()
        {
            if (IsConnected)
            {
                scannerInterface.TurnOff();
            }
        }

        public void ScannerInitialize()
        {
            if (IsConnected)
            {
                scannerInterface.Initialize();
            }
        }

        public void DeviceInitialize()
        {
            if (IsConnected)
            {
                scannerInterface.DeviceInitialize();
            }
        }

        public bool ConnectToDecoder()
        {
            if (IsConnected)
            {
                return scannerInterface.ConnectToDecoder();
            }

            return false;
        }

        public bool DisconnectFromDecoder()
        {
            if (IsConnected)
            {
                return scannerInterface.DisconnectFromDecoder();
            }

            return false;
        }

        public void ShowSettings()
        {
            if (IsConnected)
            {
                scannerInterface.ShowSettings();
            }
        }

        public void DeviceReinitialize()
        {
            if (IsConnected)
            {
                scannerInterface.DeviceReinitialize();
            }
        }

        public void Wakeup()
        {
            if (IsConnected)
            {
                scannerInterface.DeviceWakeup();
            }
        }

        public void OnBeginCheckConnection()
        {
            if (IsConnected)
            {
                scannerInterface.DeviceOnBeginCheckConnection();
            }
        }

        public string GetManufacturer()
        {
            if (IsConnected)
            {
                return scannerInterface.GetDeviceManufacturer();
            }

            return string.Empty;
        }

        public string GetModelAndCode()
        {
            if (IsConnected)
            {
                return scannerInterface.GetDeviceModelAndCode();
            }

            return string.Empty;
        }

        public string CleanupDeviceId(string deviceId)
        {
            if (IsConnected)
            {
                return scannerInterface.CleanupDeviceId(deviceId);
            }

            return deviceId;
        }

        public string GetDeviceName()
        {
            if (IsConnected)
            {
                return scannerInterface.GetDeviceName();
            }

            return string.Empty;
        }
    }
}