using Incipiens.Base.Wpf.Controllers;
using Incipiens.Base.Funcoes;
using Incipiens.Base.GerenciadorEF.Funcoes;
using Incipiens.Modulos.Cliente.Database;
using Incipiens.Modulos.Cliente.Database.projection;
using Incipiens.Modulos.Cliente.Object;
using Incipiens.Modulos.Cliente.Object.projection;
using Incipiens.Modulos.Pessoa.Wpf.Windows;
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
using Incipiens.Base.Model.Tipos;

namespace Incipiens.Modulos.Cliente.Wpf.Windows
{
    /// <summary>
    /// Lógica interna para winConsultaCliente.xaml
    /// </summary>
    public partial class winConsultaCliente : Window,IPesquisar
    {
        ControllerWinConsulta<projectionCliente> controllerWinConsulta;
        ControllerWinSelecao<projectionCliente, oCliente> controllerWinSelecao;

        public oCliente clienteSelecionado;
        private dbClienteProjection vDbClienteProjection;
        private dbCliente vDbCliente;
        public projectionCliente vClientePesquisa;

        #region Construtores 

        public void ConstrutorComum(Expression<Func<projectionCliente, bool>> preFiltro = null)
        {
            vClientePesquisa = new projectionCliente();

            order = new oOrderBy<projectionCliente>()
            {
                orderBy = p => p._NomeRazaoSocial
            };

            InitializeComponent();

            vDbCliente = new dbCliente();
            vDbClienteProjection = new dbClienteProjection();

            controllerWinConsulta = new ControllerWinConsulta<projectionCliente>(
                this,
                dgConsultaCliente,
                busyAtualizar,
                vDbClienteProjection,
                chkMostrarSelecionados: chkMostrarInseridos,preFiltro:preFiltro,menuInferior:menuInferior,orderBy:order);
            controllerWinConsulta.Atualizar();
            this.DataContext = vClientePesquisa;
        }

        public ControllerWinNovo<projectionCliente, oCliente> createControllerNovo(Type detalhar = null)
        {
            return
                new ControllerWinNovo<projectionCliente, oCliente>(
                    controllerWinConsulta,                    
                    vDbCliente,
                    vDbClienteProjection,
                    vDbCliente,
                    menuConfirmarNovo.getMenuNovo,
                    winNovo: typeof(winSelecionaTipoPessoa),
                    winEditar: typeof(winSelecionaTipoPessoa),
                    winDetalhar: detalhar,
                    iDeletar: vDbCliente); 
        }

        public ControllerWinSelecao<projectionCliente, oCliente> createControllerSelecao(oCliente pessoaSelecionada)
        {
            return
               controllerWinSelecao = new ControllerWinSelecao<projectionCliente, oCliente>(
                    controllerWinConsulta,
                    vDbClienteProjection,
                    vDbCliente,
                    menuConfirmarNovo.getMenuConfirmar,
                    pessoaSelecionada);
        }
        public winConsultaCliente(Expression<Func<projectionCliente, bool>> preFiltro = null, Type detalhar = null)
        {
            ConstrutorComum();
                new ControllerWinNovoSelecao<projectionCliente, oCliente>(
                    createControllerNovo(detalhar),
                    menuConfirmarNovo);

        }
        public winConsultaCliente(oCliente cliente, Expression<Func<projectionCliente, bool>> preFiltro = null, Type detalhar = null)
        {
            ConstrutorComum();
            new ControllerWinNovoSelecao<projectionCliente, oCliente>(
                createControllerNovo(detalhar),
                createControllerSelecao(cliente),
                menuConfirmarNovo);
        }
      
        public ControllerWinSelecao<projectionCliente, oCliente> createControllerSelecao(ObservableCollection<oCliente> clientesSelecionados, bool inserirRepetidos = false)
        {
            return
                new ControllerWinSelecao<projectionCliente, oCliente>(
                    controllerWinConsulta,
                    vDbClienteProjection,
                    vDbCliente,
                    menuConfirmarNovo.getMenuConfirmar,
                    clientesSelecionados,
                    inserirRepetidos);
        }
        public winConsultaCliente(ObservableCollection<oCliente> clientesSelecionados, bool inserirRepetidos = false, Expression<Func<projectionCliente, bool>> preFiltro = null, Type detalhar = null)
        {
            ConstrutorComum();
            new ControllerWinNovoSelecao<projectionCliente, oCliente>(
                createControllerNovo(detalhar),
                createControllerSelecao(clientesSelecionados, inserirRepetidos),
                menuConfirmarNovo);
        }

        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtIdCliente.Focus();
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            if (controllerWinSelecao != null)
                clienteSelecionado = controllerWinSelecao.itemRetorno;
        }

        #region Pesquisa

        private Expression<Func<projectionCliente, bool>> ex = null;
        private void txtIdCliente_TextChanged(object sender, TextChangedEventArgs e)
        {
            vClientePesquisa._ApelidoFantasia = "";
            vClientePesquisa._CpfCnpj = "";
            vClientePesquisa._NomeRazaoSocial = "";
            Pesquisar();
        }

        private void TxtOutrasPesquisas_TextChanged(object sender, TextChangedEventArgs e)
        {
            vClientePesquisa._IdCliente = 0;
            if (LiberaPesquisa)
            {
                if (sender == txtNome)
                    order.orderBy = e => e._NomeRazaoSocial + e._ApelidoFantasia;
                else if (sender == mskCnpj || sender == mskCpf)
                    order.orderBy = e => e._CpfCnpj;
                Pesquisar();
            }
        }
        

        public bool LiberaPesquisa { get; set; }
        private oOrderBy<projectionCliente> order;
        public void Pesquisar()
        {

            if (LiberaPesquisa)
            {
                Expression<Func<projectionCliente, bool>> ex = null;

                projectionCliente p = (projectionCliente)this.DataContext;

                if (p._IdCliente != 0)
                    ex = end => end._IdCliente == p._IdCliente;
                else
                    if (
                        !String.IsNullOrEmpty(p._ApelidoFantasia) ||
                        !String.IsNullOrEmpty(p._CpfCnpj) ||
                        !String.IsNullOrEmpty(p._NomeRazaoSocial))
                {
                    var search = new ExpressionsSearch<projectionCliente>(p);
                    ex = ExpressionsSearch<projectionCliente>.And(new Expression<Func<projectionCliente, bool>>[]
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
