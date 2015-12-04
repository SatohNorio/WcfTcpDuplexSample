using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Diagnostics;
using System.Threading;

namespace Gsf.Samples.WCF
{
	/// <summary>
	/// WCFサービスクラスを定義します。
	/// </summary>
	/// <remarks>
	/// サービス側では、コールバックを使用するため、ServiceBehaviorAttribute の UseSynchronizationContext を false に指定し、
	/// ConcurrencyMode を ConcurrencyMode.Reentrant に指定します。
	/// </remarks>
	[ServiceBehaviorAttribute(UseSynchronizationContext = false, ConcurrencyMode = ConcurrencyMode.Reentrant)]
	public class MyService : IMyService
	{
		// ------------------------------------------------------------------------------------------------------------
		#region 初期化処理

		/// <summary>
		/// WCFサービスを初期化します。
		/// </summary>
		/// <param name="info">通信しているクライアントの情報オブジェクトを指定します。</param>
		public void Initialize(ClientInfo info)
		{
			this.FName = String.Format("{0}:{1}", info.IpAddress, info.Port);
			var tracer = EventTracer.Instance;
			tracer.Write("【WCF】" + this.FName + " が初期化しました。");
		}

		/// <summary>
		/// イベントログ用のクライアント名を管理します。
		/// </summary>
		private string FName;

		#endregion
		// ------------------------------------------------------------------------------------------------------------
		// ------------------------------------------------------------------------------------------------------------
		#region コールバック処理

		/// <summary>
		/// コールバック呼び出し処理を開始します。
		/// </summary>
		public void StartCallback()
		{
			var name = this.FName;
			var tracer = EventTracer.Instance;
			var context = OperationContext.Current;
			var callback = context.GetCallbackChannel<IMyCallback>();

			tracer.Write("【WCF】" + name + " Callback処理を開始します。");
			while (!this.FQuitRequest)
			{
				tracer.Write("【WCF】" + name + " WatchDog");
				try
				{
					callback.WatchDog();
				}
				catch (CommunicationException ex)
				{
					tracer.Write("【WCF】WatchDog 実行時にエラーが発生しました。", WarningLevel.Warning, ex.Message + ":" + ex.StackTrace);
					this.FQuitRequest = true;
				}
				Thread.Sleep(1000);
			}
			tracer.Write("【WCF】" + name + " Callback処理を終了しました。");
		}

		/// <summary>
		/// コールバックの監視を終了する要求を管理します。
		/// </summary>
		private bool FQuitRequest = false;

		/// <summary>
		/// コールバック呼び出し処理を停止します。
		/// </summary>
		public void StopCallback()
		{
			EventTracer.Instance.Write("【WCF】" + this.FName + " Callback処理を終了します。");
			this.FQuitRequest = true;
		}

		#endregion
		// ------------------------------------------------------------------------------------------------------------

		/// <summary>
		/// 次のオーダIDを採番して取得します。
		/// </summary>
		/// <returns>取得したオーダIDを返します。</returns>
		public int GetNextOrderId()
		{
			EventTracer.Instance.Write("【WCF】" + this.FName + " オーダIDを取得します。");
			return ++MyService.FOrderId;
		}

		/// <summary>
		/// オーダIDを管理します。
		/// </summary>
		private static int FOrderId = 0;

		/// <summary>
		/// オーダ情報をデータベースに追加します。
		/// </summary>
		/// <param name="record">オーダ情報を指定します。</param>
		public void AddOrder(OrderRecord record)
		{
			EventTracer.Instance.Write("【WCF】" + this.FName + " オーダ情報を追加します。");
		}
	}
}
