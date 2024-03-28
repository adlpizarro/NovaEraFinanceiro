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

using System.Linq.Expressions;

using Incipiens.Modulos.Endereco.Database;
using Incipiens.Modulos.Endereco.Wpf.Windows;
using Incipiens.Modulos.Endereco.Object;

using Incipiens.Modulos.Endereco.Object.projection;

using Incipiens.Base.Wpf.Atualizador;

using Incipiens.Base.Wpf.Controllers;

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
                App.Current.Shutdown();
            }

            InitializeComponent();

            new ControllerDataGridIncluirRemoverComSelecaoSimples<oEndereco>(
                dgConsultaEndereco, typeof(winConsultaEndereco), menuRemoverIncluir, true);
        }

        private void btnBackup_Click(object sender, RoutedEventArgs e)
        {
            winBackup backup = new winBackup(new _Context());
            backup.ShowDialog();
        }

        private void btnConsultaEndereco_Click(object sender, RoutedEventArgs e)
        {
            winConsultaEndereco consultaEndereco = new winConsultaEndereco();
            consultaEndereco.ShowDialog();
        }

        private oEndereco vEndereco = new oEndereco();
        private void btnBuscarEndereco_Click(object sender, RoutedEventArgs e)
        {
            uscCadastroEndereco.DataContext = vEndereco;
            winConsultaEndereco cons = new winConsultaEndereco(vEndereco);
            cons.ShowDialog();
        }

        private void btnConsultaEnderecoNovo_Click(object sender, RoutedEventArgs e)
        {
            winConsultaEndereco cons = new winConsultaEndereco();
            cons.ShowDialog();
        }

        private void btnConsultaGrupoUsuario_Click(object sender, RoutedEventArgs e)
        {
            /*winConsultaGrupoUsuarios winConsultaGrupoUsuarios = new winConsultaGrupoUsuarios();
            winConsultaGrupoUsuarios.ShowDialog();*/
        }
    }
}
