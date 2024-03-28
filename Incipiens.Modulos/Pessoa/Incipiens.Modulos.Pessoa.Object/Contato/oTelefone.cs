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

namespace Incipiens.Modulos.Pessoa.Object.Contato
{
    [DisplayName("Telefone")]
    public class oTelefone : ModelBase, IEqualityComparer<oTelefone>, IRowVersion
    {
        public override string dadosPrincipais
        {
            get { return _NumeroTelefone; }
        }

        public oTelefone()
        {
            this._PosicaoTelefone = 0;
            this._NumeroTelefone = "";
            this._Observacoes = "";
        }

        #region Propriedades

        [DisplayName("Posição Telefone")]
        [Required]

        public int _PosicaoTelefone
        {
            get { return GetValue(() => _PosicaoTelefone); }
            set { SetValue(() => this._PosicaoTelefone, value); }
        }

        [DisplayName("Telefone")]
        [MaxLength(10, ErrorMessage = "Telefone: Máximo 10 caracteres")]
        [Required(ErrorMessage = "Telefone: Campo Obrigatório")]
        [RegularExpression(@"[0-9]{10}", ErrorMessage = "Telefone Inválido.")]
        public string _NumeroTelefone
        {
            get { return GetValue(() => _NumeroTelefone); }
            set { SetValue(() => this._NumeroTelefone, value); }
        }

        [DisplayName("Observações")]
        [MaxLength(120, ErrorMessage = "Observações: Máximo 120 caracteres")]
        public string _Observacoes
        {
            get { return GetValue(() => _Observacoes); }
            set { SetValue(() => this._Observacoes, value); }
        }

        public long _VersaoLinha { get; set; }

        #region Pessoa

        public long _IdPessoa { get; set; }



        #endregion

        #endregion

        #region IEqualityComparer

        public bool Equals(oTelefone x, oTelefone y)
        {
            return 
                x._PosicaoTelefone == y._PosicaoTelefone && 
                x._IdPessoa == y._IdPessoa;
        }

        public int GetHashCode(oTelefone obj)
        {
            return (obj._IdPessoa + "/" + obj._PosicaoTelefone).GetHashCode();
        }


        #endregion

        public override void CloneDeep(ModelBase destino)
        {
            this.Clone(destino);
        }

        public static string FormataTelefone(string telefone)
        {
            string s = telefone.RemoverMascara();
            if (s.Length == 10)
                s = "(" + s.Substring(0, 2) + ")" + s.Substring(2, 4) + "-" + s.Substring(6, 4);
            return s;
        }

        public override bool isEmpty()
        {
            return string.IsNullOrWhiteSpace(_NumeroTelefone);
        }
    }
}
