using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;

using Incipiens.Base.Wpf.CustomControlLibrary.Mensagens;

using Incipiens.Base.Wpf.CustomControlLibrary.Outros;
using Incipiens.Base.Funcoes;

using Incipiens.Modulos.Funcionario.Database;
using Incipiens.Modulos.Funcionario.Database.projection;
using Incipiens.Modulos.Funcionario.Object;
using Incipiens.Modulos.Funcionario.Object.projection;

using Incipiens.Base.Wpf.Controllers;
using Incipiens.Base.Wpf.Temas.Menus;
using Incipiens.Base.GerenciadorEF.Funcoes;
using Incipiens.Base.Funcoes;
using System.Linq;
using System.Linq.Expressions;
using Incipiens.Base.Model.Tipos;

namespace Incipiens.Modulos.Funcionario.Wpf.Windows
{
    /// <summary>
    /// Lógica interna para winConsultaFuncionario.xaml
    /// </summary>
    public partial class winConsultaFuncionario : Window, IPesquisar
    {
        ControllerWinConsulta<projectionFuncionario> controllerWinConsulta;
        ControllerWinSelecao<projectionFuncionario, oFuncionario> controllerWinSelecao;

        private oFuncionario vFuncionario;
        private dbFuncionario vDbFuncionario;
        private dbFuncionarioProjection vDbFuncionarioProjection;
        private projectionFuncionario pesquisaFuncionario;

        #region Construtores

        public void ConstrutorComum(Expression<Func<projectionFuncionario, bool>> preFiltro = null)
        {
            pesquisaFuncionario = new projectionFuncionario();
            order = new oOrderBy<projectionFuncionario>()
            {
                orderBy = f => f._Nome
            };


            InitializeComponent();

          
            vDbFuncionario = new dbFuncionario();
            vDbFuncionarioProjection = new dbFuncionarioProjection();

            controllerWinConsulta = new ControllerWinConsulta<projectionFuncionario>(
                this, dgConsultaFuncionário, busyAtualizar, vDbFuncionarioProjection,
                chkMostrarSelecionados: chkMostrarInseridos, preFiltro: preFiltro, menuInferior: menuInferior);

            this.DataContext = pesquisaFuncionario;
        }

        public ControllerWinNovo<projectionFuncionario, oFuncionario> createControllerNovo()
        {
            return
                new ControllerWinNovo<projectionFuncionario, oFuncionario>(
                    controllerWinConsulta,
                    vDbFuncionario,
                    vDbFuncionarioProjection,
                    vDbFuncionario,
                    menuConfirmarNovo.getMenuNovo,
                    winNovo: typeof(winCadastroFuncionario),
                    winEditar: typeof(winCadastroFuncionario),
                    iDeletar: vDbFuncionario);


        }

        public ControllerWinSelecao<projectionFuncionario, oFuncionario> createControllerSelecao(oFuncionario pessoaSelecionada)
        {
            return
               controllerWinSelecao = new ControllerWinSelecao<projectionFuncionario, oFuncionario>(
                    controllerWinConsulta,
                    vDbFuncionarioProjection,
                    vDbFuncionario,
                    menuConfirmarNovo.getMenuConfirmar,
                    pessoaSelecionada);

        }

        public ControllerWinSelecao<projectionFuncionario, oFuncionario> createControllerSelecao(ObservableCollection<oFuncionario> funcionariosSelecionados, bool inserirRepetidos = false)
        {
            return
                new ControllerWinSelecao<projectionFuncionario, oFuncionario>(
                    controllerWinConsulta,
                    vDbFuncionarioProjection,
                    vDbFuncionario,
                    menuConfirmarNovo.getMenuConfirmar,
                    funcionariosSelecionados,
                    inserirRepetidos);
        }

        public winConsultaFuncionario(Expression<Func<projectionFuncionario, bool>> preFiltro = null)
        {
            ConstrutorComum(preFiltro);

            new ControllerWinNovoSelecao<projectionFuncionario, oFuncionario>(
                createControllerNovo(),
                menuConfirmarNovo);
        }

        public winConsultaFuncionario(oFuncionario funcionario, Expression<Func<projectionFuncionario, bool>> preFiltro = null)
        {
            ConstrutorComum(preFiltro);
            new ControllerWinNovoSelecao<projectionFuncionario, oFuncionario>(
                createControllerNovo(),
                createControllerSelecao(funcionario),
                menuConfirmarNovo);
        }

        public winConsultaFuncionario(ObservableCollection<oFuncionario> funcionariosSelecionados, bool inserirRepetidos = false, Expression<Func<projectionFuncionario, bool>> preFiltro = null)
        {
            ConstrutorComum(preFiltro);
            new ControllerWinNovoSelecao<projectionFuncionario, oFuncionario>(
                createControllerNovo(),
                createControllerSelecao(funcionariosSelecionados, inserirRepetidos),
                menuConfirmarNovo);
        }



        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtIdFuncionario.Focus();
        }



        #region Pesquisar

        private Expression<Func<projectionFuncionario, bool>> ex = null;
        private void txtId_TextChanged(object sender, TextChangedEventArgs e)
        {
            pesquisaFuncionario._Apelido = "";
            pesquisaFuncionario._Cpf = "";
            pesquisaFuncionario._Nome = "";
            Pesquisar();
        }

        private void TxtOutrasPesquisas_TextChanged(object sender, TextChangedEventArgs e)
        {
            pesquisaFuncionario._IdFuncionario = 0;
            if (sender == txtNome)
                order.orderBy = e => e._Nome + e._Apelido;
            else if (sender == mskCpf)
                order.orderBy = e => e._Cpf;

            Pesquisar();
        }


        public bool LiberaPesquisa { get; set; }
        private oOrderBy<projectionFuncionario> order;
        public void Pesquisar()
        {

            if (LiberaPesquisa)
            {
                Expression<Func<projectionFuncionario, bool>> ex = null;

                projectionFuncionario p = (projectionFuncionario)this.DataContext;

                if (p._IdFuncionario != 0)
                    ex = end => end._IdFuncionario == p._IdFuncionario;
                else
                    if (
                        !String.IsNullOrEmpty(p._Apelido) ||
                        !String.IsNullOrEmpty(p._Cpf) ||
                        !String.IsNullOrEmpty(p._Nome))
                {
                    var search = new ExpressionsSearch<projectionFuncionario>(p);
                    ex = ExpressionsSearch<projectionFuncionario>.And(new Expression<Func<projectionFuncionario, bool>>[]
                    {
                        search.Like(p => p._Nome),
                        search.Like(p=> p._Apelido),
                        search.Like(p=>p._Cpf)
                    });
                }

                controllerWinConsulta.Atualizar(ex, order);
            }
        }
        #endregion

    }
}