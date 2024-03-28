using Incipiens.Base.Funcoes.Atributos;
using Incipiens.Base.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace Incipiens.Modulos.Fornecedor.Object.projection
{
    [DisplayName("Fornecedores")]
    public class projectionFornecedor : ModelBase, IEqualityComparer<projectionFornecedor>
    {
        [Grid(Position: 0, Header: "Id Fornecedor", Width: "0,25*")]
        public long _IdFornecedor
        {
            get { return GetValue(() => _IdFornecedor); }
            set { SetValue(() => _IdFornecedor, value); }
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

        public override string dadosPrincipais 
        { 
            get {
                if (_ApelidoFantasia != "" && _ApelidoFantasia != null)
                    return _ApelidoFantasia;
                else
                    return _NomeRazaoSocial; 
            } 
        }

        public bool Equals(projectionFornecedor x, projectionFornecedor y)
        {
            return x._IdFornecedor == y._IdFornecedor;
        }
        public int GetHashCode(projectionFornecedor obj)
        {
            return obj._IdFornecedor.GetHashCode();
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
