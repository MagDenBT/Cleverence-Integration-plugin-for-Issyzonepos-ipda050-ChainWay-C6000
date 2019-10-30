using Android.Content;
using System;

namespace Cleverence.Barcoding
{
	/// <summary>
	/// Базовый класс мобильного устройства
	/// </summary>
    public class MobileComputer : IDisposable
	{
        protected Context context;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context">Андроид-контекст для внутренних нужд объекта.</param>
        public MobileComputer(Context context)
        {
            this.context = context;
        }



        /// <summary>
        /// Получить объект класса системной службы.
        /// </summary>
        /// <typeparam name="T">Тип объекта службы.</typeparam>
        /// <param name="name">Наименование получаемой службы.</param>
        /// <returns></returns>
        private T GetSystemService<T>(string name)
            where T : class
        {
            var ret = context.GetSystemService(name);
            if (ret != null)
            {
                return ret as T;
            }

            return null;
        }

        public virtual bool IsConnected
        {
            get
            {
                return true;
            }
        }

        public virtual void Reinitialize()
        {

        }

        public virtual void Wakeup()
        {
        }

        public virtual void OnBeginCheckConnection()
        {

        }

        /// <summary>
        /// Признак устройства-прайсчекера
        /// </summary>
        public virtual bool IsPriceChecker
        {
            get
            {
                return false;
            }
        }

        public virtual bool IsAndroid
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Имеет ли мобильное устройство физическую цифровую клавиатуру.
        /// </summary>
        public virtual bool HasNumericKeyboard
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Для совместимости (в Win-клиенте и CE есть такое свойство).
        /// </summary>
        public bool NoKeyboard
        {
            get
            {
                return !HasKeyboard;
            }
        }

        /// <summary>
        /// Имеет ли терминал физические кнопки стрелок вверх, вниз, влево, вправо
        /// </summary>
        public virtual bool HasArrows
        {
            get
            {
                return false;
            }
        }

        public virtual bool ForceHideActionBar
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Первичная инициализация объекта собильного устройства.
        /// </summary>
        public virtual void Initialize()
        {
        }

        /// <summary>
        /// Имеет ли мобильное устройство физическую клавиатуру.
        /// </summary>
        public virtual bool HasKeyboard
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Получить наименование производителя мобильного устройства.
        /// </summary>
        /// <returns></returns>
        protected virtual string GetManufacturer()
        {
            if (string.IsNullOrEmpty(Android.OS.Build.Manufacturer))
                return "";

            string[] arr = Android.OS.Build.Manufacturer.Trim().Split(' ');
            return arr[0];
        }

        protected string GetTelephoneId()
        {
            Android.Telephony.TelephonyManager tm = GetSystemService<Android.Telephony.TelephonyManager>(Android.Content.Context.TelephonyService);
            return tm.DeviceId;
        }

        protected string GetWiFiMac()
        {
            Android.Net.Wifi.WifiManager wifiManager = GetSystemService<Android.Net.Wifi.WifiManager>(Android.Content.Context.WifiService);
            string mac = wifiManager.ConnectionInfo.MacAddress;
            return mac.Replace(":", "");
        }

        /// <summary>
        /// Получить модель и код мобильного устройства.
        /// </summary>
        /// <returns></returns>
        protected virtual string GetModelAndCode()
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

        /// <summary>
        /// Выполнить очистку мобильного устройства с указанным идентификатором.
        /// </summary>
        /// <param name="strDeviceId">Идентификатор устройства для очистки.</param>
        /// <returns></returns>
        protected virtual string CleanupDeviceId(string strDeviceId)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            foreach (char c in strDeviceId)
            {
                if (IsAllowedChar(c)) sb.Append(c);
            }

            return sb.ToString();
        }

        private bool IsAllowedChar(char c)
        {
            if (Char.IsDigit(c)) return true;
            if (c >= 'A' && c <= 'Z') return true;
            if (c >= 'a' && c <= 'z') return true;
            if (c == '-') return true;
            if (c == '_') return true;
            if (c == '@') return true;
            if (c == '.') return true;

            return false;
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
        /// Объект сканера штрихкодов мобильного устройства.
        /// </summary>
        public virtual BarcodeScanner Scanner
        {
            get { return null; }
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

        #region IDisposable Members

        /// <summary>
        /// Освободить ресурсы объекта сканнера штрихкодов.
        /// </summary>
        public virtual void Dispose()
		{
            try
            {
                if (this.Scanner != null && this.Scanner is IDisposable)
                {
                    (this.Scanner as IDisposable).Dispose();
                }
            }
            catch { }
		}

        #endregion
    }
}
