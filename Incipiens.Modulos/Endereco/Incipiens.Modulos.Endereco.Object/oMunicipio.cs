using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Collections.Generic;
using Incipiens.Base.Model;
using System.Linq;
using System.Linq.Expressions;
using System;
using System.Reflection;
using Incipiens.Base.Funcoes;
using System.Security.Cryptography;
using Incipiens.Base.Funcoes.Atributos;

namespace Incipiens.Modulos.Endereco.Object
{
    [DisplayName("Municipios")]
    public class oMunicipio : ModelBase, IEqualityComparer<oMunicipio>
    {
        
        public oMunicipio()
        {

        }

        
        public override string dadosPrincipais
        {
            get { 
                return this._EstadoFederal != null ? this._Nome + "-" + this._EstadoFederal._Uf : this._Nome; }
        }

        #region Encapsulamento

        public string _IdMunicipio
        {
            get { return GetValue(() => _IdMunicipio); }
            set { SetValue(() => _IdMunicipio, value); }
        }

        
        [MaxLength(120, ErrorMessage = "Municipio precisa conter no máximo 120 caracteres")]
        [DisplayName("Município")]
        public string _Nome
        {
            get { return GetValue(() => _Nome); }
            set { 
                SetValue(() => _Nome, value);
                this.NotifyPropertyChanged("dadosPrincipais");
            }
            
        }


        #region Estado

        [Display(AutoGenerateField = false)]
        public string _IdEstado
        {
            get
            {
                return GetValue(() => _IdEstado);
            }
            set
            {
                SetValue(() => _IdEstado, value);
            }
        }

        [Display(AutoGenerateField = false)]
        public oEstadoFederal _EstadoFederal
        {
            get { return GetValue(() => _EstadoFederal); }
            set
            {
                SetValue(() => _EstadoFederal, value);
                if (_EstadoFederal != null)
                    _EstadoFederal.PropertyChanged += _EstadoFederal_PropertyChanged; 
                NotifyPropertyChanged("dadosPrincipais");
            }
        }

        private void _EstadoFederal_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == informacoesPropriedade<oEstadoFederal>.GetNameProperty(e => e._Uf))
                NotifyPropertyChanged("dadosPrincipais");
        }

        #endregion

        #endregion

        #region IEqualityComparer

        public bool Equals(oMunicipio x, oMunicipio y)
        {
            return x._IdMunicipio == y._IdMunicipio;
        }

        public int GetHashCode(oMunicipio obj)
        {
            return obj._IdMunicipio.GetHashCode();
        }

        #endregion

        public override void CloneDeep(ModelBase destino)
        {
            this.Clone(destino);

            var _destino = (oMunicipio)destino;

            if (this._EstadoFederal != null)
            {
                _destino._EstadoFederal = new oEstadoFederal();
                this._EstadoFederal.CloneDeep(_destino._EstadoFederal);
            }
        }

        public override bool isEmpty()
        {
            return
                string.IsNullOrWhiteSpace(_Nome) &&
                string.IsNullOrWhiteSpace(_IdEstado) &&
                string.IsNullOrWhiteSpace(_IdMunicipio);
        }
    }
}

