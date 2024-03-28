using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Incipiens.Modulos.Endereco.Database;
using System.ComponentModel;
using Incipiens.Modulos.Endereco.Object;
using Incipiens.Base.Funcoes;
using Incipiens.Base.Wpf.CustomControlLibrary.Outros;
using Incipiens.Base.Wpf.CustomControlLibrary.Mensagens;
using Incipiens.Modulos.Endereco.Object.projection;
using System.Linq.Expressions;
using Incipiens.Base.Wpf.Temas.Botoes;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;
using Incipiens.Base.Wpf.Controllers;
using Microsoft.EntityFrameworkCore;
using Incipiens.Base.Model.Tipos;

using Incipiens.Base.Wpf.Temas.Extensoes;
using Incipiens.Base.Wpf.Temas.Menus;
using Incipiens.Modulos.Endereco.Database.projection;
using Incipiens.Base.GerenciadorEF.Funcoes;
using MySql.Data.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Incipiens.Modulos.Endereco.Wpf.Windows
{
    /// <summary>
    /// Lógica interna para winConsultaEndereco.xaml
    /// </summary>
    public partial class winConsultaEndereco : Window, IPesquisar
    { 
        private projectionEndereco vEnderecoPesquisa;
        
        private ControllerWinConsulta<projectionEndereco> controllerWinConsulta;

        private dbEndereco vDbEndereco;
        private dbProjectionEndereco vDbProjectionEndereco;

        #region Construtores

        public  void ConstrutorComum(Expression<Func<projectionEndereco, bool>> preFiltro = null)
        {
            InitializeComponent();

            vDbEndereco = new dbEndereco();
            vDbProjectionEndereco = new dbProjectionEndereco();

            controllerWinConsulta = new ControllerWinConsulta<projectionEndereco>(
                this, dgConsultaEndereco, busyAtualizar, vDbProjectionEndereco, chkMostrarSelecionados: chkMostrarInseridos, preFiltro: preFiltro, menuInferior: menuInferior);

            vEnderecoPesquisa = new projectionEndereco();
            this.DataContext = vEnderecoPesquisa;           

            Pesquisar();
        }

        public ControllerWinNovo<projectionEndereco, oEndereco> createControllerNovo()
        {
            return
                new ControllerWinNovo<projectionEndereco, oEndereco>(
                    controllerWinConsulta,
                    vDbEndereco,
                    vDbProjectionEndereco,
                    vDbEndereco,
                    menuConfirmarNovo.getMenuNovo,
                    typeof(winCadastroEndereco),
                    typeof(winCadastroEndereco),
                    iDeletar: vDbEndereco);
        }

        public ControllerWinSelecao<projectionEndereco, oEndereco> createControllerSelecao(oEndereco enderecoSelecionado)
        {
            return
                new ControllerWinSelecao<projectionEndereco, oEndereco>(
                    controllerWinConsulta,
                    vDbProjectionEndereco,
                    vDbEndereco,
                    menuConfirmarNovo.getMenuConfirmar,
                    enderecoSelecionado);

        }

        public ControllerWinSelecao<projectionEndereco, oEndereco> createControllerSelecao(ObservableCollection<oEndereco> enderecosSelecionados, bool inserirRepetidos = false)
        {
            return
                new ControllerWinSelecao<projectionEndereco, oEndereco>(
                    controllerWinConsulta,
                    vDbProjectionEndereco,
                    vDbEndereco,
                    menuConfirmarNovo.getMenuConfirmar,
                    enderecosSelecionados,
                    inserirRepetidos);
        }

        public winConsultaEndereco(Expression<Func<projectionEndereco, bool>> preFiltro = null)
        {
            ConstrutorComum(preFiltro);
            new ControllerWinNovoSelecao<projectionEndereco, oEndereco>(
                createControllerNovo(),
                menuConfirmarNovo);

        }

        public winConsultaEndereco(oEndereco enderecoSelecionado, Expression<Func<projectionEndereco, bool>> preFiltro = null)
        {
            ConstrutorComum(preFiltro);

            new ControllerWinNovoSelecao<projectionEndereco, oEndereco>(
                createControllerNovo(),
                createControllerSelecao(enderecoSelecionado),
                menuConfirmarNovo);
        }

        public winConsultaEndereco(ObservableCollection<oEndereco> enderecosSelecionados, bool inserirRepetidos = false, Expression<Func<projectionEndereco, bool>> preFiltro = null)
        {
            ConstrutorComum(preFiltro);
            new ControllerWinNovoSelecao<projectionEndereco, oEndereco>(
                createControllerNovo(),
                createControllerSelecao(enderecosSelecionados, inserirRepetidos),
                menuConfirmarNovo);
        }

        public winConsultaEndereco(ObservableCollection<oEndereco> enderecosSelecionados, bool inserirRepetidos = false)
        {
            ConstrutorComum();
            new ControllerWinNovoSelecao<projectionEndereco, oEndereco>(
                createControllerNovo(),
                createControllerSelecao(enderecosSelecionados, inserirRepetidos),
                menuConfirmarNovo);
        }

            #endregion

        #region Eventos Disparadores de Busca

            private void TxtIdEndereco_TextChanged(object sender, TextChangedEventArgs e)
        {
            vEnderecoPesquisa._Bairro = "";
            vEnderecoPesquisa._Complemento = "";
            vEnderecoPesquisa._Logradouro = "";
            vEnderecoPesquisa._IdEstadoFederal = "";
            vEnderecoPesquisa._IdMunicipio = "";

            Pesquisar();
        }

        private void TxtOutrasPesquisas_TextChanged(object sender, TextChangedEventArgs e)
        {
            vEnderecoPesquisa._IdEndereco = 0;
            if (order == null)
                order = new oOrderBy<projectionEndereco>();
            if (sender == txtLogradouro)
                order.orderBy=e => e._Logradouro;
            else if (sender == txtBairro)
                order.orderBy = e => e._Bairro;
            else if (sender == txtComplemento)
                order.orderBy = e => e._Complemento;
            else if (sender == txtUF)
                order.orderBy = e => e._Uf;
            else if (sender == txtMunicipio)
                order.orderBy = e => e._Municipio;

            Pesquisar();
        }

        public bool LiberaPesquisa { get; set; }
        public oOrderBy<projectionEndereco> order;
        public void Pesquisar()
        {
            if (LiberaPesquisa)
            {
                Expression<Func<projectionEndereco, bool>> ex = null;

                var p = vEnderecoPesquisa;

                if (p._IdEndereco != 0)
                    ex = end => end._IdEndereco == p._IdEndereco;
                else
                    if (
                        !String.IsNullOrEmpty(p._Bairro) ||
                        !String.IsNullOrEmpty(p._Cep) ||
                        !String.IsNullOrEmpty(p._Complemento) ||
                        !String.IsNullOrEmpty(p._Uf) ||
                        !String.IsNullOrEmpty(p._Municipio) ||
                        !String.IsNullOrEmpty(p._Logradouro) ||
                        !String.IsNullOrEmpty(p._Numero))
                {
                    var search = new ExpressionsSearch<projectionEndereco>(p);
                    ex = ExpressionsSearch<projectionEndereco>.And(new Expression<Func<projectionEndereco, bool>>[]
                    {
                        search.Like(prj => prj._Logradouro),
                        search.Like(prj => prj._Bairro),
                        search.Like(prj => prj._Complemento),
                        search.Like(prj => prj._Uf),
                        search.Like(prj => prj._Municipio)
                    });
                }

                controllerWinConsulta.Atualizar(ex, order);
            }
        }


        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtIdEndereco.Focus();
        }
    }
}

