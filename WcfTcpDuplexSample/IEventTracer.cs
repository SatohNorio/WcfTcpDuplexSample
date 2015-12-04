using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsf.Samples.WCF
{
	/// <summary>
	/// イベントログを扱うインターフェースを定義します。
	/// </summary>
	public interface IEventTracer
	{
		/// <summary>
		/// ログをオブジェクトに書き込みます。
		/// </summary>
		/// <param name="iMsg">ログのメイン情報となるメッセージを指定します。</param>
		/// <param name="iLevel">ログの重要度を表す警告レベルを指定します。</param>
		/// <param name="iDescription">ログの詳細情報を指定します。</param>
		void Write(string iMsg, WarningLevel iLevel, string iDescription);

		/// <summary>
		/// ログをオブジェクトに書き込みます。
		/// </summary>
		/// <param name="iMsg">ログのメイン情報となるメッセージを指定します。</param>
		/// <param name="iLevel">ログの重要度を表す警告レベルを指定します。</param>
		void Write(string iMsg, WarningLevel iLevel);

		/// <summary>
		/// ログをオブジェクトに書き込みます。
		/// </summary>
		/// <param name="iMsg">ログのメイン情報となるメッセージを指定します。</param>
		void Write(string iMsg);
	}

	/// <summary>
	/// ログの重要度となる警告レベルを定義します。
	/// </summary>
	public enum WarningLevel
	{
		/// <summary>
		/// 重要度の低い通常のログを表します。
		/// </summary>
		Normal,

		/// <summary>
		/// 通常のログとは区別したい情報を持つログを表します。
		/// </summary>
		Information,

		/// <summary>
		/// 好ましくない動作をしたときに通知するためのログを表します。
		/// </summary>
		Notice,

		/// <summary>
		/// アプリケーションの動作は続行できますが、深刻な問題が発生した時に出力するログを表します。
		/// </summary>
		Warning,

		/// <summary>
		/// アプリケーションが正常な状態ではなくなってしまう、致命的な問題が発生した時に出力するログを表します。
		/// </summary>
		Error,
	}
}
