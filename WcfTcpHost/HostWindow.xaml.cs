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

namespace WcfTcpHost
{
	/// <summary>
	/// HostWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class HostWindow : Window
	{
		// ------------------------------------------------------------------------------------------------------------
		#region コンストラクタ

		/// <summary>
		/// WcfTcpHost.HostWindow クラスの新しいインスタンスを作成します。
		/// </summary>
		public HostWindow()
		{
			InitializeComponent();

			this.FSvcHost = new ServiceHost(typeof(MyService));
			this.FSvcHost.Open();
			this.Closed += WindowClosed;
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
			if (this.FSvcHost != null)
			{
				this.FSvcHost.Close();
			}
		}

		#endregion
		// ------------------------------------------------------------------------------------------------------------

		/// <summary>
		/// サービスホストのオブジェクトを管理します。
		/// </summary>
		private ServiceHost FSvcHost;
	}
}
