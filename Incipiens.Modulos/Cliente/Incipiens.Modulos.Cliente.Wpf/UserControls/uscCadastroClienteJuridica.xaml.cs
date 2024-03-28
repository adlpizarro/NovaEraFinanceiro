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

using Incipiens.Modulos.Pessoa.Wpf.UserControls;


namespace Incipiens.Modulos.Cliente.Wpf.UserControls
{
    /// <summary>
    /// Interação lógica para uscCadastroClienteJuridica.xam
    /// </summary>
    public partial class uscCadastroClienteJuridica : UserControl
    {
        public uscCadastroClienteJuridica()
        {
            InitializeComponent();
            uscPessoaJuridica._uscPessoaJuridicaDadosPrincipais._mskCnpj.TextChanged += _mskCnpj_TextChanged;
        }

        private void _mskCnpj_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataContext != null)
                dbCliente.BuscarPorIdPessoa((oCliente)DataContext);
        }
    }
}
