using Incipiens.Base.Wpf.Controllers;
using Incipiens.Base.Model;
using Incipiens.Modulos.Fornecedor.Object;
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
using System.Windows.Shapes;
using Incipiens.Base.Model.Interfaces;
using Incipiens.Modulos.Pessoa.Object.enumerador;

namespace Incipiens.Modulos.Fornecedor.Wpf.Windows
{
    /// <summary>
    /// Lógica interna para winSelecionaTipoPessoa.xaml
    /// </summary>
    public partial class winSelecionaTipoPessoa : Window, IObjetoRetorno<oFornecedor>
    {
        public winSelecionaTipoPessoa()
        {
            InitializeComponent();
            menuConfirmar.btnConfirmar.Click += BtnConfirmar_Click;
            menuConfirmar.btnVoltar.Click += BtnVoltar_Click;
        }
        public winSelecionaTipoPessoa(oFornecedor fornecedor)
        {
            InitializeComponent();
            if (fornecedor._DadosFornecedor != null)
            {
                fornecedorRetorno = fornecedor;
                if (fornecedor._DadosFornecedor is oPessoaJuridica pj)
                    new winCadastroFornecedorJuridica(fornecedor).ShowDialog();
                else if (fornecedor._DadosFornecedor is oPessoaFisica pf)
                    new winCadastroFornecedorFisica(fornecedor).ShowDialog();
                else
                    throw new ApplicationException("fornecedor não pode ser nula");
            }
            else
            {
                menuConfirmar.btnConfirmar.Click += BtnConfirmar_Click;
                menuConfirmar.btnVoltar.Click += BtnVoltar_Click;
            }
        }
        private oFornecedor fornecedorRetorno;

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
                fornecedorRetorno = new oFornecedor();
                fornecedorRetorno._DadosFornecedor = new oPessoaJuridica();
                winCadastroFornecedorJuridica winPj = new winCadastroFornecedorJuridica(fornecedorRetorno);
                winPj.ShowDialog();
                this.Close();
            }
            else if ((enumTipoPessoa)cbxTipoPessoa.SelectedValue == enumTipoPessoa.Fisica)
            {
                fornecedorRetorno = new oFornecedor();
                fornecedorRetorno._DadosFornecedor = new oPessoaFisica();
                winCadastroFornecedorFisica winPf = new winCadastroFornecedorFisica(fornecedorRetorno);
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
            if (fornecedorRetorno != null)
                this.Close();
            else
            {
                cbxTipoPessoa.SelectedIndex = 0;
                cbxTipoPessoa.Focus();
            }
        }

        public oFornecedor getObjetoRetorno()
        {
            return fornecedorRetorno;
        }
    }
}
