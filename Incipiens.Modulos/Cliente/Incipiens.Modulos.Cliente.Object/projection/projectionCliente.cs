using Incipiens.Base.Funcoes.Atributos;
using Incipiens.Base.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace Incipiens.Modulos.Cliente.Object.projection
{
    [DisplayName("Clientes")]
    public class projectionCliente : ModelBase, IEqualityComparer<projectionCliente>
    {
        [Grid(Position: 0, Header: "Id Cliente", Width: "0,25*")]
        public long _IdCliente
        {
            get { return GetValue(() => _IdCliente); }
            set { SetValue(() => _IdCliente, value); }
        }
        [Grid(Position: 1, Header: "CPF/CNPJ", Width: "0,3*")]
        public string _CpfCnpj
        {
            get { return GetValue(() => _CpfCnpj); }
            set { SetValue(() => _CpfCnpj, value); }
        }
        [Grid(Position: 3, Header: "Apelido/Nome Fantasia", Width: "1*")]
        public string _ApelidoFantasia
        {
            get { return GetValue(() => _ApelidoFantasia); }
            set { SetValue(() => _ApelidoFantasia, value); }
        }
        [Grid(Position: 2, Header: "Nome/Razão Social", Width: "1*")]
        public string _NomeRazaoSocial
        {
            get { return GetValue(() => _NomeRazaoSocial); }
            set { SetValue(() => _NomeRazaoSocial, value); }
        }
        public override string dadosPrincipais { get { return _NomeRazaoSocial + "/" + _ApelidoFantasia; } }

        public bool Equals(projectionCliente x, projectionCliente y)
        {
            return x._IdCliente == y._IdCliente;
        }
        public int GetHashCode(projectionCliente obj)
        {
            return obj._IdCliente.GetHashCode();
        }

        public override void CloneDeep(ModelBase destino)
        {
            this.Clone(destino);
        }

        public override bool isEmpty()
        {
            throw new NotImplementedException();
        }
    }
}
