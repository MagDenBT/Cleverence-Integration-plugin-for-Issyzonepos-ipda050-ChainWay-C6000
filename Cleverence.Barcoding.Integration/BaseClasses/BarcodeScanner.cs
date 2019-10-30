using System;
using Cleverence.Barcoding;
using Android.Content;
using Android.Views;

namespace Cleverence.Barcoding
{

    /// <summary>
    /// ������� ����� ������� ���������� ���������� ����������.
    /// </summary>
    public abstract class BarcodeScanner : IDisposable
	{
        protected Context context;

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="context">�������-�������� ��� ���������� ���� �������.</param>
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

        // ��������� ��� Honeywell Dolphin 70e Black, (� ��� Xamarin EMDK-based ��������)
        // �.�. �� �� ����� �������� ������������
        // �� �������� � �����������
        public virtual bool ConnectToDecoder()
        {
            return false;
        }

        // ��������� ��� Honeywell Dolphin 70e Black, (� ��� Xamarin EMDK-based ��������)
        // �.�. �� �� ����� �������� ������������
        // �� �������� � �����������
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
        /// ������������ ����� ������������ ������� ����������.
        /// </summary>
        public abstract void TurnOn();

        /// <summary>
        /// ��������� ����� ������������ ������� ����������.
        /// </summary>
		public abstract void TurnOff();

        /// <summary>
        /// ������� �� ����� ������������ ���������.
        /// </summary>
		public abstract bool IsTurnedOn
		{
			get;
		}


        /// <summary>
        /// ���������� ��� �������� ������������ ���������.
        /// </summary>
		public event ScanEventHandler Scan;



        /// <summary>
        /// ��������� ������� ������������ ������� ����������.
        /// </summary>
        /// <param name="text">��������������� �����.</param>
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
        /// �������� �� ��� ������� ���������� ��������� ��������.
        /// </summary>
        public virtual bool IsSettingsAvailable
        {
            get { return false; }
        }

        /// <summary>
        /// ���������� ������ ������������ � ����������.
        /// </summary>
        public virtual bool ShowScanButton
        {
            get { return true; }
        }

        /// <summary>
        /// ������� ��������� �������� ������� ����������.
        /// </summary>
        public virtual void ShowSettings()
        {
        }


        #region ���������� �����������

        /// <summary>
        /// ��������� ��������� ��������� ������� ����������.
        /// </summary>
        public virtual void BackupSymbologySettings()
        {
            throw new NotImplementedException("���������� ���������� ������� ��� ������� ���������� �� ��������������.");
        }

        /// <summary>
        /// ������������ ��������� ���������� ��������� ������� ����������.
        /// </summary>
        public virtual void RestoreSymbologySettings()
        {
            throw new NotImplementedException("���������� ���������� ������� ��� ������� ���������� �� ��������������.");
        }


        /// <summary>
        /// ���������� ��-��������� ��������� ���������� ��������� ������� ����������.
        /// </summary>
        public virtual void EnableDefaultBarcodeTypes()
        {
            throw new NotImplementedException("���������� ������ ���������� ��� ������� ������� �� ��������������.");
        }

        public void EnableBarcodeType(string type)
        {
            EnableBarcodeType(type, false);
        }

        /// <summary>
        /// ��������� ��������� ��������� ������� ����������.
        /// </summary>
        /// <param name="type">��� ���� ����������� ���������.</param>
        /// <param name="enableOnlyThis">��������� ������ ������ ��� ���������. �.�. ��������� ���������.</param>
		public void EnableBarcodeType(string type, bool enableOnlyThis)
        {
            try
            {
                BarcodeType tp = (BarcodeType)Enum.Parse(typeof(BarcodeType), type, true);
                EnableBarcodeType(tp, enableOnlyThis);
            }
            catch
            {
                //������ � ��� ������
            }
        }

        /// <summary>
        /// ��������� ��������� ��������� ������� ����������.
        /// </summary>
        /// <param name="type">��� ����������� ���������.</param>
        /// <param name="enableOnlyThis">��������� ������ ������ ��� ���������. �.�. ��������� ���������.</param>
		public virtual void EnableBarcodeType(BarcodeType type, bool enableOnlyThis)
        {
            throw new NotImplementedException("���������� ������ ���������� ��� ������� ������� �� ��������������.");
        }

        /// <summary>
        /// ��������� ��������� ��������� ������� ����������.
        /// </summary>
        /// <param name="type">������������ ���� ����������� ���������.</param>
		public void DisableBarcodeType(string type)
        {
            try
            {
                BarcodeType tp = (BarcodeType)Enum.Parse(typeof(BarcodeType), type, true);
                DisableBarcodeType(tp);
            }
            catch
            {
                //������ � ��� ������
            }
        }

        /// <summary>
        /// ��������� ��������� ��������� ������� ����������.
        /// </summary>
        /// <param name="type">��� ����������� ���������.</param>
		public virtual void DisableBarcodeType(BarcodeType type)
        {
            throw new NotImplementedException("���������� ������ ���������� ��� ������� ������� �� ��������������.");
        }

        /// <summary>
        /// �������� ������ �������������� ��������� ������� ����������.
        /// </summary>
        /// <returns></returns>
        public virtual BarcodeType[] GetSupportedSymbologies()
        {
            return new BarcodeType[0];
        }

        #endregion

        /// <summary>
        /// ���������� ������� ������� ������� ����������.
        /// </summary>
        public virtual void Dispose()
        {

        }
    }
}
