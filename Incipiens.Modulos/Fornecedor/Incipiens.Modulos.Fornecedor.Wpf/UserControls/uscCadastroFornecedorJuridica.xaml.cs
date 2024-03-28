using Incipiens.Modulos.Fornecedor.Database;
using Incipiens.Modulos.Fornecedor.Object;

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

namespace Incipiens.Modulos.Fornecedor.Wpf.UserControls
{
    /// <summary>
    /// Interação lógica para uscCadastroFornecedorJuridica.xam
    /// </summary>
    public partial class uscCadastroFornecedorJuridica : UserControl
    {
        public uscCadastroFornecedorJuridica()
        {
            InitializeComponent();

            uscPessoaJuridica._uscPessoaJuridicaDadosPrincipais._mskCnpj.TextChanged += _mskCnpj_TextChanged;
        }

        private void _mskCnpj_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataContext != null)
                dbFornecedor.BuscarPorIdPessoa((oFornecedor)DataContext);
        }
    }
}
