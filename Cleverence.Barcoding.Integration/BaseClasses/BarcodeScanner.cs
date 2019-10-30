using System;
using Cleverence.Barcoding;
using Android.Content;
using Android.Views;

namespace Cleverence.Barcoding
{

    /// <summary>
    /// Базовый класс сканера штрихкодов мобильного устройства.
    /// </summary>
    public abstract class BarcodeScanner : IDisposable
	{
        protected Context context;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context">Андроид-контекст для внутренних нужд объекта.</param>
        public BarcodeScanner(Context context)
        {
            this.context = context;
        }

        public virtual bool IsConnected
        {
            get
            {
                return true;
            }
        }

        public virtual void Initialize()
        {
        }

        // Добавлено для Honeywell Dolphin 70e Black, (и для Xamarin EMDK-based сканеров)
        // т.к. он не умеет работать одновременно
        // со сканером и фотокамерой
        public virtual bool ConnectToDecoder()
        {
            return false;
        }

        // Добавлено для Honeywell Dolphin 70e Black, (и для Xamarin EMDK-based сканеров)
        // т.к. он не умеет работать одновременно
        // со сканером и фотокамерой
        public virtual bool DisconnectFromDecoder()
        {
            return false;
        }

        protected virtual void OnGlobalKeyDown(object sender, View.KeyEventArgs e)
        {

        }

        protected virtual void OnGlobalKeyUp(object sender, View.KeyEventArgs e)
        {

        }

        protected virtual void OnGlobalKeyPress(object sender, View.KeyEventArgs e)
        {

        }

        protected virtual void OnGlobalTouchEvent(object sender, View.TouchEventArgs e)
        {

        }

        protected bool _fIsQrCodeEnabled = false;
        public virtual bool IsQRCodeEnabled
        {
            get { return _fIsQrCodeEnabled; }
            set { _fIsQrCodeEnabled = value; }
        }

        /// <summary>
        /// Активировать режим сканирования сканера штрихкодов.
        /// </summary>
        public abstract void TurnOn();

        /// <summary>
        /// Отключить режим сканирования сканера штрихкодов.
        /// </summary>
		public abstract void TurnOff();

        /// <summary>
        /// Включен ли режим сканирования штрихкода.
        /// </summary>
		public abstract bool IsTurnedOn
		{
			get;
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

        /// <summary>
        /// Доступен ли для сканера штрихкодов интерфейс настроек.
        /// </summary>
        public virtual bool IsSettingsAvailable
        {
            get { return false; }
        }

        /// <summary>
        /// Показывать кнопку сканирования в интерфейсе.
        /// </summary>
        public virtual bool ShowScanButton
        {
            get { return true; }
        }

        /// <summary>
        /// Вызвать интерфейс настроек сканера штрихкодов.
        /// </summary>
        public virtual void ShowSettings()
        {
        }


        #region управление кодировками

        /// <summary>
        /// Сохранить настройки кодировок сканера штрихкодов.
        /// </summary>
        public virtual void BackupSymbologySettings()
        {
            throw new NotImplementedException("Сохранение параметров сканера для данного устройства не поддерживается.");
        }

        /// <summary>
        /// Восстановить настройки активности кодировок сканера штрихкодов.
        /// </summary>
        public virtual void RestoreSymbologySettings()
        {
            throw new NotImplementedException("Сохранение параметров сканера для данного устройства не поддерживается.");
        }


        /// <summary>
        /// Установить по-умолчанию настройки активности кодировок сканера штрихкодов.
        /// </summary>
        public virtual void EnableDefaultBarcodeTypes()
        {
            throw new NotImplementedException("Управление типами штрихкодов для данного сканера не поддерживается.");
        }

        public void EnableBarcodeType(string type)
        {
            EnableBarcodeType(type, false);
        }

        /// <summary>
        /// Разрешить указанную кодировку сканера штрихкодов.
        /// </summary>
        /// <param name="type">Имя типа разрешаемой кодировки.</param>
        /// <param name="enableOnlyThis">Разрешить только данный тип кодировки. Т.е. запретить остальные.</param>
		public void EnableBarcodeType(string type, bool enableOnlyThis)
        {
            try
            {
                BarcodeType tp = (BarcodeType)Enum.Parse(typeof(BarcodeType), type, true);
                EnableBarcodeType(tp, enableOnlyThis);
            }
            catch
            {
                //Запись в лог ошибки
            }
        }

        /// <summary>
        /// Разрешить указанную кодировку сканера штрихкодов.
        /// </summary>
        /// <param name="type">Тип разрешаемой кодировки.</param>
        /// <param name="enableOnlyThis">Разрешить только данный тип кодировки. Т.е. запретить остальные.</param>
		public virtual void EnableBarcodeType(BarcodeType type, bool enableOnlyThis)
        {
            throw new NotImplementedException("Управление типами штрихкодов для данного сканера не поддерживается.");
        }

        /// <summary>
        /// Запретить указанную кодировку сканера штрихкодов.
        /// </summary>
        /// <param name="type">Наименование типа запрещаемой кодировки.</param>
		public void DisableBarcodeType(string type)
        {
            try
            {
                BarcodeType tp = (BarcodeType)Enum.Parse(typeof(BarcodeType), type, true);
                DisableBarcodeType(tp);
            }
            catch
            {
                //Запись в лог ошибки
            }
        }

        /// <summary>
        /// Запретить указанную кодировку сканера штрихкодов.
        /// </summary>
        /// <param name="type">Тип запрещаемой кодировки.</param>
		public virtual void DisableBarcodeType(BarcodeType type)
        {
            throw new NotImplementedException("Управление типами штрихкодов для данного сканера не поддерживается.");
        }

        /// <summary>
        /// Получить список поддерживаемых кодировок сканера штрихкодов.
        /// </summary>
        /// <returns></returns>
        public virtual BarcodeType[] GetSupportedSymbologies()
        {
            return new BarcodeType[0];
        }

        #endregion

        /// <summary>
        /// Освободить ресурсы объекта сканера штрихкодов.
        /// </summary>
        public virtual void Dispose()
        {

        }
    }
}
