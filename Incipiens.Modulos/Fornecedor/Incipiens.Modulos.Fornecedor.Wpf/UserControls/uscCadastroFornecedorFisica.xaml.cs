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
    /// Interação lógica para uscCadastroFornecedorFisica.xam
    /// </summary>
    public partial class uscCadastroFornecedorFisica : UserControl
    {
        public uscCadastroFornecedorFisica()
        {
            InitializeComponent();

            uscPessoaFisica._uscPessoaFisicaDadosPrincipais._mskCpf.TextChanged += _mskCpf_TextChanged;
        }

        private void _mskCpf_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataContext != null)
                dbFornecedor.BuscarPorIdPessoa((oFornecedor)DataContext);

        }

        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.F2)
            {
                oFornecedor fornecedor = (oFornecedor)DataContext;
            }
               
        }
    }
}
