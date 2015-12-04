using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ServiceModel;

using WcfTcpClient.WCFSampleService;

namespace WcfTcpClient
{
	/// <summary>
	/// ClientWindow.xaml の相互作用ロジック
	/// </summary>
	[CallbackBehaviorAttribute(UseSynchronizationContext = false, ConcurrencyMode = ConcurrencyMode.Reentrant)]
	public partial class ClientWindow : Window, IMyServiceCallback
	{
		// ------------------------------------------------------------------------------------------------------------
		#region コンストラクタ

		/// <summary>
		/// WcfTcpHost.ClientWindow クラスの新しいインスタンスを作成します。
		/// </summary>
		public ClientWindow()
		{
			InitializeComponent();

			// 双方向通信を行う場合、サービス側にコールバックの実装を教える必要がある。
			var context = new InstanceContext(this);
			var proxy = new MyServiceClient(context);

			var info = new ClientInfo();
			info.IpAddress = "192.168.150.1";
			info.Port = 8000;
			proxy.Initialize(info);

			this.FProxy = proxy;
			this.AddLog("Hello");
			this.Execute("StartCallback");
		}

		#endregion
		// ------------------------------------------------------------------------------------------------------------
		// ------------------------------------------------------------------------------------------------------------
		#region 終了処理

		/// <summary>
		/// プログラム終了時の処理を行います。
		/// </summary>
		/// <param name="sender">イベントを送信したオブジェクトを指定します。</param>
		/// <param name="e">イベント引数を指定します。</param>
		private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Execute("StopCallback");
			this.FProxy = null;
		}

		/// <summary>
		/// プログラム終了時の処理を行います。
		/// </summary>
		/// <param name="sender">イベントを送信したオブジェクトを指定します。</param>
		/// <param name="e">イベント引数を指定します。</param>
		private void WindowClosed(object sender, EventArgs e)
		{
		}

		#endregion
		// ------------------------------------------------------------------------------------------------------------

		/// <summary>
		/// WCFサービスのプロキシオブジェクトを管理します。
		/// </summary>
		private IMyService FProxy;

		/// <summary>
		/// コンボボックスのキーダウンイベントを処理します。
		/// </summary>
		/// <param name="sender">イベントを送信したオブジェクトを指定します。</param>
		/// <param name="e">イベント引数を指定します。</param>
		private void comboBoxKeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				var txt = this.comboBox.Text;
				this.Execute(txt);
				this.comboBox.Text = "";
				this.comboBox.Items.Insert(0, txt);
			}
		}

		// ------------------------------------------------------------------------------------------------------------
		#region コマンド作成

		/// <summary>
		/// WCFコマンドを作成します。
		/// </summary>
		/// <param name="cmdType">コマンドを種別を表す文字列を指定します。</param>
		public void Execute(string cmdType)
		{
			var proxy = this.FProxy;
			switch (cmdType.ToLower())
			{
				case "startcallback":
					proxy.StartCallback();
					break;
				case "stopcallback":
					proxy.StopCallback();
					break;
				case "order":
					var rec = new OrderRecord();
					this.AddLog("GetNextOrderId");
					rec.OrderId = proxy.GetNextOrderId();
					this.AddLog("AddOrder");
					proxy.AddOrder(rec);
					break;
			}
		}

		#endregion
		// ------------------------------------------------------------------------------------------------------------
		// ------------------------------------------------------------------------------------------------------------
		#region IMyServiceCallback インターフェースの実装

		/// <summary>
		/// ホストとの接続を確認します。送信したデータを受信します。
		/// </summary>
		public void WatchDog()
		{
			this.AddLog("WatchDog");
		}

		#endregion
		// ------------------------------------------------------------------------------------------------------------

		/// <summary>
		/// リストボックスにメッセージを追加します。
		/// </summary>
		/// <param name="msg"></param>
		private void AddLog(string msg)
		{
			if (Application.Current != null)
			{
				var dispatcher = Application.Current.Dispatcher;
				if (dispatcher.CheckAccess())
				{
					this.listBox.Items.Add(msg);
					this.scrollViewer.ScrollToBottom();
				}
				else
				{
					dispatcher.Invoke(() => this.AddLog(msg));
				}
			}
		}
	}
}
