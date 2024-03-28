using Incipiens.Base.Wpf.Controllers;
using Incipiens.Base.Model;
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
using System.Windows.Shapes;
using Incipiens.Base.Model.Interfaces;
using Incipiens.Modulos.Pessoa.Object.enumerador;

namespace Incipiens.Modulos.Cliente.Wpf.Windows
{
    /// <summary>
    /// Lógica interna para winSelecionaTipoPessoa.xaml
    /// </summary>
    public partial class winSelecionaTipoPessoa : Window, IObjetoRetorno<oCliente>
    {
        public winSelecionaTipoPessoa()
        {
            InitializeComponent();
            menuConfirmar.btnConfirmar.Click += BtnConfirmar_Click;
            menuConfirmar.btnVoltar.Click += BtnVoltar_Click;
        }
        public winSelecionaTipoPessoa(oCliente cliente)
        {
            InitializeComponent();
            if (cliente._DadosCliente != null)
            {
                clienteRetorno = cliente;
                if (cliente._DadosCliente is oPessoaJuridica pj)
                    new winCadastroClienteJuridica(cliente).ShowDialog();
                else if (cliente._DadosCliente is oPessoaFisica pf)
                    new winCadastroClienteFisica(cliente).ShowDialog();
                else
                    throw new ApplicationException("Cliente não pode ser nula");
            }
            else
            {
                menuConfirmar.btnConfirmar.Click += BtnConfirmar_Click;
                menuConfirmar.btnVoltar.Click += BtnVoltar_Click;
            }
        }
        private oCliente clienteRetorno;

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
                clienteRetorno = new oCliente(enumTipoPessoa.Juridica);
                winCadastroClienteJuridica winPj = new winCadastroClienteJuridica(clienteRetorno);
                winPj.ShowDialog();
                this.Close();
            }
            else if ((enumTipoPessoa)cbxTipoPessoa.SelectedValue == enumTipoPessoa.Fisica)
            {
                clienteRetorno = new oCliente(enumTipoPessoa.Fisica);
                winCadastroClienteFisica winPf = new winCadastroClienteFisica(clienteRetorno);
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
            if (clienteRetorno != null)
                this.Close();
            else
            {
                cbxTipoPessoa.SelectedIndex = 0;
                cbxTipoPessoa.Focus();
            }
        }

        public oCliente getObjetoRetorno()
        {
            return clienteRetorno;
        }
    }
}
