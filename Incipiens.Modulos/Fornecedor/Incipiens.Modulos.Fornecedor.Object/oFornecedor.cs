using System;
using Incipiens.Modulos.Pessoa.Object;
using System.ComponentModel.DataAnnotations;
using Incipiens.Base.Model;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using Incipiens.Base.Model.Interfaces;

namespace Incipiens.Modulos.Fornecedor.Object
{
    [DisplayName("Fornecedor")]
    public class oFornecedor : ModelBase, IEqualityComparer<oFornecedor>, IRowVersion
    {
        public oFornecedor()
        {

        }

        #region Encapsulamento

        public long _IdFornecedor
        {
            get { return GetValue(() => _IdFornecedor); }
            set { SetValue(() => _IdFornecedor, value); }
        }

        #region DadosFornecedor

        public long _IdPessoa
        {
            get { return GetValue(() => _IdPessoa); }
            set { SetValue(() => _IdPessoa, value); }
        }

        [Required]
        public oPessoa _DadosFornecedor
        {
            get { return GetValue(() => _DadosFornecedor); }
            set
            {
                SetValue(() => _DadosFornecedor, value);

                if (_DadosFornecedor is oPessoaFisica pf)
                    pf.PropertyChanged += Pf_PropertyChanged;
                else if (_DadosFornecedor is oPessoaJuridica pj)
                    pj.PropertyChanged += Pj_PropertyChanged;

                this.NotifyPropertyChanged("dadosPrincipais");
            }


        }

        private void Pj_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "_RazaoSocial" || e.PropertyName == "_NomeFantasia")
                this.NotifyPropertyChanged("dadosPrincipais");
        }

        private void Pf_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "_Nome" || e.PropertyName == "_Apelido")
                this.NotifyPropertyChanged("dadosPrincipais");
        }

        #endregion

        public override string dadosPrincipais
        {
            get
            {
                if (this._DadosFornecedor != null)
                    if (this._DadosFornecedor is oPessoaFisica)
                        return ((oPessoaFisica)_DadosFornecedor).dadosPrincipais;
                    else
                        return ((oPessoaJuridica)_DadosFornecedor).dadosPrincipais;
                else
                    return "";
            }
        }

        public long _VersaoLinha { get; set; }

        #endregion


        #region IEqualityComparer

        public bool Equals(oFornecedor x, oFornecedor y)
        {
            return x._IdFornecedor == y._IdFornecedor;
        }

        public int GetHashCode(oFornecedor obj)
        {
            return obj._IdFornecedor.GetHashCode();
        }

        #endregion

        public override void CloneDeep(ModelBase destino)
        {
            this.Clone(destino);

            var _destino = (oFornecedor)destino;

            if (this._DadosFornecedor is oPessoaFisica)
                _destino._DadosFornecedor = new oPessoaFisica();
            else
                _destino._DadosFornecedor = new oPessoaJuridica();

            this._DadosFornecedor.CloneDeep(_destino._DadosFornecedor);
        }

        public override bool isEmpty()
        {
            throw new NotImplementedException();
        }
    }
}
