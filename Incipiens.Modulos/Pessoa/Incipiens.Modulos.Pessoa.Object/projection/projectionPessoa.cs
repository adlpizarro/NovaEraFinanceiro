using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

using Incipiens.Base.Model;
using Incipiens.Base.Funcoes.Atributos;
using System.ComponentModel;

namespace Incipiens.Modulos.Pessoa.Object.projection
{
    [DisplayName("Pessoas")]
    public class projectionPessoa: ModelBase, IEqualityComparer<projectionPessoa>
    {
        public  projectionPessoa()
        {
            _Apelido_NomeFantasia = "";
            _Cpf_Cnpj = "";
            _Nome_RazaoSocial = "";
            _IdPessoa = 0;
        }

        public override string dadosPrincipais
        {
            get { return _Nome_RazaoSocial + "/" + _Apelido_NomeFantasia; }
        }

        [Grid(Position: 0, Header: "Id Pessoa", Width: "0,25*")]
        public long _IdPessoa
        {
            get { return GetValue(() => _IdPessoa); }
            set { SetValue(() => _IdPessoa, value); }
        }

        [Grid(Position: 1, Header: "CPF/CNPJ", Width: "0,3*")]
        public string _Cpf_Cnpj
        {
            get 
            {
                var valor = GetValue(() => _Cpf_Cnpj);
                if (valor == null)
                    return null;
                if (valor.Length == 14)
                    return oPessoaJuridica.FormataCnpj(valor);
                else if (valor.Length == 11)
                    return oPessoaFisica.FormataCpf(valor);
                else
                    return valor;
            }
            set { SetValue(() => _Cpf_Cnpj, value); }
        }

        [Grid(Position: 2, Header: "Nome/Razão Social", Width: "1*")]
        public string _Nome_RazaoSocial
        {
            get { return GetValue(() => _Nome_RazaoSocial); }
            set { SetValue(() => _Nome_RazaoSocial, value); }
        }

        [Grid(Position: 3, Header: "Apelido/Nome Fantasia", Width: "1*")]
        public string _Apelido_NomeFantasia
        {
            get { return GetValue(() => _Apelido_NomeFantasia); }
            set { SetValue(() => _Apelido_NomeFantasia, value); }
        }

        public bool Equals(projectionPessoa x, projectionPessoa y)
        {
            return x._IdPessoa == y._IdPessoa;
        }

        public int GetHashCode(projectionPessoa obj)
        {
            return obj._IdPessoa.GetHashCode();
        }

        public override void CloneDeep(ModelBase destino)
        {
            this.Clone(destino);
        }

        public override bool isEmpty()
        {
            return string.IsNullOrWhiteSpace(dadosPrincipais);
        }
    }
}
