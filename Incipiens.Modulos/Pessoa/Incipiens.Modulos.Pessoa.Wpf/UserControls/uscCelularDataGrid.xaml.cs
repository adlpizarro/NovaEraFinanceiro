using System;
using System.Collections.Generic;
using System.Text;
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

using Incipiens.Modulos.Pessoa.Object.Contato;

using Incipiens.Base.Wpf.Controllers;

namespace Incipiens.Modulos.Pessoa.Wpf.UserControls
{
    /// <summary>
    /// Interação lógica para uscCelularDataGrid.xam
    /// </summary>
    public partial class uscCelularDataGrid : UserControl
    {
        public uscCelularDataGrid()
        {
            InitializeComponent();

            new ControllerDataGridIncluirRemoverEditavel(
                            dgCelulares, menuRemoverIncluir);
        }

        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.F12)
            {
                var ls = (ObservableCollection<oCelular>)dgCelulares.ItemsSource;
                foreach (var c in ls)
                    MessageBox.Show(c._Observacoes);
            }
        }
    }
}
