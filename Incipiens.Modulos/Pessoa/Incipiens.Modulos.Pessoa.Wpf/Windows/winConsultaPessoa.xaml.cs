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
using System.Collections.ObjectModel;

using Incipiens.Base.Wpf.Controllers;
using Incipiens.Base.Wpf.CustomControlLibrary.Outros;
using Incipiens.Base.Funcoes;


using Incipiens.Modulos.Pessoa.Object;
using Incipiens.Modulos.Pessoa.Object.projection;
using Incipiens.Modulos.Pessoa.Database;
using Incipiens.Modulos.Pessoa.Database.projection;
using Incipiens.Base.Model;
using System.Linq.Expressions;
using System.Linq;
using Incipiens.Base.GerenciadorEF.Funcoes;
using Incipiens.Base.Model.Interfaces;
using Incipiens.Base.Model.Tipos;

namespace Incipiens.Modulos.Pessoa.Wpf.Windows
{
    /// <summary>
    /// Lógica interna para winConsultaPessoa.xaml
    /// </summary>
    public partial class winConsultaPessoa : Window, IPesquisar
    {
        ControllerWinConsulta<projectionPessoa> controllerWinConsulta;
        ControllerWinSelecao<projectionPessoa, oPessoa> controllerWinSelecao;

        public oPessoa pessoaSelecionada;
        private dbProjectionPessoa vDbPessoaProjection;
        private dbPessoa vDbPessoa;
        private dbProjectionPessoa2 vDbPessoaProjection2;

        private projectionPessoa vPessoaPesquisa;

        oOrderBy<projectionPessoa> order = new oOrderBy<projectionPessoa>()
        {
            orderBy = p => p._Nome_RazaoSocial
        };

        #region Contrutores

        public void ConstrutorComum(Expression<Func<projectionPessoa, bool>> preFiltro = null)
        {
            vPessoaPesquisa = new projectionPessoa();

            InitializeComponent();

            vDbPessoa = new dbPessoa();
            vDbPessoaProjection = new dbProjectionPessoa();
            vDbPessoaProjection2 = new dbProjectionPessoa2();

            controllerWinConsulta = new ControllerWinConsulta<projectionPessoa>(
                this, dgConsultaPessoa, busyAtualizar, vDbPessoaProjection, chkMostrarSelecionados: chkMostrarInseridos, preFiltro: preFiltro, menuInferior: menuInferior);

            this.DataContext = vPessoaPesquisa;

            Pesquisar();
        }

        public ControllerWinNovo<projectionPessoa, oPessoa> createControllerNovo()
        {

            return
                new ControllerWinNovo<projectionPessoa, oPessoa>(
                    controllerWinConsulta,
                    vDbPessoa,
                    vDbPessoaProjection,
                    vDbPessoa,
                    menuConfirmarNovo.getMenuNovo,
                    winNovo: typeof(winSelecionaTipoPessoa),
                    winEditar: typeof(winSelecionaTipoPessoa),
                    iDeletar: vDbPessoa);
        }
        public ControllerWinSelecao<projectionPessoa, oPessoa> createControllerSelecao(oPessoa pessoaSelecionada)
        {
            return
               controllerWinSelecao = new ControllerWinSelecao<projectionPessoa, oPessoa>(
                    controllerWinConsulta,
                    vDbPessoaProjection,
                    vDbPessoa,
                    menuConfirmarNovo.getMenuConfirmar,
                    pessoaSelecionada);
        }


        public winConsultaPessoa(Expression<Func<projectionPessoa, bool>> preFiltro = null)
        {
            ConstrutorComum(preFiltro);

            new ControllerWinNovoSelecao<projectionPessoa, oPessoa>(
                createControllerNovo(),
                menuConfirmarNovo);
        }

        public winConsultaPessoa(oPessoa pessoaSelecionada, Expression<Func<projectionPessoa, bool>> preFiltro = null)
        {
            ConstrutorComum(preFiltro);
            new ControllerWinNovoSelecao<projectionPessoa, oPessoa>(
                    createControllerNovo(),
                    createControllerSelecao(pessoaSelecionada),
                        menuConfirmarNovo);
        }


