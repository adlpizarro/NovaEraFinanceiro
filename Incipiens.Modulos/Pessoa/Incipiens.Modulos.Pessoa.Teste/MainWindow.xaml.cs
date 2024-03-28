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

using System.Collections.ObjectModel;

using Incipiens.Modulos.Pessoa.Object.projection;
using Incipiens.Modulos.Pessoa.Object;
using Incipiens.Modulos.Pessoa.Wpf.Windows;
using Incipiens.Modulos.Pessoa.Database;

using Incipiens.Base.Wpf.Controllers;
using Incipiens.Base.Wpf.Atualizador;

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
            new ControllerDataGridIncluirRemoverComSelecaoSimples<projectionPessoa>(
                    dgConsultaPessoa,
                    typeof(winConsultaPessoa),
                    menuRemoverIncluir);
        }

        private void btnBackup_Click(object sender, RoutedEventArgs e)
        {
            winBackup backup = new winBackup(new _Context());
            backup.ShowDialog();
        }

        private void btnConsultaPessoa_Click(object sender, RoutedEventArgs e)
        {
            winConsultaPessoa win = new winConsultaPessoa();
            win.ShowDialog();
        }

        private void btnBuscarPessoa_Click(object sender, RoutedEventArgs e)
        {
            oPessoa pessoa = null;
            winConsultaPessoa win = new winConsultaPessoa(pessoa);
            win.ShowDialog();
            pessoa = win.pessoaSelecionada;
            if (pessoa != null)
                uscPessoa.DataContext = pessoa;
        }

        private ObservableCollection<projectionPessoa> vLstPessoas = new ObservableCollection<projectionPessoa>();
        private void btnConsultaVariosPessoa_Click(object sender, RoutedEventArgs e)
        {
            var win = new winConsultaPessoa(vLstPessoas);
            win.ShowDialog();
        }

    }
}
