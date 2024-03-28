using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

using Incipiens.Base.Model;
using System.ComponentModel;
using Incipiens.Base.Funcoes.Atributos;

namespace Incipiens.Modulos.Funcionario.Object.projection
{
    [DisplayName("Funcionários")]
    public class projectionFuncionario: ModelBase, IEqualityComparer<projectionFuncionario>
    {

        #region Propriedades
        [Grid(Position: 0, Header: "Id Fornecedor", Width: "0,25*")]
        public long _IdFuncionario
        {
            get { return GetValue(() => _IdFuncionario); }
            set { SetValue(() => _IdFuncionario, value); }
        }
        [Grid(Position: 1, Header: "CPF", Width: "0,3*")]
        public string _Cpf
        {
            get { return GetValue(() => _Cpf); }
            set { SetValue(() => _Cpf, value); }
        }
        [Grid(Position: 3, Header: "Apelido", Width: "1*")]
        public string _Apelido
        {
            get { return GetValue(() => _Apelido); }
            set { SetValue(() => _Apelido, value); }
        }
        [Grid(Position: 2, Header: "Nome", Width: "1*")]
        public string _Nome
        {
            get { return GetValue(() => _Nome); }
            set { SetValue(() => _Nome, value); }
        }

        public override string dadosPrincipais { get { return _Nome + "/" + _Apelido; } }

        #endregion

        #region IEqualityComparer

        public bool Equals(projectionFuncionario x, projectionFuncionario y)
        {
            return x._IdFuncionario == y._IdFuncionario;
        }

        public int GetHashCode(projectionFuncionario obj)
        {
            return obj._IdFuncionario.GetHashCode();
        }

        #endregion

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
