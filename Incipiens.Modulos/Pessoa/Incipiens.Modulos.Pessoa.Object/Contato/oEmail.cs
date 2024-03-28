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

namespace Incipiens.Modulos.Pessoa.Object.Contato
{
    [DisplayName("Email")]
    public class oEmail: ModelBase, IEqualityComparer<oEmail>, IRowVersion
    {
        public override string dadosPrincipais
        {
            get { return _Email; }
        }

        public oEmail()
        {

            _EnviarCobrancas = true;
            _EnviarDocs = true;
            _Email = "";
            _PosicaoEmail = 0;
            _Observacoes = "";
        }

        #region Propriedades

        [DisplayName("Posição Email")]
        [Required]

        public int _PosicaoEmail
        {
            get { return GetValue(() => _PosicaoEmail); }
            set { SetValue(() => _PosicaoEmail, value); }
        }

        [MaxLength(60, ErrorMessage = "Email: Máximo 60 caracteres")]
        [DisplayName("Email")]
        [RegularExpression("^([0-9a-zA-Z]([-.\\w]*[0-9a-zA-z)*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-~Z]{2,9})$", ErrorMessage = "Email Inválido.")]
        [Required(ErrorMessage = "Email: Inválido", AllowEmptyStrings = false)]
        public string _Email
        {
            get { return GetValue(() => _Email); }
            set { SetValue(() => _Email, value); }
        }

        [Required(ErrorMessage = "Enviar Cobrança: Campo Obrigatório")]
        [DisplayName("Enviar Cobrança")]
        public bool _EnviarCobrancas
        {
            get { return GetValue(() => _EnviarCobrancas); }
            set { SetValue(() => _EnviarCobrancas, value); }
        }

        [Required(ErrorMessage = "Enviar Docs: Campo Obrigatório")]
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

        #region Pessoa

        public long _IdPessoa { get; set; }


        #endregion

        public long _VersaoLinha { get; set; }

        #endregion

        #region IEqualityComparer

        public bool Equals(oEmail x, oEmail y)
        {
            return 
                x._PosicaoEmail == y._PosicaoEmail &&
                x._IdPessoa == y._IdPessoa;
        }

        public int GetHashCode(oEmail obj)
        {
            return (obj._IdPessoa +"/"+ obj._PosicaoEmail).GetHashCode();
        }

        #endregion

        public override void CloneDeep(ModelBase destino)
        {
            this.Clone(destino);
        }

        public override bool isEmpty()
        {
            return string.IsNullOrWhiteSpace(_Email);
        }
    }
}
