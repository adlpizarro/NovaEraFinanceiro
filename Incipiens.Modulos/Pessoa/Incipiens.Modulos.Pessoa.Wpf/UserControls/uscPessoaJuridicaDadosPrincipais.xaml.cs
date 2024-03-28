using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace Incipiens.Modulos.Pessoa.Wpf.UserControls
{
    /// <summary>
    /// Interação lógica para uscPessoaJuridicaDadosPrincipais.xam
    /// </summary>
    public partial class uscPessoaJuridicaDadosPrincipais : UserControl
    {
        public uscPessoaJuridicaDadosPrincipais()
        {
            InitializeComponent();
        }

        public void AcionaFocus()
        {
            mskCnpj.Focus();
        }

        public BusyIndicator busyEndereco
        {
            get { return uscEndereco.busyEndereco; }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            uscEndereco.busyEndereco.FocusAfterBusy = mskCnpj;
        }

        private int vCnpjTextChanged = 0;
        private void mskCnpj_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (vCnpjTextChanged > 1)
                if (DataContext is oPessoaJuridica pj)
                    dbPessoaJuridica.BuscarPorCnpj(pj);
            vCnpjTextChanged++;
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is oPessoaFisica pj)
            {
                pj.ValidateAll();
                pj._Endereco.ValidateAll();
            }

        }

        public MaskedTextBox _mskCnpj
        {
            get { return mskCnpj; }
        }
    }
}
