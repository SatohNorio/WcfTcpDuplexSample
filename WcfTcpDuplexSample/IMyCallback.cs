using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace Gsf.Samples.WCF
{
	/// <summary>
	/// サーバからコールバックするメソッドの一覧を定義します。
	/// </summary>
	public interface IMyCallback
	{
		/// <summary>
		/// クライアントと通信が確立しているか確認します。切断されていたらタイムアウトの例外がスローされます。
		/// </summary>
		[OperationContract(IsOneWay = true)]
		void WatchDog();
	}
}
