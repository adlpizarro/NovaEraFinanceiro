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

using Incipiens.Modulos.Cliente.Database;
using Incipiens.Base.Wpf.Atualizador;
using Incipiens.Base.Wpf.Controllers;
using Incipiens.Modulos.Cliente.Object;
using Incipiens.Modulos.Cliente.Wpf.Windows;
using Incipiens.Modulos.Cliente.Wpf.UserControls;

using Incipiens.Modulos.Pessoa.Wpf.Windows;

namespace Teste
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            try
            {
                _Context._StringConexao = System.Configuration.ConfigurationManager.ConnectionStrings["bdPrincipal"].ConnectionString;
                winAtualizador at = new winAtualizador(new _Context());
                at.ShowDialog();
            }
            catch
            {
                Application.Current.Shutdown();
            }
            InitializeComponent();

            new ControllerDataGridIncluirRemoverComSelecaoSimples<oCliente>(
               dgConsultaPessoa, typeof(winConsultaCliente), menuRemoverIncluir);

        }

        private oCliente vCliente = new oCliente();
        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            new winConsultaCliente().ShowDialog();
        }

        private void btnConsulta_Click(object sender, RoutedEventArgs e)
        {
            new winConsultaCliente().ShowDialog();
        }

        private void btnBackup_Click(object sender, RoutedEventArgs e)
        {
            new winBackup(new _Context()).ShowDialog();
        }
    }
}
