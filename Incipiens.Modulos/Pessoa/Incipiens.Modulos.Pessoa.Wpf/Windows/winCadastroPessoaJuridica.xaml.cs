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

using Incipiens.Modulos.Pessoa.Object;
using Incipiens.Modulos.Pessoa.Database;

using Incipiens.Base.Wpf.Controllers;

namespace Incipiens.Modulos.Pessoa.Wpf.Windows
{
    /// <summary>
    /// Lógica interna para winCadastroPessoaJuridica.xaml
    /// </summary>
    public partial class winCadastroPessoaJuridica : Window
    {
        public winCadastroPessoaJuridica(oPessoaJuridica pessoa)
        {

            this.Loaded += WinCadastroPessoaJuridica_Loaded;
            InitializeComponent();

            new ControllerWinSalvar<oPessoaJuridica>(
                this,
                new dbPessoaJuridica(),
                menuSalvar,
                pessoa);
        }

        private void WinCadastroPessoaJuridica_Loaded(object sender, RoutedEventArgs e)
        {
            uscCadastroPessoaJuridica.AcionaFocus();
        }
    }
}
