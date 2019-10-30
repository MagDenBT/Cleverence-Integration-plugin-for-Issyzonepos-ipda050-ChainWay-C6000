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
    public class CustomMobileComputer : RemoteBarcodeDeviceBase
    {
        public override int BatteryLifePercent => base.BatteryLifePercent;

        public override bool ForceHideActionBar { get { return true; } } //base.ForceHideActionBar;

        public override string GetDeviceName()
        {

            return Build.Model;
        }


        public override string GetManufacturer()
        {
            if (string.IsNullOrEmpty(Android.OS.Build.Manufacturer))
                return "";

            string[] arr = Android.OS.Build.Manufacturer.Trim().Split(' ');
            return arr[0];
        }

        public override string GetModelAndCode()
        {

            if (Android.OS.Build.Serial.ToUpper().IndexOf("0123456789ABCDEF") > -1 ||
                 Android.OS.Build.Serial == "0000000000" ||
                 Android.OS.Build.Serial == "0000000000000000")
            {
                string id = "";
                try
                {
                    id = GetTelephoneId();
                }
                catch
                {
                    // Запись в лог ошибки
                }

                if (string.IsNullOrWhiteSpace(id))
                {
                    try
                    {
                        id = GetWiFiMac();
                    }
                    catch
                    {
                        // Запись в лог ошибки
                    }
                }

                if (string.IsNullOrWhiteSpace(id))
                    id = "Error[" + Android.OS.Build.Serial + "]";

                id = Android.OS.Build.Model + "-" + id;
                id = id.Trim();
                id = id.Trim(new char[] { (char)0 });
                id = id.Replace(" ", "");

                return id;
            }
            else
            {
                string id = Android.OS.Build.Model + "-" + Android.OS.Build.Serial;
                id = id.Trim();
                id = id.Trim(new char[] { (char)0 });
                id = id.Replace(" ", "");

                return id;
            }
        }

        public override bool HasArrows
        {
            get
            {
                return false;
            }
        }
        public override bool HasKeyboard
        {
            get
            {
                return true;
            }
        }

        public override bool HasNumericKeyboard
        {
            get
            {
                return true;
            }
        }

        public override void Wakeup()
        {

            base.Wakeup();
        }
    }
}