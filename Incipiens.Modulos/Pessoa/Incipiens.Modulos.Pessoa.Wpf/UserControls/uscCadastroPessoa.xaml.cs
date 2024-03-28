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
    /// Interação lógica para uscCadastroPessoa.xam
    /// </summary>
    public partial class uscCadastroPessoa : UserControl
    {
        public uscCadastroPessoa()
        {
            InitializeComponent();
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (grd.Children.Count > 0)
                grd.Children.RemoveAt(0);
            if (e.NewValue is oPessoaFisica pf)
            {
                uscCadastroPessoaFisica uscPessoaFisica = new uscCadastroPessoaFisica();
                uscPessoaFisica.DataContext = pf;
                grd.Children.Add(uscPessoaFisica);
            }
            else if (e.NewValue is oPessoaJuridica pj)
            {
                uscCadastroPessoaJuridica uscPessoaJuridica = new uscCadastroPessoaJuridica();
                uscPessoaJuridica.DataContext = pj;
                grd.Children.Add(uscPessoaJuridica);
            }
        }
    }
}
