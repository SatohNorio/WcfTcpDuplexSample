using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsf.Samples.WCF
{
	/// <summary>
	/// サービス内で発生しているイベントをトレースするためのクラスを定義します。このクラスはシングルトンパターンで作成されています。
	/// </summary>
	public class EventTracer : IEventTracer
	{
		// ------------------------------------------------------------------------------------------------------------
		#region コンストラクタ

		/// <summary>
		/// Gsf.Samples.WCF.EventTracer クラスの新しいインスタンスを作成します。このクラスはシングルトンパターンで作成されるため、コンストラクタから直接インスタンスを作成することはできません。
		/// </summary>
		private EventTracer()
		{
		}

		#endregion
		// ------------------------------------------------------------------------------------------------------------
		// ------------------------------------------------------------------------------------------------------------
		#region シングルトン関連情報

		/// <summary>
		/// プロセスで唯一のインスタンスを管理します。
		/// </summary>
		private static EventTracer FTracer = new EventTracer();

		/// <summary>
		/// プロセスで唯一のインスタンスを取得します。
		/// </summary>
		public static EventTracer Instance
		{
			get
			{
				return EventTracer.FTracer;
			}
		}

		#endregion
		// ------------------------------------------------------------------------------------------------------------
		// ------------------------------------------------------------------------------------------------------------
		#region イベントログ管理

		/// <summary>
		/// イベントログを実際に処理するオブジェクトを管理します。
		/// </summary>
		private IEventTracer FTraceProcesser;

		/// <summary>
		/// イベントログを実際に処理するオブジェクトを組み込みます。
		/// </summary>
		/// <param name="tracer">イベントログを実際に処理するオブジェクトを指定します。</param>
		public void Assign(IEventTracer tracer)
		{
			this.FTraceProcesser = tracer;
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
			this.FTraceProcesser.Write(msg, level, description);
		}

		/// <summary>
		/// ログをオブジェクトに書き込みます。
		/// </summary>
		/// <param name="msg">ログのメイン情報となるメッセージを指定します。</param>
		/// <param name="level">ログの重要度を表す警告レベルを指定します。</param>
		public void Write(string msg, WarningLevel level)
		{
			this.FTraceProcesser.Write(msg, level);
		}

		/// <summary>
		/// ログをオブジェクトに書き込みます。
		/// </summary>
		/// <param name="msg">ログのメイン情報となるメッセージを指定します。</param>
		public void Write(string msg)
		{
			this.FTraceProcesser.Write(msg);
		}

		#endregion
		// ------------------------------------------------------------------------------------------------------------
	}
}
