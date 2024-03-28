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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Reflection;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

using Incipiens.Modulos.Pessoa.Object.Contato;
using Incipiens.Modulos.Pessoa.Database;
using Incipiens.Modulos.Pessoa.Object;

using Incipiens.Base.Wpf.CustomControlLibrary.Outros;

using Incipiens.Base.Model;
using Incipiens.Base.GerenciadorEF.InterfacesEntityFramework;

using Incipiens.Base.Funcoes;
using Incipiens.Base.Wpf.CustomControlLibrary.Mensagens;
using Incipiens.Base.Wpf.Controllers;
using System.Windows.Controls.Primitives;
using System.Diagnostics;

namespace Incipiens.Modulos.Pessoa.Wpf.UserControls
{
    /// <summary>
    /// Interação lógica para uscContatoDataGrid.xam
    /// </summary>
    public partial class uscContatoDataGrid : UserControl
    {
        public uscContatoDataGrid()
        {
            InitializeComponent();

            /*btnIncluir.btn.Click += BtnIncluir_Click;
            btnRemover.btn.Click += BtnRemover_Click;
            btnRemover.btn.PreviewKeyDown += Btn_PreviewKeyDown;
            btnIncluir.btn.PreviewKeyDown += Btn_PreviewKeyDown;*/
        }