        //Por ser abstract precisamos fazer isso
        public ControllerWinNovo<projectionPessoa, projectionPessoa> createControllerNovoSelecaoVarios()
        {
            return
                new ControllerWinNovo<projectionPessoa, projectionPessoa>(
                    controllerWinConsulta,
                    vDbPessoaProjection2,
                    vDbPessoaProjection2,
                    vDbPessoaProjection2,
                    menuConfirmarNovo.getMenuNovo,
                    winNovo: typeof(winSelecionaTipoPessoa),
                    winEditar: typeof(winSelecionaTipoPessoa));
        }

        public ControllerWinSelecao<projectionPessoa, projectionPessoa> createControllerSelecao(ObservableCollection<projectionPessoa> pessoasSelecionadas, bool inserirRepetidos = false)
        {
            return
                new ControllerWinSelecao<projectionPessoa, projectionPessoa>(
                    controllerWinConsulta,
                    vDbPessoaProjection2,
                    vDbPessoaProjection2,
                    menuConfirmarNovo.getMenuConfirmar,
                    pessoasSelecionadas,
                    inserirRepetidos);

        }

        public winConsultaPessoa(ObservableCollection<projectionPessoa> pessoasSelecionadas, bool inserirRepetidos = false, Expression<Func<projectionPessoa, bool>> preFiltro = null)
        {
            ConstrutorComum(preFiltro);
            new ControllerWinNovoSelecao<projectionPessoa, projectionPessoa>(
                    createControllerNovoSelecaoVarios(),
                    createControllerSelecao(pessoasSelecionadas, inserirRepetidos),
                        menuConfirmarNovo);
        }

        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtIdPessoa.Focus();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (controllerWinSelecao != null)
                pessoaSelecionada = controllerWinSelecao.itemRetorno;
        }

        #region Pesquisa

        private Expression<Func<projectionPessoa, bool>> ex = null;

        
        private void TxtIdPessoa_TextChanged(object sender, TextChangedEventArgs e)
        {
            vPessoaPesquisa._Apelido_NomeFantasia = "";
            vPessoaPesquisa._Cpf_Cnpj = "";
            vPessoaPesquisa._Nome_RazaoSocial = "";
            Pesquisar();
        }

        private void TxtOutrasPesquisas_TextChanged(object sender, TextChangedEventArgs e)
        {
            vPessoaPesquisa._IdPessoa = 0;
            if (sender == txtNomeRazaoSocial_ApelidoNomeFantasia)
                order.orderBy = e => e._Nome_RazaoSocial;
            else if (sender == mskCnpj || sender==mskCpf)
                order.orderBy = e => e._Cpf_Cnpj;

            Pesquisar();
        }
        public bool LiberaPesquisa { get; set; }
        public void Pesquisar()
        {
            if (LiberaPesquisa)
            {
                Expression<Func<projectionPessoa, bool>> ex = null;

                var p = vPessoaPesquisa;

                if (p._IdPessoa != 0)
                    ex = end => end._IdPessoa == p._IdPessoa;
                else
                    if (
                        !String.IsNullOrEmpty(p._Apelido_NomeFantasia) ||
                        !String.IsNullOrEmpty(p._Cpf_Cnpj) ||
                        !String.IsNullOrEmpty(p._Nome_RazaoSocial))
                {
                    var search = new ExpressionsSearch<projectionPessoa>(p);
                    ex = ExpressionsSearch<projectionPessoa>.And(new Expression<Func<projectionPessoa, bool>>[]
                    {
                        search.Like(p => p._Nome_RazaoSocial),
                        search.Like(p=> p._Apelido_NomeFantasia),
                        search.Like(p=>p._Cpf_Cnpj)
                    });
                }

                controllerWinConsulta.Atualizar(ex, order);
            }
        }

        #endregion
    }
}
