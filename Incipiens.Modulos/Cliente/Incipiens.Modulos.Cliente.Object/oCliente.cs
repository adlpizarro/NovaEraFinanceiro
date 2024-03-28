using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Security.Cryptography;
using Incipiens.Base.Funcoes;
using Incipiens.Base.Model;
using Incipiens.Modulos.Pessoa.Object;
using Incipiens.Base.Model.Interfaces;
using Incipiens.Modulos.Pessoa.Object.enumerador;

namespace Incipiens.Modulos.Cliente.Object
{
    [DisplayName("Cliente")]
    public class oCliente : ModelBase, IEqualityComparer<oCliente>, IRowVersion
    {
        public oCliente()
        {
            //_DadosCliente = new oPessoaFisica();
        }

        public oCliente(enumTipoPessoa tpPessoa)
        {
            if (tpPessoa == enumTipoPessoa.Fisica)
                _DadosCliente = new oPessoaFisica();
            else if (tpPessoa == enumTipoPessoa.Juridica)
                _DadosCliente = new oPessoaJuridica();
        }

        #region Encapsulamento

        public long _IdCliente
        {
            get { return GetValue(() => _IdCliente); }
            set { SetValue(() => _IdCliente, value); }
        }

        #region DadosCliente

        public long _IdPessoa
        {
            get { return GetValue(() => _IdPessoa); }
            set { SetValue(() => _IdPessoa, value); }
        }
        [Required]
        public oPessoa _DadosCliente
        {
            get { return GetValue(() => _DadosCliente); }
            set { SetValue(() => _DadosCliente, value);
                if(_DadosCliente!=null)
                    _DadosCliente.PropertyChanged += _DadosCliente_PropertyChanged;
                NotifyPropertyChanged("dadosPrincipais");
            }
        }
        private void _DadosCliente_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged(
                informacoesPropriedade<oCliente>.GetNameProperty(
                    f => f._DadosCliente) + "." + e.PropertyName);
            if (e.PropertyName == "dadosPrincipais")
                NotifyPropertyChanged("dadosPrincipais");
             
        }
        #endregion
        public override string dadosPrincipais
        {
            get {
                if (this._DadosCliente != null)
                    if (this._DadosCliente is oPessoaFisica)
                        return ((oPessoaFisica)_DadosCliente).dadosPrincipais;
                    else
                        return ((oPessoaJuridica)_DadosCliente).dadosPrincipais;
                else
                    return "";
            } 
        }
            
        public long _VersaoLinha { get; set; }

        #endregion

        #region IEqualityComparer

        public bool Equals(oCliente x, oCliente y)
        {
            return x._IdCliente == y._IdCliente;
        }

        public int GetHashCode(oCliente obj)
        {
            return obj._IdCliente.GetHashCode();
        }

        #endregion

        public override void CloneDeep(ModelBase destino)
        {
            this.Clone(destino);
            var _destino = (oCliente)destino;

            if (this._DadosCliente is oPessoaFisica)
                _destino._DadosCliente = new oPessoaFisica();
            else
                _destino._DadosCliente = new oPessoaJuridica();

            this._DadosCliente.CloneDeep(_destino._DadosCliente);
        }

        public override bool isEmpty()
        {
            throw new NotImplementedException();
        }
    }
}
