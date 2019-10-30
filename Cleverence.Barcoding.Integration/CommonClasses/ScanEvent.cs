using System;

namespace Cleverence.Barcoding
{
	public class ScanArgs
	{
		public ScanArgs(string txt)
		{
			this.text = txt;
		}

		string text = "";
		public string Text
		{
			get
			{
				return text;
			}
		}

		private bool handled = false;
		public bool Handled
		{
			get { return this.handled; }
			set { this.handled = value; }
		}
	}

    public delegate void ScanEventHandler(ScanArgs e);
}
