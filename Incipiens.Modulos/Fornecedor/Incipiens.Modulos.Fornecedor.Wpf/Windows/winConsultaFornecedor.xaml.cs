using Incipiens.Base.Wpf.Controllers;
using Incipiens.Modulos.Fornecedor.Database;
using Incipiens.Modulos.Fornecedor.Database.projection;
using Incipiens.Modulos.Fornecedor.Object;
using Incipiens.Modulos.Fornecedor.Object.projection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Incipiens.Base.Funcoes;
using Incipiens.Base.GerenciadorEF.Funcoes;
using Incipiens.Modulos.Fornecedor.Wpf;
using Incipiens.Base.Model.Tipos;


namespace Incipiens.Modulos.Fornecedor.Wpf.Windows
{
    /// <summary>
    /// Lógica interna para winConsultaFornecedor.xaml
    /// </summary>
    public partial class winConsultaFornecedor : Window, IPesquisar
    {
        ControllerWinConsulta<projectionFornecedor> controllerWinConsulta;
        ControllerWinSelecao<projectionFornecedor, oFornecedor> controllerWinSelecao;

        public oFornecedor fornecedorSelecionado;
        private dbFornecedorProjection vDbFornecedorProjection;
        private dbFornecedor vDbFornecedor;
        private projectionFornecedor pesquisaFornecedor;

        #region Construtores 

        public void ConstrutorComum(Expression<Func<projectionFornecedor, bool>> preFiltro = null)
        {

            order = new oOrderBy<projectionFornecedor>()
            {
                orderBy = f => f._NomeRazaoSocial
            };

            pesquisaFornecedor = new projectionFornecedor();
            InitializeComponent();

            vDbFornecedor = new dbFornecedor();
            vDbFornecedorProjection = new dbFornecedorProjection();


            controllerWinConsulta = new ControllerWinConsulta<projectionFornecedor>(
                this,
                dgConsultaCliente,
                busyAtualizar,
                vDbFornecedorProjection,
                chkMostrarSelecionados: chkMostrarInseridos,preFiltro:preFiltro,menuInferior:menuInferior);
            controllerWinConsulta.Atualizar();

            this.DataContext = pesquisaFornecedor;

            
        }

        public ControllerWinNovo<projectionFornecedor, oFornecedor> createControllerNovo(Type detalhar = null)
        {
            return
                new ControllerWinNovo<projectionFornecedor, oFornecedor>(
                    controllerWinConsulta,
                    vDbFornecedor,
                    vDbFornecedorProjection,
                    vDbFornecedor,
                    menuConfirmarNovo.getMenuNovo,
                    winNovo: typeof(winSelecionaTipoPessoa),
                    winEditar: typeof(winSelecionaTipoPessoa),
                    iDeletar: vDbFornecedor,
                    winDetalhar: detalhar
                    );
        }

        public ControllerWinSelecao<projectionFornecedor, oFornecedor> createControllerSelecao(oFornecedor pessoaSelecionada)
        {
            return
               controllerWinSelecao = new ControllerWinSelecao<projectionFornecedor, oFornecedor>(
                    controllerWinConsulta,
                    vDbFornecedorProjection,
                    vDbFornecedor,
                    menuConfirmarNovo.getMenuConfirmar,
                    pessoaSelecionada);

        }
        public winConsultaFornecedor(Expression<Func<projectionFornecedor, bool>> preFiltro = null, Type detalhar = null)
        {
            ConstrutorComum(preFiltro);

            new ControllerWinNovoSelecao<projectionFornecedor, oFornecedor>(
                createControllerNovo(detalhar),
                menuConfirmarNovo);

        }
        public winConsultaFornecedor(oFornecedor cliente, Expression<Func<projectionFornecedor, bool>> preFiltro = null, Type detalhar = null)
        {
            ConstrutorComum(preFiltro);
            new ControllerWinNovoSelecao<projectionFornecedor, oFornecedor>(
                createControllerNovo(detalhar),
                createControllerSelecao(cliente),
                menuConfirmarNovo);
        }
        
        public ControllerWinSelecao<projectionFornecedor, oFornecedor> createControllerSelecao(ObservableCollection<oFornecedor> fornecedoresSelecionados, bool inserirRepetidos = false)
        {
            return
                new ControllerWinSelecao<projectionFornecedor, oFornecedor>(
                    controllerWinConsulta,
                    vDbFornecedorProjection,
                    vDbFornecedor,
                    menuConfirmarNovo.getMenuConfirmar,
                    fornecedoresSelecionados,
                    inserirRepetidos);
        }
        public winConsultaFornecedor(ObservableCollection<oFornecedor> fornecedoresSelecionados, bool inserirRepetidos = false, Expression<Func<projectionFornecedor, bool>> preFiltro = null, Type detalhar = null)
        {
            ConstrutorComum(preFiltro);
            new ControllerWinNovoSelecao<projectionFornecedor, oFornecedor>(
                createControllerNovo(detalhar),
                createControllerSelecao(fornecedoresSelecionados, inserirRepetidos),
                menuConfirmarNovo);
        }
        #endregion


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtIdFornecedor.Focus();
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            if (controllerWinSelecao != null)
                fornecedorSelecionado = controllerWinSelecao.itemRetorno;
        }



        #region Pesquisar

        private Expression<Func<projectionFornecedor, bool>> ex = null;
        private void txtId_TextChanged(object sender, TextChangedEventArgs e)
        {
            pesquisaFornecedor._ApelidoFantasia = "";
            pesquisaFornecedor._CpfCnpj = "";
            pesquisaFornecedor._NomeRazaoSocial = "";
            Pesquisar();
        }

        private void TxtOutrasPesquisas_TextChanged(object sender, TextChangedEventArgs e)
        {
            pesquisaFornecedor._IdFornecedor = 0;
            if (sender == txtNome)
                order.orderBy = e => e._NomeRazaoSocial + e._ApelidoFantasia;
            else if (sender == mskCnpj || sender == mskCpf)
                order.orderBy = e => e._CpfCnpj;

            Pesquisar();
        }


        public bool LiberaPesquisa { get; set; }
        private oOrderBy<projectionFornecedor> order;
        public void Pesquisar()
        {

            if (LiberaPesquisa)
            {
                Expression<Func<projectionFornecedor, bool>> ex = null;

                projectionFornecedor p = (projectionFornecedor)this.DataContext;

                if (p._IdFornecedor != 0)
                    ex = end => end._IdFornecedor == p._IdFornecedor;
                else
                    if (
                        !String.IsNullOrEmpty(p._ApelidoFantasia) ||
                        !String.IsNullOrEmpty(p._CpfCnpj) ||
                        !String.IsNullOrEmpty(p._NomeRazaoSocial))
                {
                    var search = new ExpressionsSearch<projectionFornecedor>(p);
                    ex = ExpressionsSearch<projectionFornecedor>.And(new Expression<Func<projectionFornecedor, bool>>[]
                    {
                        search.Like(p => p._NomeRazaoSocial),
                        search.Like(p=> p._ApelidoFantasia),
                        search.Like(p=>p._CpfCnpj)
                    });
                }

                controllerWinConsulta.Atualizar(ex, order);
            }
        }


        #endregion
    }
}
