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
using Incipiens.Base.Wpf.Controllers;
using Incipiens.Base.Model;
using Incipiens.Base.Model.Interfaces;
using Incipiens.Modulos.Pessoa.Object.enumerador;

namespace Incipiens.Modulos.Pessoa.Wpf.Windows
{
    /// <summary>
    /// Lógica interna para winSelecionaTipoPessoa.xaml
    /// </summary>
    public partial class winSelecionaTipoPessoa : Window, IObjetoRetorno<oPessoa>
    {
        public winSelecionaTipoPessoa()
        {
            InitializeComponent();

            menuConfirmar.btnConfirmar.Click += BtnConfirmar_Click;
            menuConfirmar.btnVoltar.Click += BtnVoltar_Click;
        }

        public winSelecionaTipoPessoa(oPessoa oPessoa)
        {
            InitializeComponent();
            pessoaRetorno = oPessoa;
            if (oPessoa is oPessoaJuridica pj)
                new winCadastroPessoaJuridica(pj).ShowDialog();
            else if (oPessoa is oPessoaFisica pf)
                new winCadastroPessoaFisica(pf).ShowDialog();
            else
                throw new ApplicationException("oPessoa não pode ser nula");
        }

        private oPessoa pessoaRetorno;

        private void Voltar()
        {
            this.Close();
        }

        private void BtnVoltar_Click(object sender, RoutedEventArgs e)
        {
            Voltar();
        }
        private void Confirmar()
        {
            if ((enumTipoPessoa)cbxTipoPessoa.SelectedValue == enumTipoPessoa.Juridica)
            {
                pessoaRetorno = new oPessoaJuridica();
                winCadastroPessoaJuridica winPj = new winCadastroPessoaJuridica((oPessoaJuridica)pessoaRetorno);
                winPj.ShowDialog();
                this.Close();
            }
            else if ((enumTipoPessoa)cbxTipoPessoa.SelectedValue == enumTipoPessoa.Fisica)
            {
                pessoaRetorno = new oPessoaFisica();
                winCadastroPessoaFisica winPf = new winCadastroPessoaFisica((oPessoaFisica)pessoaRetorno);
                winPf.ShowDialog();
                this.Close();
            }
        }

        private void BtnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            Confirmar();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F1:
                case Key.Enter:
                    Confirmar();
                    e.Handled = true;
                    break;

                case Key.Escape:
                    Voltar();
                    e.Handled = true;
                    break;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (pessoaRetorno != null)
                this.Close();
            else
            {
                cbxTipoPessoa.SelectedIndex = 0;
                cbxTipoPessoa.Focus();
            }
        }

        public oPessoa getObjetoRetorno()
        {
            return pessoaRetorno;
        }
    }
}
