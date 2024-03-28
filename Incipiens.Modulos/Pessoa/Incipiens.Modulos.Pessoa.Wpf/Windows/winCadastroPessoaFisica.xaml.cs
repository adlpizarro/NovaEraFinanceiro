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

using Incipiens.Base.Wpf.Controllers;
using Incipiens.Modulos.Pessoa.Object;
using Incipiens.Modulos.Pessoa.Database;

namespace Incipiens.Modulos.Pessoa.Wpf.Windows
{
    /// <summary>
    /// Lógica interna para winCadastroPessoaFisica.xaml
    /// </summary>
    public partial class winCadastroPessoaFisica : Window
    {
        public winCadastroPessoaFisica(oPessoaFisica pessoa)
        {
            InitializeComponent();
            new ControllerWinSalvar<oPessoaFisica>(
               this,
               new dbPessoaFisica(),
               menuSalvar,
               pessoa);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            uscCadastroPessoaFisica.AcionaFocus();
        }
    }
}
