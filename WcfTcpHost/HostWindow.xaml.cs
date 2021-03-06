﻿using System;
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
	public partial class HostWindow : Window, IEventTracer
	{
		// ------------------------------------------------------------------------------------------------------------
		#region コンストラクタ

		/// <summary>
		/// WcfTcpHost.HostWindow クラスの新しいインスタンスを作成します。
		/// </summary>
		public HostWindow()
		{
			InitializeComponent();

			EventTracer.Instance.Assign(this);
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
		// ------------------------------------------------------------------------------------------------------------
		#region IEventTracer インターフェース実装

		/// <summary>
		/// ログをオブジェクトに書き込みます。
		/// </summary>
		/// <param name="msg">ログのメイン情報となるメッセージを指定します。</param>
		/// <param name="level">ログの重要度を表す警告レベルを指定します。</param>
		/// <param name="description">ログの詳細情報を指定します。</param>
		public void Write(string msg, WarningLevel level, string description)
		{
			Dispatcher.Invoke(() => this.AddLog(msg + ":" + description));
		}

		/// <summary>
		/// ログをオブジェクトに書き込みます。
		/// </summary>
		/// <param name="msg">ログのメイン情報となるメッセージを指定します。</param>
		/// <param name="level">ログの重要度を表す警告レベルを指定します。</param>
		public void Write(string msg, WarningLevel level)
		{
			Dispatcher.Invoke(() => this.AddLog(msg));
		}

		/// <summary>
		/// ログをオブジェクトに書き込みます。
		/// </summary>
		/// <param name="msg">ログのメイン情報となるメッセージを指定します。</param>
		public void Write(string msg)
		{
			Dispatcher.Invoke(() => this.AddLog(msg));
		}

		/// <summary>
		/// リストボックスにメッセージを追加します。
		/// </summary>
		/// <param name="msg"></param>
		private void AddLog(string msg)
		{
			this.listBox.Items.Add(msg);
			this.scrollViewer.ScrollToBottom();
		}

		#endregion
		// ------------------------------------------------------------------------------------------------------------

		/// <summary>
		/// サービスホストのオブジェクトを管理します。
		/// </summary>
		private ServiceHost FSvcHost;

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
			}
		}
	}
}
