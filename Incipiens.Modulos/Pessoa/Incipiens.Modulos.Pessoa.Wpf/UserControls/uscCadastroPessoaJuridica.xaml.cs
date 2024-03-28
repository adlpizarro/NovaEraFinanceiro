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

namespace Incipiens.Modulos.Pessoa.Wpf.UserControls
{
    /// <summary>
    /// Interação lógica para uscCadastroPessoaJuridica.xam
    /// </summary>
    public partial class uscCadastroPessoaJuridica : UserControl
    {
        public uscCadastroPessoaJuridica()
        {
            InitializeComponent();
            uscContatos.showParentesco = false;
        }
        public void AcionaFocus()
        {
            uscDadosPrincipais.AcionaFocus();
        }


        public uscPessoaJuridicaDadosPrincipais _uscPessoaJuridicaDadosPrincipais
        {
            get { return uscDadosPrincipais; }
        }

        /*****************************************************/
        //Sempre as duas juntas
        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var pf = (oPessoaJuridica)this.DataContext;
            this.uscContatos.lstContato = pf._Contatos;
        }
        

        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
                tabDadosPrincipais.IsSelected = true;
        }
        /*****************************************************/
    }
}
