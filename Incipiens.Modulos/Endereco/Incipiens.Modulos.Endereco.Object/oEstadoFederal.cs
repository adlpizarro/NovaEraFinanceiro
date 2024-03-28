using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Incipiens.Base.Model;
using System.Linq;
using System.Linq.Expressions;
using System;
using System.Reflection;
using Incipiens.Base.Funcoes;
using System.Collections.ObjectModel;
using System.Security.Cryptography;

namespace Incipiens.Modulos.Endereco.Object
{
    public class oEstadoFederal : ModelBase, IEqualityComparer<oEstadoFederal>
    {

        public oEstadoFederal()
        {
            _Municipios = new ObservableCollection<oMunicipio>();
        }

        #region Propriedades

        [DisplayName("Id. Estado")]
        public string _IdEstado
        {
            get { return GetValue(() => _IdEstado); }
            set { SetValue(() => _IdEstado, value); }
        }

        [MaxLength(2, ErrorMessage = "UF precisa conter exatamente 2 caracteres")]
        [MinLength(2, ErrorMessage = "UF precisa conter exatamente 2 caracteres")]
        [DisplayName("UF")]
        public string _Uf
        {
            get { return GetValue(() => _Uf); }
            set { SetValue(() => _Uf, value); }
        }

        [MaxLength(60, ErrorMessage = "Estado Federal precisa conter no máximo 60 caracteres")]
        [DisplayName("Estado Federal")]
        public string _Nome
        {
            get { return GetValue(() => _Nome); }
            set
            {
                SetValue(() => _Nome, value);
                NotifyPropertyChanged("dadosPrincipais");
            }
        }

        public ObservableCollection<oMunicipio> _Municipios
        {
            get { return GetValue(() => _Municipios); }
            set { SetValue(() => _Municipios, value); }
        }

        public override string dadosPrincipais { get { return _Nome; } }

        #endregion

        #region IEqualityComparer

        public bool Equals(oEstadoFederal x, oEstadoFederal y)
        {
            return x._IdEstado == y._IdEstado;
        }

        public int GetHashCode(oEstadoFederal obj)
        {
            return obj._IdEstado.GetHashCode();
        }

        #endregion

        public override void CloneDeep(ModelBase destino)
        {
            this.Clone(destino);
        }

        public override bool isEmpty()
        {
            return
                string.IsNullOrWhiteSpace(_Nome) &&
                string.IsNullOrWhiteSpace(_IdEstado) &&
                string.IsNullOrWhiteSpace(_Uf);
        }
    }
}
