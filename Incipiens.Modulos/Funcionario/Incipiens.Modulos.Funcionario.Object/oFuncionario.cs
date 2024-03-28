using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using Incipiens.Base.Model;
using Incipiens.Modulos.Pessoa.Object.Contato;
using Incipiens.Modulos.Endereco.Object;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using Incipiens.Base.Funcoes;
using Incipiens.Modulos.Pessoa.Object;
using Incipiens.Base.Model.Interfaces;

namespace Incipiens.Modulos.Funcionario.Object
{
    [DisplayName("Funcionario")]
    public class oFuncionario : ModelBase, IEqualityComparer<oFuncionario>, IRowVersion
    {
        public oFuncionario()
        {
            _IdFuncionario = 0;
            _IdPessoa = 0;
            _DadosFuncionario = new oPessoaFisica();
        }   

        #region Propriedades

        public long _IdFuncionario
        {
            get { return GetValue(() => _IdFuncionario); }
            set { SetValue(() => _IdFuncionario, value); }
        }

        #region DadosPessoa

        public long _IdPessoa
        {
            get { return GetValue(() => _IdPessoa); }
            set { SetValue(() => _IdPessoa, value); }
        }

        public oPessoaFisica _DadosFuncionario
        {
            get { return GetValue(() => _DadosFuncionario); }
            set
            {
                SetValue(() => _DadosFuncionario, value);
                if (_DadosFuncionario != null)
                    _DadosFuncionario.PropertyChanged += _DadosFuncionario_PropertyChanged; 
                NotifyPropertyChanged("dadosPrincipais");
            }
        }

        private void _DadosFuncionario_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == informacoesPropriedade<oPessoaFisica>.GetNameProperty(p => p.dadosPrincipais))
                NotifyPropertyChanged("dadosPrincipais");
        }


        #endregion

        public long _VersaoLinha { get; set; }

        public override string dadosPrincipais { get { return _DadosFuncionario != null ? 
                    _DadosFuncionario.dadosPrincipais : this._IdFuncionario.ToString(); } }

        #endregion

        #region IEqualityComparer

        public bool Equals(oFuncionario x, oFuncionario y)
        {
            return x._IdFuncionario == y._IdFuncionario;
        }

        public int GetHashCode(oFuncionario obj)
        {
            return obj._IdFuncionario.GetHashCode();
        }

        #endregion

        public override void CloneDeep(ModelBase destino)
        {
            this.Clone(destino);

            var _destino = (oFuncionario)destino;

            _destino._DadosFuncionario = new oPessoaFisica();
            this._DadosFuncionario.CloneDeep(_destino._DadosFuncionario);
        }

        public override bool isEmpty()
        {
            return _DadosFuncionario.isEmpty();
        }
    }
}
