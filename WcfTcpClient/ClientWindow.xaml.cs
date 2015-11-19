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

using Gsf.Samples.WCF;

namespace WcfTcpClient
{
	/// <summary>
	/// ClientWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class ClientWindow : Window, IMyCallback
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
			var ep = new EndpointAddress("net.tcp://192.168.150.1:8000/WCFSampleService/HelloWCF");
			this.FProxy = DuplexChannelFactory<IMyService>.CreateChannel(context, new NetTcpBinding(), ep);

			var msg = "Hello!";
			this.FProxy.Regist(msg);
			this.listBox.Items.Add(msg);
		}

		#endregion
		// ------------------------------------------------------------------------------------------------------------
		// ------------------------------------------------------------------------------------------------------------
		#region WindowClosedイベント処理

		/// <summary>
		/// プログラム終了時の処理を行います。
		/// </summary>
		/// <param name="sender">イベントを送信したオブジェクトを指定します。</param>
		/// <param name="e">イベント引数を指定します。</param>
		private void WindowClosed(object sender, EventArgs e)
		{
			this.FProxy = null;
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
				this.FProxy.Regist(txt);
			}
		}

		// ------------------------------------------------------------------------------------------------------------
		#region IMyCallbackインターフェースの実装

		/// <summary>
		/// ホストが送信したデータを受信します。
		/// </summary>
		/// <param name="name"></param>
		public void SendData(string name)
		{
			this.listBox.Items.Add(name);
		}

		#endregion
		// ------------------------------------------------------------------------------------------------------------
	}
}
