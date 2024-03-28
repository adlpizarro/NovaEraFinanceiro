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
using System.Windows.Shapes;

using Incipiens.Modulos.Endereco.Database;
using Incipiens.Modulos.Endereco.Object;
using Incipiens.Base.Wpf.CustomControlLibrary.Outros;
using Incipiens.Base.Wpf.CustomControlLibrary.Mensagens;
using Incipiens.Base.Wpf.Controllers;

namespace Incipiens.Modulos.Endereco.Wpf.Windows
{
    /// <summary>
    /// Lógica interna para winCadastroEndereco.xaml
    /// </summary>
    public partial class winCadastroEndereco : Window
    {
        public winCadastroEndereco(oEndereco endereco)
        {
            this.Loaded += WinCadastroEndereco_Loaded;
            InitializeComponent();
            new ControllerWinSalvar<oEndereco>(
                this,
                new dbEndereco(),
                menuSalvar,
                endereco);
        }

        private void WinCadastroEndereco_Loaded(object sender, RoutedEventArgs e)
        {
            uscCadastroEndereco.AcionaFocus();
        }
    }
}
