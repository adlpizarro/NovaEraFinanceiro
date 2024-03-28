using Incipiens.Base.Wpf.Atualizador;
using Incipiens.Base.Wpf.Controllers;
using Incipiens.Modulos.Fornecedor;
using Incipiens.Modulos.Fornecedor.Wpf.Windows;
using Incipiens.Modulos.Fornecedor.Object;
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
using Incipiens.Modulos.Fornecedor.Database;

namespace Teste
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            _Context._StringConexao = System.Configuration.ConfigurationManager.ConnectionStrings["bdPrincipal"].ConnectionString;
            winAtualizador at = new winAtualizador(new _Context());
            at.ShowDialog();

            if (!at.resultado)
                Application.Current.Shutdown();

            InitializeComponent();

            new ControllerDataGridIncluirRemoverComSelecaoSimples<oFornecedor>(
               dgConsultaPessoa, typeof(winConsultaFornecedor), menuRemoverIncluir);
        }

        private oFornecedor vCliente = new oFornecedor();
        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            new winConsultaFornecedor().ShowDialog();
        }

        private void btnConsulta_Click(object sender, RoutedEventArgs e)
        {
            new winConsultaFornecedor().ShowDialog();
        }

        private void btnBackup_Click(object sender, RoutedEventArgs e)
        {
            new winBackup(new _Context()).ShowDialog();
        }
    }
}
