using Incipiens.Base.Funcoes;
using Incipiens.Modulos.Cliente.Database;
using Incipiens.Modulos.Cliente.Object;
using Incipiens.Modulos.Pessoa.Object;
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

namespace Incipiens.Modulos.Cliente.Wpf.UserControls
{
    /// <summary>
    /// Interação lógica para uscCadastroClienteFisica.xam
    /// </summary>
    public partial class uscCadastroClienteFisica : UserControl
    {
        public uscCadastroClienteFisica()
        {
            InitializeComponent();
            uscPessoaFisica._uscPessoaFisicaDadosPrincipais._mskCpf.TextChanged += _mskCpf_TextChanged;
        }

        private void _mskCpf_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataContext != null)
                dbCliente.BuscarPorIdPessoa((oCliente)DataContext);
        }

    }
}
