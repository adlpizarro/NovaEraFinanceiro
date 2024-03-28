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

using Incipiens.Base.Wpf.Controllers;
using Incipiens.Modulos.Funcionario.Object;
using Incipiens.Modulos.Funcionario.Wpf.Windows;
using Incipiens.Modulos.Funcionario.Database;
using Incipiens.Base.Wpf.Atualizador;

namespace Teste
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IPesquisar
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

            new ControllerDataGridIncluirRemoverComSelecaoSimples<oFuncionario>(
               dgConsultaPessoa, typeof(winConsultaFuncionario), menuRemoverIncluir);
        }

        private oFuncionario vFuncionario = new oFuncionario();

        public bool LiberaPesquisa { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            /*uscFuncionario.DataContext = vFuncionario;
            new winConsultaFuncionario(vFuncionario).ShowDialog();*/
        }

        private void btnConsulta_Click(object sender, RoutedEventArgs e)
        {
            new winConsultaFuncionario().ShowDialog();
        }

        private void btnBackup_Click(object sender, RoutedEventArgs e)
        {
            new winBackup(new _Context()).ShowDialog();
        }

        public void Pesquisar()
        {
            throw new NotImplementedException();
        }
    }
}