        private void LstController_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    var controller = (Controller)item;
                    lstContato.Add(controller._contato);
                }
            }
            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    var controller = (Controller)item;
                    lstContato.Remove(controller._contato);
                }
            }
        }

        public ObservableCollection<oContato> lstContato;
        public ObservableCollection<Controller> lstController;

        private void dgContatos_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (dgContatos.CurrentColumn != null)
                if (dgContatos.CurrentColumn.DisplayIndex == dgContatos.Columns.Count - 1)
                {
                    if (e.Key == Key.Enter)
                    {
                        e.Handled = true;
                        AtivarDetalheContato((Controller)dgContatos.CurrentItem);
                    }
                }
        }

        public bool showParentesco = true;
        public bool showCargoFuncao = true;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            if (DataContext is ObservableCollection<oContato>)
            {
                lstContato = (ObservableCollection<oContato>)DataContext;
                lstController = new ObservableCollection<Controller>();

                foreach (var c in lstContato)
                {
                    Controller controller = new Controller(c);
                    lstController.Add(controller);
                }
                lstController.CollectionChanged += LstController_CollectionChanged;
                DataContext = lstController;
                new ControllerDataGridIncluirRemoverEditavel(dgContatos, menuIncluirRemover);
            }


            if (!showCargoFuncao)
                dgContatos.Columns[3].Visibility = Visibility.Hidden;
            if (!showParentesco)
                dgContatos.Columns[4].Visibility = Visibility.Hidden;
        }

        private void AtivarDetalheContato(Controller item)
        {

            Windows.winContatoDetalhes winDetalhes = new Windows.winContatoDetalhes(item._contato);
            winDetalhes.ShowDialog();
            item._apelido = item._contato._DadosContato._Apelido;
            item._nome = item._contato._DadosContato._Nome;
            item._cpf = item._contato._DadosContato._Cpf;
            item._cargo_funcao = item._contato._CargoFuncao;
            item._parentesco = item._contato._Parentesco;
            if (item._contato._DadosContato._Telefones.Count == 0)
                item._telefone = "";
            else
                item._telefone = item._contato._DadosContato._Telefones[0]._NumeroTelefone;

            if (item._contato._DadosContato._Celulares.Count == 0)
                item._celular = "";
            else
                item._celular = item._contato._DadosContato._Celulares[0]._NumeroCelular;

            if (item._contato._DadosContato._Emails.Count == 0)
                item._email = "";
            else
                item._email = item._contato._DadosContato._Emails[0]._Email;
        }

        private void btnDetalharContato_Click(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow row)
                    AtivarDetalheContato((Controller)row.Item);
        }
    }

    public class Controller : ModelBase
    {
        public Controller(oContato contato)
        {
            _contato = contato;
            _cpf = contato._DadosContato._Cpf;

            _nome = contato._DadosContato._Nome;
            _apelido = contato._DadosContato._Apelido;
            _cargo_funcao = contato._CargoFuncao;
            _parentesco = contato._Parentesco;
            _telefone = contato._DadosContato._Telefones.Count > 0 ? contato._DadosContato._Telefones[0]._NumeroTelefone : "";
            _celular = contato._DadosContato._Celulares.Count > 0 ? contato._DadosContato._Celulares[0]._NumeroCelular : "";
            _email = contato._DadosContato._Emails.Count > 0 ? contato._DadosContato._Emails[0]._Email : "";
            _contato.NotifyPropertyChanged("_cpf");
        }

        public Controller()
        {
            _contato = new oContato();
            NotifyPropertyChanged("_nome");
            NotifyPropertyChanged("_apelido");
        }

        #region Propriedades

        [oPessoaFisica.CpfValidation]
        [CpfIgualValidation]
        public string _cpf
        {
            get { return GetValue(() => _cpf); }
            set
            {
                _contato._DadosContato._Cpf = value;

                if (!String.IsNullOrEmpty(value))
                {
                    dbPessoaFisica.BuscarPorCpf(_contato._DadosContato);
                    
                }
                _nome = _contato._DadosContato._Nome;
                _apelido = _contato._DadosContato._Apelido;
                _parentesco = _contato._Parentesco;
                _telefone = _contato._DadosContato._Telefones.Count > 0 ? _contato._DadosContato._Telefones[0]._NumeroTelefone : "";
                _celular = _contato._DadosContato._Celulares.Count > 0 ? _contato._DadosContato._Celulares[0]._NumeroCelular : "";
                _email = _contato._DadosContato._Emails.Count > 0 ? _contato._DadosContato._Emails[0]._Email : "";
                SetValue(() => _cpf, value);
            }
        }

        [NomeApelidoValidation2]
        public string _nome
        {
            get { return GetValue(() => _nome); }
            set
            {
                SetValue(() => _nome, value);
                _contato._DadosContato._Nome = value;
                NotifyPropertyChanged(informacoesPropriedade<Controller>.GetNameProperty(c => c._apelido));
            }
        }

        [NomeApelidoValidation2]
        public string _apelido
        {
            get { return GetValue(() => _apelido); }
            set
            {
                SetValue(() => _apelido, value);
                _contato._DadosContato._Apelido = value;
                NotifyPropertyChanged(informacoesPropriedade<Controller>.GetNameProperty(c => c._nome));
            }
        }

        public string _cargo_funcao
        {
            get { return GetValue(() => _cargo_funcao); }
            set
            {
                SetValue(() => _cargo_funcao, value);
                _contato._CargoFuncao = value;
            }
        }

        public string _parentesco
        {
            get { return GetValue(() => _parentesco); }
            set
            {
                SetValue(() => _parentesco, value);
                _contato._Parentesco = value;
            }
        }

        public string _telefone
        {
            get { return GetValue(() => _telefone); }
            set
            {
                var telefones = _contato._DadosContato._Telefones;
                if (String.IsNullOrEmpty(value))
                {
                    if (telefoneAdd != null)
                        if (telefoneAdd._NumeroTelefone == _telefone)
                        {
                            telefones.Remove(telefoneAdd);
                            telefoneAdd = null;
                        }
                }
                else
                {
                    var telefoneBuscado = telefones.SingleOrDefault(t => t._NumeroTelefone == value);
                    if (telefoneBuscado == null)
                    {
                        if (telefoneAdd != null)
                            telefones.Remove(telefoneAdd);
                        telefoneAdd = new oTelefone()
                        {
                            _NumeroTelefone = value
                        };
                        telefones.Add(telefoneAdd);
                    }
                    else
                        if (telefoneAdd == null)
                        telefoneAdd = telefoneBuscado;
                }
                SetValue(() => _telefone, value);
            }
        }
        private oTelefone telefoneAdd;

        public string _celular
        {
            get { return GetValue(() => _celular); }
            set
            {
                var celulares = _contato._DadosContato._Celulares;
                if (String.IsNullOrEmpty(value))
                {
                    if (celularAdd != null)
                        if (celularAdd._NumeroCelular == _celular)
                        {
                            celulares.Remove(celularAdd);
                            celularAdd = null;
                        }
                }
                else
                {
                    var celularBuscado = celulares.SingleOrDefault(t => t._NumeroCelular == value);
                    if (celularBuscado == null)
                    {
                        if (celularAdd != null)
                            celulares.Remove(celularAdd);
                        celularAdd = new oCelular()
                        {
                            _NumeroCelular = value
                        };
                        celulares.Add(celularAdd);
                    }
                    else
                        if (celularAdd == null)
                        celularAdd = celularBuscado;
                }
                SetValue(() => _celular, value);
            }
        }
        private oCelular celularAdd;

        public string _email
        {
            get { return GetValue(() => _email); }
            set
            {
                var emails = _contato._DadosContato._Emails;
                if (String.IsNullOrEmpty(value))
                {
                    if (emailAdd != null)
                        if (emailAdd._Email == _email)
                        {
                            emails.Remove(emailAdd);
                            emailAdd = null;
                        }
                }
                else
                {
                    var emailBuscado = emails.SingleOrDefault(t => t._Email == value);
                    if (emailBuscado == null)
                    {
                        if (emailAdd != null)
                            emails.Remove(emailAdd);
                        emailAdd = new oEmail()
                        {
                            _Email = value
                        };
                        emails.Add(emailAdd);
                    }
                    else
                        if (emailAdd == null)
                        emailAdd = emailBuscado;
                }
                SetValue(() => _email, value);
            }
        }
        private oEmail emailAdd;

        public oContato _contato 
        {
            get { return GetValue(() => _contato); }
            set 
            { 
                SetValue(() => _contato, value);
                _contato._DadosContato.NotifyPropertyChanged(informacoesPropriedade<oPessoa>.GetNameProperty(c => c._InscricaoEstadual));
            }
        }

        public override string dadosPrincipais { get { return _nome; } }

        #endregion

        #region Validacao

        public class NomeApelidoValidation2 : ValidationAttribute
        {
            private static bool ok = true;

            protected override System.ComponentModel.DataAnnotations.ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var pf = (Controller)validationContext.ObjectInstance;

                if (String.IsNullOrEmpty(pf._nome) && String.IsNullOrEmpty(pf._apelido))
                    return new System.ComponentModel.DataAnnotations.ValidationResult("Nome ou Apelido devem ser preenchidos");

                if (ok)
                {
                    if (validationContext.MemberName == informacoesPropriedade<Controller>.GetNameProperty(p => p._nome))
                    {
                        ok = false;
                        pf.Validate(informacoesPropriedade<Controller>.GetNameProperty(p => p._apelido));
                        ok = true;
                    }
                    else if (validationContext.MemberName == informacoesPropriedade<Controller>.GetNameProperty(p => p._apelido))
                    {
                        ok = false;
                        pf.Validate(informacoesPropriedade<Controller>.GetNameProperty(p => p._nome));
                        ok = true;
                    }
                }
                return System.ComponentModel.DataAnnotations.ValidationResult.Success;
            }
        }

        public class CpfIgualValidation : ValidationAttribute
        {
            private static bool ok = true;

            protected override System.ComponentModel.DataAnnotations.ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var pf = (Controller)validationContext.ObjectInstance;
                if (pf._contato._DadosContato.HasError(informacoesPropriedade<oPessoaFisica>.GetNameProperty(p => p._Cpf)))
                {
                    var erro = pf._contato._DadosContato.GetError(informacoesPropriedade<oPessoaFisica>.GetNameProperty(p => p._Cpf)).SingleOrDefault();
                    return new System.ComponentModel.DataAnnotations.ValidationResult(erro);
                }
                if (ok)
                {
                    if (validationContext.MemberName == informacoesPropriedade<Controller>.GetNameProperty(p => p._cpf))
                    {
                        ok = false;
                        pf.Validate(informacoesPropriedade<Controller>.GetNameProperty(p => p._cpf));
                        ok = true;
                    }
                }
                return System.ComponentModel.DataAnnotations.ValidationResult.Success;
            }
        }

        #endregion

        public override void CloneDeep(ModelBase destino)
        {
            this.Clone(destino);

            var _destino = (Controller)destino;
            this._contato.CloneDeep(_destino._contato);
        }

        public override bool isEmpty()
        {
            throw new NotImplementedException();
        }
    }
}
