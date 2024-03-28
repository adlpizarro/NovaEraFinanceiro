using Incipiens.Base.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Incipiens.Base.Funcoes.Atributos;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel;

namespace Incipiens.Modulos.Endereco.Object.projection
{
    [DisplayName("Municipios")]
    public class projectionMunicipio : ModelBase, IEqualityComparer<projectionMunicipio>
    {
        [Grid(Position: 1, Header: "Cod. Ibge", Width: "0,1*")]
        public string _IdMunicipio
        {
            get { return GetValue(() => _IdMunicipio); }
            set { SetValue(() => _IdMunicipio, value); }
        }

        [Grid(Position: 1, Header: "Municipio", Width: "1*")]
        public string _Nome
        {
            get { return GetValue(() => _Nome); }
            set
            {
                SetValue(() => _Nome, value);
                this.NotifyPropertyChanged("dadosPrincipais");
            }

        }

        [Grid(Position: 2, Header: "UF", Width: "0,1*")]
        public string _Uf
        {
            get { return GetValue(() => _Uf); }
            set
            {
                SetValue(() => _Uf, value);
                this.NotifyPropertyChanged("dadosPrincipais");
            }
        }

        public override string dadosPrincipais
        {
            get { return _Nome + "-" + _Uf; }
        }

        public override void CloneDeep(ModelBase destino)
        {
            this.Clone(destino);
        }

        public bool Equals(projectionMunicipio x, projectionMunicipio y)
        {
            return x._IdMunicipio == y._IdMunicipio;
        }

        public int GetHashCode(projectionMunicipio obj)
        {
            return obj._IdMunicipio.GetHashCode();
        }

        public override bool isEmpty()
        {
            if (String.IsNullOrEmpty(_IdMunicipio))
                return true;
            else
                return false;

        }
    }
}
