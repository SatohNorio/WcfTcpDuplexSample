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
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.FSvcHost = new ServiceHost(typeof(MyService));
            this.Closed += MainWindowClosed;
        }

        private void MainWindowClosed(object sender, EventArgs e)
        {
            if (this.FSvcHost != null)
            {
                this.FSvcHost.Close();
            }
        }

        private ServiceHost FSvcHost;
    }
}
