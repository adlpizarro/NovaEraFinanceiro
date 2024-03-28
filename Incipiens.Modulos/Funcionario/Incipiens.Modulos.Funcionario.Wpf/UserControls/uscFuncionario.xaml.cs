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

using Incipiens.Modulos.Funcionario.Object;
using Incipiens.Modulos.Funcionario.Database;

using Incipiens.Modulos.Pessoa.Object;
using Incipiens.Base.Funcoes;


namespace Incipiens.Modulos.Funcionario.Wpf.UserControls
{
    /// <summary>
    /// Interação lógica para uscFuncionario.xam
    /// </summary>
    public partial class uscFuncionario : UserControl
    {
        public uscFuncionario()
        {
            InitializeComponent();

            uscPessoaFisica._uscPessoaFisicaDadosPrincipais._mskCpf.TextChanged += _mskCpf_TextChanged;
        }

        private void _mskCpf_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataContext != null)
                dbFuncionario.BuscarPorIdPessoa((oFuncionario)DataContext);
        }

    }
}
