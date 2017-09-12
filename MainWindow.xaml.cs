using System;
using System.CodeDom.Compiler;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Threading;

namespace ProgressBar
{
	public partial class MainWindow : Window
	{
		private DispatcherTimer _timer = new DispatcherTimer();

		public MainWindow()
		{
			this.InitializeComponent();
			base.Activated += new EventHandler(this.MainWindow_Activated);
			base.Deactivated += new EventHandler(this.MainWindow_Deactivated);
			base.ShowInTaskbar = false;
			base.AllowsTransparency = true;
			this.UpdateProgress();
			this._timer.Tick += new EventHandler(this._timer_Tick);
			this._timer.Interval = new TimeSpan(0, 0, 30);
			this._timer.Start();
		}

		private async void _timer_Tick(object sender, EventArgs e)
		{
			await this.UpdateProgress();
		}

		private void MainWindow_Activated(object sender, EventArgs e)
		{
			int verticalOffset = Int32.Parse(ConfigurationSettings.AppSettings["verticalOffset"]);
			base.Topmost = true;
			base.Top = 10 + verticalOffset;
			base.Left = 10;
			base.Activate();
		}

		private void MainWindow_Deactivated(object sender, EventArgs e)
		{
			int verticalOffset = Int32.Parse(ConfigurationSettings.AppSettings["verticalOffset"]);

			base.Topmost = true;
			base.Top = 10 + verticalOffset;
			base.Left = 10;
		}

		protected override void OnSourceInitialized(EventArgs e)
		{
			base.OnSourceInitialized(e);
			WindowsServices.SetWindowExTransparent((new WindowInteropHelper(this)).Handle);
		}

		private async Task UpdateProgress()
		{
			try
			{
				System.Net.ServicePointManager.SecurityProtocol = 
					System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;

				HttpClient httpClient = new HttpClient();
				try
				{
					string item = ConfigurationSettings.AppSettings["url"];
					HttpResponseMessage async = await httpClient.GetAsync(item);
					string str = await async.Content.ReadAsStringAsync();
					string value = Regex.Match(str, "<div class=\"fill\"[^>]*>").Value;
					string value1 = Regex.Match(value, "width:([0-9]*)%").Groups[1].Value;
					this.progressbar.Value = double.Parse(value1);
					

					this.textblock.Text = string.Concat(value1, "%");
				}
				finally
				{
					if (httpClient != null)
					{
						((IDisposable)httpClient).Dispose();
					}
				}
			}
			catch
			{
			}
		}
	}
}