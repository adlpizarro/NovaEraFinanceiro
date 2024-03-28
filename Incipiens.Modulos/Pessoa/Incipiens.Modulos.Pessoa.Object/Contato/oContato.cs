using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Incipiens.Base.Model;
using Incipiens.Base.Funcoes;
using Incipiens.Base.Model.Interfaces;

namespace Incipiens.Modulos.Pessoa.Object.Contato
{
    [DisplayName("Contato")]
    public class oContato: ModelBase, IEqualityComparer<oContato>, IRowVersion
    {
        public oContato()
        {
            this._PosicaoContato = 0;
            this._DadosContato = new oPessoaFisica();
            this._Parentesco = "";
            this._CargoFuncao = "";
        }


        #region Propriedades

        [DisplayName("Posição Contato")]
        [Key]
        public int _PosicaoContato
        {
            get { return GetValue(() => _PosicaoContato); }
            set { SetValue(() => _PosicaoContato, value); }
        }

        #region DadosContato

        [Required]
        public oPessoaFisica _DadosContato
        {
            get { return GetValue(() => _DadosContato); }
            set { 
                SetValue(() => _DadosContato, value);
                
            }
        }

        public long _IdPessoaContato
        {
            get { return GetValue(() => _IdPessoaContato); }
            set { SetValue(() => _IdPessoaContato, value); }
        }

        #endregion

        [DisplayName("Cargo/Função")]
        [MaxLength(60, ErrorMessage = "Cargo/Função: Máximo 60 caracteres")]
        public string _CargoFuncao
        {
            get { return GetValue(() => _CargoFuncao); }
            set { SetValue(() => _CargoFuncao, value); }
        }

        [DisplayName("Parentesco")]
        [MaxLength(60, ErrorMessage = "Parentesco: Máximo 60 caracteres")]
        public string _Parentesco
        {
            get { return GetValue(() => _Parentesco); }
            set { SetValue(() => _Parentesco, value); }
        }

        [DisplayName("Observações")]
        [MaxLength(120, ErrorMessage = "Observações: Máximo 120 caracteres")]
        public string _Observacoes
        {
            get { return GetValue(() => _Observacoes); }
            set { SetValue(() => _Observacoes, value); }
        }


        #region Pessoa

        [Display(AutoGenerateField = false)]
        public long _IdPessoa
        {
            get { return GetValue(() => _IdPessoa); }
            set { SetValue(() => _IdPessoa, value); }
        }

        #endregion

        public long _VersaoLinha { get; set; }

        public override string dadosPrincipais
        {
            get { return _DadosContato != null ? _DadosContato.dadosPrincipais : ""; }
        }

        #endregion


        #region IEqualityComparer

        public bool Equals(oContato x, oContato y)
        {
            return 
                x._PosicaoContato == y._PosicaoContato &&
                x._IdPessoa == y._IdPessoa;
        }

        public int GetHashCode(oContato obj)
        {
            return (obj._IdPessoa + "/" + obj._PosicaoContato).GetHashCode();
        }

        #endregion

        public override void CloneDeep(ModelBase destino)
        {
            this.Clone(destino);

            var _destino = (oContato)destino;
            
            this._DadosContato.cloneContato = false;
            this._DadosContato.CloneDeep(_destino._DadosContato);
        }

        public override bool isEmpty()
        {
            return _DadosContato.isEmpty();
        }
    }
}
