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
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window,ServiceReference1.IMyServiceCallback
    {
        public MainWindow()
        {
            InitializeComponent();

            // 双方向通信を行う場合、サービス側にコールバックの実装を教える必要がある。
            var context = new InstanceContext(this);
            _client = new ServiceReference1.MyServiceClient(context);
        }

        ServiceReference1.MyServiceClient _client;

        private void comboBoxKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var txt= this.comboBox.Text;
                this._client.Regist(txt);
            }
        }

        public void SendData(string name)
        {
            this.listBox.Items.Add(name);
        }
    }
}
