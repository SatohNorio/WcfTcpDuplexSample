using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Gsf.Samples.WCF
{
	/* << netTcpBinding を使用したWCF >>
	 * 
	 * 1. WCFサービスのクラスとインターフェース定義
	 *    「WCFサービスライブラリ」のプロジェクトを追加する。この中にWCFで使用するインターフェースと実装を追加し、ライブラリを作成する。
	 *    WCFを使用するのに必要なサービス参照は下記の二つ。
	 *    
	 *    1) System.ServiceModel : WCFのサービス（公開するメソッドををまとめたクラス）を定義する ServiceContract を使用するために必要。
	 *    2) System.Runtime.Serialization : WCFでデータをやり取りするためのクラスを定義する DataContract を定義するために必要。
	 * 
	 * 2. ServiceContract のインターフェースを定義（このファイルを参考にする。）
	 *    netTcpBinding ではサーバからのコールバックを行うことができる。
	 *    使用するためにはコールバック用のメソッドをまとめたインターフェースを定義する。（IMyCallback.cs を参考にする。）
	 * 
	 * 3. サービスの実装定義（MyService.cs を参考にする。）
	 *    サービス側ではコールバックを使用するため、ServiceBehaviorAttribute の UseSynchronizationContext を false に指定し、
	 *    ConcurrencyMode を ConcurrencyMode.Reentrant に指定する。
	 * 
	 * 4. サーバ側 App.config の設定
	 *    App.config にあらかじめ設定を記述することで、コード量を短くし、設定を外部から動的に変更することが可能となる。
	 *    ソリューションエクスプローラーの App.config で右クリックし、「WCF構成の編集」を行うとGUIで設定を行うことができる。
	 *    netTcpBinding で通信を行うためには、最低限サーバ側のエンドポイントの設定があればよい。
	 * 
	 *    * 参考URL *
	 *    つくりんぐ : http://blog.valeur3.com/?p=1174
	 * 
	 *    <endpoint> 要素 の address は、baseAddress を設定すると相対パスを使用することができる。
	 *    ただし、クライアントからサービス参照を行う場合はフルパスで指定が必要。
	 *    binding には netTcpBinding を選択する。
	 *    bindingConfiguration は必須ではないが、クライアントからサービス参照を行う場合は必要。
	 *    name は任意でよい。contract はサービスのインターフェースをフルパスで指定する。
	 *    
	 *    <service> 要素の behaviorConfiguration は省略可能。
	 *    ただし、クライアントからサービス参照を行う場合は必要。
	 *    name はサービスを実装したクラスをフルパスで指定する。
	 * 
	 * 5. サービス参照を使用する場合
	 *    クライアント側でサービス参照を使用して定義を読み込む場合、サーバ側のプロセスを起動して、http 経由で構成を取得する必要がある。
	 *    そのためには、サーバ側の App.config で下記の設定が必要。
	 * 
	 *    1) <bindings> 要素の追加
	 *       GUI では、[構成]-[バインド]から設定する。
	 *       <security mode="None" /> に変更する。
	 * 
	 *    2) <behaviors> 要素の追加
	 *       GUI では、[構成]-[詳細設定]-[サービス動作]から設定する。
	 *       <serviceMetadata> 要素 : httpGetEnabled を true に設定し、httpGetUrl を設定する。
	 *       httpGetUrl には http でアクセスする URI を指定する。 ポートはサービスのポートとは別にする必要がある。
	 *       <serviceDebug> 要素 : includeExceptionDetailInFaults を false に設定する。
	 * 
	 *    3) http アクセス用のエンドポイントの追加
	 *       address はサービスと同じアドレスに /mex を付加するのが慣例となっているようだ。
	 *       binding は mexTcpBinding を指定する。名前は任意でよい。
	 *       contract は IMetadataExchange を指定する。
	 * 
	 *    4) bindingConfiguration の設定
	 *       WCF の通信を実際に行う <endpoint> 要素の bindingConfiguration に 1) で作成した binding を指定する。 
	 * 
	 *    5) behaviorConfiguration の指定
	 *       <service> 要素の behaviorConfiguration に 2) で作成した behavior を指定する。
	 *       
	 *    サービス参照を利用可能にした場合は、管理者権限で起動する必要がある。
	 *    VisualStudio からデバッグ等で開始したい場合は VisualStudio を管理者権限で起動する。
	 *    管理者権限で起動したくない場合は、下記の二ヶ所を変更し、サービス参照を使用しないようにする。
	 * 
	 *    1) mexTcpBinding を使用したエンドポイントをコメントアウトする。
	 *    2) <service> 要素の behaviorConfiguration を空欄にする。
	 *    
	 * 6. クライアント側の設定
	 *    クライアント側の App.config はサービス参照を追加すると自動で変更される。
	 *    サービス参照の追加は下記の手順で行う。
	 * 
	 *    1) ソリューションエクスプローラーの「参照」を右クリックし、「サービス参照の追加...」を選択する。
	 *    2) アドレスに 5.-2) で設定した behavior の httpGetUrl 属性の URL を入力し、任意の名前空間を設定して【OK】を押す。
	 *
	 *    以上でサービス参照が自動で追加される。
	 * 
	 * 7. サービス参照の追加でエラーになる場合
	 *    サービス参照の追加でエラーになる場合は、下記を確認する。
	 * 
	 *    1) プロキシサーバを使用しないようになっているか確認する。
	 *    2) ファイアウォールを無効にして確認する。（サーバ側、クライアント側両方）
	 *    3) behavior の httpGetUrl 属性の URL が実際の PC の IP アドレスと合っているか確認する。
	 * 
	 * 8. その他のトラブルシューティング
	 *    Q. DataContract 属性を指定したのに、サービス参照で取得したクラスの中に入っていない。
	 *    A. DataContract の名前空間が System.Runtime.Serialization になっているか確認する。
	 *       他の名前空間に 同名のクラスがあり、衝突することがある。
	 * 
	 *    Q. 起動時にサービスプロジェクトの WCF サービスホスト が起動するのがうざい。
	 *    A. サービスプロジェクトのプロパティから[WCF オプション]を選択し、
	 *       「同じソリューション内での別のプロジェクトのデバッグ時に WCF サービス ホストを開始する」のチェックを外す。
	 */

	/// <summary>
	/// WCFデータサービスのインターフェースを定義します。
	/// </summary>
	[ServiceContract(Namespace = "http://Gsf.Samples.WCF", CallbackContract = typeof(IMyCallback))]
	public interface IMyService
	{
		/// <summary>
		/// WCFサービスを初期化します。
		/// </summary>
		/// <param name="info">通信しているクライアントの情報オブジェクトを指定します。</param>
		[OperationContract(IsOneWay = true)]
		void Initialize(ClientInfo info);

		/// <summary>
		/// コールバック呼び出し処理を開始します。
		/// </summary>
		[OperationContract(IsOneWay = true)]
		void StartCallback();

		/// <summary>
		/// コールバック呼び出し処理を停止します。
		/// </summary>
		[OperationContract(IsOneWay = true)]
		void StopCallback();

		/// <summary>
		/// 次のオーダIDを採番して取得します。
		/// </summary>
		/// <returns>取得したオーダIDを返します。</returns>
		[OperationContract(IsOneWay = false)]
		int GetNextOrderId();

		/// <summary>
		/// オーダ情報をデータベースに追加します。
		/// </summary>
		/// <param name="record">オーダ情報を指定します。</param>
		[OperationContract(IsOneWay = true)]
		void AddOrder(OrderRecord record);
	}

	/// <summary>
	/// クライアント情報を管理するクラスを定義します。
	/// </summary>
	[DataContract]
	public class ClientInfo
	{
		/// <summary>
		/// IPアドレス を取得します。
		/// </summary>
		[DataMember]
		public string IpAddress { get; set; }

		/// <summary>
		/// ポート番号 を取得します。
		/// </summary>
		[DataMember]
		public int Port { get; set; }
	}

	/// <summary>
	/// オーダ情報を管理するクラスを定義します。
	/// </summary>
	[DataContract]
	public class OrderRecord
	{
		/// <summary>
		/// オーダID を取得または設定します。
		/// </summary>
		[DataMember]
		public int OrderId { get; set; }

		/// <summary>
		/// ポート番号 を取得または設定します。
		/// </summary>
		[DataMember]
		public string OrderData { get; set; }
	}

}
