using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Data;
using Incipiens.Modulos.Endereco.Database;
using Incipiens.Modulos.Endereco.Object;
using System.Linq;
using System.Threading;
using Incipiens.Base.Funcoes;
using Incipiens.Base.Wpf.CustomControlLibrary.Mensagens;
using Incipiens.Base.Wpf.Controllers;

using System.ComponentModel;
using Incipiens.Base.Model;
using Serilog;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using Xceed.Wpf.Toolkit;
using Incipiens.Modulos.Endereco.Wpf.Windows;

namespace Incipiens.Modulos.Endereco.Wpf.UsersControls
{
    /// <summary>
    /// Interação lógica para uscCadastroEndereco.xam
    /// </summary>
    public partial class uscCadastroEndereco : UserControl
    {
        ControllerSugestionTextBox<oEndereco> controllerLogradouro;
        ControllerSugestionTextBox<oEndereco> controllerBairro;

        public void AcionaFocus()
        {
            mskCep.Focus();
        }

        public uscCadastroEndereco()
        { 
            InitializeComponent();

            btnBuscarCep.btn.Click += BtnCep_Click;
            busyCep.FocusAfterBusy = mskCep;

            btnBuscarMunicipio.btn.Click += BtnMunicipio_Click;
            mtbMunicipio.ModelChanged += MtbMunicipio_ModelChanged;
            TravaDestrava();
        }

        private void MtbMunicipio_ModelChanged(object sender, RoutedPropertyChangedEventArgs<ModelBase> e)
        {
            if(e.NewValue != null)
            {
                oMunicipio mun = e.NewValue as oMunicipio;
                mun.PropertyChanged += Municipio_PropertyChanged;
                TravaDestrava();
            }
        }

        private void Municipio_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == informacoesPropriedade<oMunicipio>.GetNameProperty(m => m._IdMunicipio))
            {
                var mun = sender as oMunicipio;
                controllerLogradouro = new ControllerSugestionTextBox<oEndereco>(
                        txtLogradouro, e => e._Logradouro, new dbEndereco(), e => e._IdMunicipio == mun._IdMunicipio);

                controllerBairro = new ControllerSugestionTextBox<oEndereco>(
                    txtLogradouro, e => e._Bairro, new dbEndereco(), e => e._IdMunicipio == mun._IdMunicipio);
                TravaDestrava();
            }
        }

        private void TravaDestrava()
        {
            bool trava = DataContext != null;
            if (trava)
                trava = !string.IsNullOrWhiteSpace(((oEndereco)DataContext)._Municipio._IdMunicipio);

            txtLogradouro.IsEnabled = trava;
            txtBairro.IsEnabled = trava;
            txtComplemento.IsEnabled = trava;
            txtNumero.IsEnabled = trava;
        }

        private void BtnMunicipio_Click(object sender, RoutedEventArgs e)
        {
            oEndereco endereco = (oEndereco)DataContext;
            if(endereco._Municipio == null)
                endereco._Municipio = new oMunicipio();
            winConsultaMunicipio win = new winConsultaMunicipio(endereco._Municipio);
            win.ShowDialog();
        }

        #region CarregarCep

        CancellationTokenSource tokenSourceCep = null;

        private async void BtnCep_Click(object sender, RoutedEventArgs e)
        {
            var endereco = (oEndereco)DataContext;
            busyCep.IsBusy = true;

            #region Task

            tokenSourceCep = new CancellationTokenSource(5000);
            var task = dbEndereco.BuscaCep(endereco._Cep, tokenSourceCep.Token);
            if (await task.DeixarRodando(tokenSourceCep))
            {
                if (task.Result != null)
                {
                    long _versao = endereco._VersaoLinha;
                    long idEndereco = endereco._IdEndereco;
                    task.Result.CloneDeep(endereco);
                    endereco._IdEndereco = idEndereco;
                    endereco._VersaoLinha = _versao;

                    if (String.IsNullOrEmpty(endereco._Logradouro))
                        busyCep.FocusAfterBusy = txtLogradouro;
                    else
                        busyCep.FocusAfterBusy = txtNumero;
                }
                else
                    System.Windows.MessageBox.Show("O cep não foi localizado", "Cep não encontrado", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessagesBoxPadroes.msgCarregametoNaoConcluido();

            #endregion

            busyCep.IsBusy = false;
        }

        public BusyIndicator busyEndereco
        {
            get { return busyCep; }
        }

        #endregion

    }
}