using Incipiens.Base.GerenciadorEF.Funcoes;
using Incipiens.Base.Wpf.Controllers;
using Incipiens.Base.Wpf.Temas.Botoes;
using Incipiens.Base.Wpf.Temas.Menus;
using Incipiens.Modulos.Endereco.Database;
using Incipiens.Modulos.Endereco.Database.projection;
using Incipiens.Modulos.Endereco.Object;
using Incipiens.Modulos.Endereco.Object.projection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;

namespace Incipiens.Modulos.Endereco.Wpf.Windows
{
    /// <summary>
    /// Lógica interna para winConsultaMunicipio.xaml
    /// </summary>
    public partial class winConsultaMunicipio : Window, IPesquisar
    {
        #region Construtores

        private projectionMunicipio vMunicipio;

        public winConsultaMunicipio(Expression<Func<projectionMunicipio, bool>> preFiltro = null)
        {
            ConstrutorComum(preFiltro);
        }

        public winConsultaMunicipio(oMunicipio municipioSelecionado, Expression<Func<projectionMunicipio, bool>> preFiltro = null)
        {
            ConstrutorComum(preFiltro);
            createControllerSelecao(municipioSelecionado);
        }

        public winConsultaMunicipio(ObservableCollection<oMunicipio> municipiosSelecionados, bool inserirRepetidos = false, Expression<Func<projectionMunicipio, bool>> preFiltro = null)
        {
            ConstrutorComum(preFiltro);
            createControllerSelecao(municipiosSelecionados, inserirRepetidos);
        }

        private ControllerWinConsulta<projectionMunicipio> controllerWinConsulta;
        private dbProjectionMunicipio vDbProjectionMunicipio;
        private dbMunicipio vDbMunicipio;

        public bool LiberaPesquisa { get; set; }

        public void ConstrutorComum(Expression<Func<projectionMunicipio, bool>> preFiltro = null)
        {
            InitializeComponent();

            vDbProjectionMunicipio = new dbProjectionMunicipio();
            vDbMunicipio = new dbMunicipio();

            controllerWinConsulta = new ControllerWinConsulta<projectionMunicipio>(
                this, dgConsultaMunicipio, busyAtualizar, vDbProjectionMunicipio, chkMostrarSelecionados: chkMostrarInseridos, preFiltro: preFiltro,menuInferior:menuInferior);

            vMunicipio = new projectionMunicipio();
            this.DataContext = vMunicipio;

            //cbxUf.ItemsSource = new dbEstadoFederal().Listar(orderBy: e => e._Uf);

            Pesquisar();
        }

        public ControllerWinSelecao<projectionMunicipio, oMunicipio> createControllerSelecao(oMunicipio municipioSelecionado)
        {
            return
                new ControllerWinSelecao<projectionMunicipio, oMunicipio>(
                    controllerWinConsulta,
                    vDbProjectionMunicipio,
                    vDbMunicipio,
                    menuConfirmar,
                    municipioSelecionado);

        }

        public ControllerWinSelecao<projectionMunicipio, oMunicipio> createControllerSelecao(ObservableCollection<oMunicipio> municipiosSelecionados, bool inserirRepetidos = false)
        {
            return
                new ControllerWinSelecao<projectionMunicipio, oMunicipio>(
                    controllerWinConsulta,
                    vDbProjectionMunicipio,
                    vDbMunicipio,
                    menuConfirmar,
                    municipiosSelecionados,
                    inserirRepetidos);
        }

        #endregion

        #region Buscas
        public void Pesquisar()
        {
            if (LiberaPesquisa != true)
                return;
            Expression<Func<projectionMunicipio, bool>> ex = null;
            var m = vMunicipio;
            if (!string.IsNullOrEmpty(m._Uf) || !string.IsNullOrEmpty(m._Nome))
            {
                var search = new ExpressionsSearch<projectionMunicipio>(m);
                ex = ExpressionsSearch<projectionMunicipio>.And(new Expression<Func<projectionMunicipio, bool>>[]
                {
                search.Like(m=> m._Nome),
                search.Like(m=> m._Uf)
                });
            }

            controllerWinConsulta.Atualizar(ex, new Base.Model.Tipos.oOrderBy<projectionMunicipio>()
            {
                orderBy = m => m._Nome
            });
        }
        #endregion

        private void cbxUf_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Pesquisar();
        }

       
        private void txtMunicipio_TextChanged(object sender, TextChangedEventArgs e)
        {
            Pesquisar();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtMunicipio.Focus();
        }
    }
}
