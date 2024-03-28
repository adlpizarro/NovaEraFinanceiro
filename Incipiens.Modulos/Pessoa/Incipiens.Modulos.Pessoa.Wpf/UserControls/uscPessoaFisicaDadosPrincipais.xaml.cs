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
using Incipiens.Modulos.Pessoa.Object;
using Incipiens.Modulos.Pessoa.Database;

using Xceed.Wpf.Toolkit;
using Incipiens.Modulos.Endereco.Object;

namespace Incipiens.Modulos.Pessoa.Wpf.UserControls
{
    /// <summary>
    /// Interação lógica para uscPessoaFisicaDadosPrincipais.xam
    /// </summary>
    public partial class uscPessoaFisicaDadosPrincipais : UserControl
    {
        public uscPessoaFisicaDadosPrincipais()
        {
            InitializeComponent();
        }

        public void AcionaFocus()
        {
            mskCpf.Focus();
        }

        public BusyIndicator busyEndereco
        {
            get { return uscEndereco.busyEndereco; }
        }

        public MaskedTextBox _mskCpf
        {
            get { return mskCpf; }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            uscEndereco.busyEndereco.FocusAfterBusy = mskCpf;
        }

        private int vCpfTextChanged = 0;

        private void mskCpf_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (vCpfTextChanged > 1)
                if (DataContext is oPessoaFisica pf)
                    dbPessoaFisica.BuscarPorCpf(pf);
            vCpfTextChanged++;
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is oPessoaFisica pf)
            {
                pf.ValidateAll();
            }
        }
    }
}
