using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Incipiens.Base.Model;
using Incipiens.Base.Model.Interfaces;
using Incipiens.Base.Funcoes;
using System.Net.Http.Headers;

namespace Incipiens.Modulos.Pessoa.Object.Contato
{
    [DisplayName("Celular")]
    public class oCelular: ModelBase, IEqualityComparer<oCelular>, IRowVersion
    {

        public oCelular()
        {
            _WhatsApp = true;
            _EnviarCobrancas = false;
            _EnviarDocs = false;
            _Observacoes = "";
            _PosicaoCelular = 0;
            _NumeroCelular = "";
        }

        public override string dadosPrincipais
        {
            get { return _NumeroCelular; }
        }


        #region Propriedades

        [DisplayName("Posição Celular")]
        [Required]
        public int _PosicaoCelular
        {
            get { return GetValue(() => _PosicaoCelular); }
            set { SetValue(() => _PosicaoCelular, value); }
        }

        [DisplayName("Celular")]
        [RegularExpression(@"[0-9]{11}", ErrorMessage = "Celular Inválido.")]
        [MaxLength(11, ErrorMessage = "Celular: Máximo 11 caracteres")]
        [Required(ErrorMessage = "Celular: Campo Obrigatório", AllowEmptyStrings = false)]
        public string _NumeroCelular
        {
            get { return GetValue(() => _NumeroCelular); }
            set { SetValue(() => _NumeroCelular, value); }
        }

        [DisplayName("WhatsApp")]
        [Required(ErrorMessage = "WhatsApp: Campo Obrigatório")]
        public bool _WhatsApp
        {
            get { return GetValue(() => _WhatsApp); }
            set { SetValue(() => _WhatsApp, value); }
        }

        [Required(ErrorMessage = "Cobranca: Campo Obrigatório")]
        [DisplayName("Enviar Cobrança")]
        public bool _EnviarCobrancas
        {
            get { return GetValue(() => _EnviarCobrancas); }
            set { SetValue(() => _EnviarCobrancas, value); }
        }

        [Required(ErrorMessage = "Eviar Docs: Campo Obrigatório")]
        [DisplayName("Enviar Docs")]
        public bool _EnviarDocs
        {
            get { return GetValue(() => _EnviarDocs); }
            set { SetValue(() => _EnviarDocs, value); }
        }

        [DisplayName("Observações")]
        [MaxLength(120, ErrorMessage = "Observações: Máximo 120 caracteres")]
        public string _Observacoes
        {
            get { return GetValue(() => _Observacoes); }
            set { SetValue(() => _Observacoes, value); }
        }

        public long _VersaoLinha { get; set; }

        #region Pessoa

        public long _IdPessoa { get; set; }

        public oPessoa _Pessoa { get; set; }

        #endregion

        #endregion

        #region IEqualityComparer

        public bool Equals(oCelular x, oCelular y)
        {
            return 
                x._PosicaoCelular == y._PosicaoCelular &&
                x._IdPessoa == y._IdPessoa;
        }

        public int GetHashCode(oCelular obj)
        {
            return (obj._IdPessoa + "/" + obj._PosicaoCelular).GetHashCode();
        }

        #endregion

        public override void CloneDeep(ModelBase destino)
        {
            this.Clone(destino);
        }

        public static string FormataCelular(string celular)
        {
            string s = celular.RemoverMascara();
            if (s.Length == 11)
                s = "(" + s.Substring(0, 2) + ")" + s.Substring(2, 5) + "-" + s.Substring(7, 4);
            return s;
        }

        public override bool isEmpty()
        {
            return string.IsNullOrWhiteSpace(_NumeroCelular);
        }
    }
}
