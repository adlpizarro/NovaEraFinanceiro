using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Incipiens.Base.Model;
using System;
using System.Net.Http;
using System.Threading;
using System.Xml;
using System.IO;
using System.Reflection;
using Incipiens.Base.Funcoes;
using System.Globalization;
using Incipiens.Modulos.Endereco.Object.projection;
using System.ComponentModel.DataAnnotations.Schema;
using Incipiens.Base.Model.Interfaces;
using Incipiens.Base.Model.Validation;
using Incipiens.Base.Model.Tipos;



namespace Incipiens.Modulos.Endereco.Object
{
    [DisplayName("Endereço")]
    public class oEndereco : ModelBase, IEqualityComparer<oEndereco>, IRowVersion
    {
        public oEndereco()
        {
            _Bairro = "";
            _Cep = "";
            _Complemento = "";
            _Logradouro = "";
            _Numero = "";
            _Municipio = null;            
        }

        public override string dadosPrincipais
        {
            get { return this._Logradouro + ", " + this._Numero; }
        }

        #region Propriedades
        
        [DisplayName("ID Endereço")]
        public long _IdEndereco
        {
            get { return GetValue(() => _IdEndereco); }
            set { SetValue(() => _IdEndereco, value); }
        }

        [MaxLength(60, ErrorMessage = "Logradouro: máximo de 60 caracteres")]
        [DisplayName("Logradouro")]
        public string _Logradouro
        {
            get { return GetValue(() => _Logradouro); }
            set
            {
                SetValue(() => _Logradouro, value);
                NotifyPropertyChanged("dadosPrincipais");
            }
        }

        [MaxLength(60, ErrorMessage = "Número: máximo de 60 caracteres")]
        [DisplayName("Número")]
        public string _Numero
        {
            get { return GetValue(() => _Numero); }
            set
            {
                SetValue(() => _Numero, value); 
                NotifyPropertyChanged("dadosPrincipais");
            }
        }

        [MaxLength(60, ErrorMessage = "Complemento: Máximo de 60 caracteres")]
        [DisplayName("Complemento")]
        public string _Complemento
        {
            get {
                return GetValue(() => _Complemento);
            }
            set { SetValue(() => _Complemento, value); }
        }

        [MaxLength(60, ErrorMessage = "Bairro: Máximo de 60 caracteres")]
        [DisplayName("Bairro")]
        public string _Bairro
        {
            get { return GetValue(() => _Bairro); }
            set { SetValue(() => _Bairro, value); }
        }

        [MaxLength(8, ErrorMessage = "CEP: Máximo de 8 caracteres")]
        [DisplayName("CEP")]
        [RegularExpression(@"[0-9]{8}", ErrorMessage = "CEP Inválido.")]
        public string _Cep
        {
            get { return GetValue(() => _Cep); }
            set
            {
                string cep = Strings.RemoverMascara(value);
                SetValue(() => _Cep, cep);
            }
        }

        #region Municipio

        [Display(AutoGenerateField = false)]
        public string _IdMunicipio 
        { 
            get { return GetValue(() => _IdMunicipio); }
            set { SetValue(() => _IdMunicipio, value); } 
        }

        [Display(AutoGenerateField = false)]   
        [RequiredNullOrEmpty(ErrorMessage = "Municipio: Campo Obrigatório")]
        [DisplayName("Municipio")]
        public oMunicipio _Municipio
        {
            get { return GetValue(() => _Municipio); }
            set 
            { 
                SetValue(() => _Municipio, value);
                if(value != null)
                    value.PropertyChanged += Municipio_PropertyChanged;
            }
        }

        private void Municipio_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged("_Municipio");
        }

        #endregion

        public long _VersaoLinha { get; set; }
        

        #endregion

        #region IEqualityComparer

        public bool Equals(oEndereco x, oEndereco y)
        {
            return x._IdEndereco == y._IdEndereco;
        }

        public int GetHashCode(oEndereco obj)
        {
            return obj._IdEndereco.GetHashCode();
        }

        #endregion

        #region Projection

        public static projection.projectionEndereco ToProjection(oEndereco e)
        {
            projectionEndereco p = new projection.projectionEndereco()
            {
                _Cep = e._Cep,
                _Bairro = e._Bairro,
                _IdEndereco = e._IdEndereco,
                _Complemento = e._Complemento,
                _IdMunicipio = e._IdMunicipio,
                _Logradouro = e._Logradouro,
                _Numero = e._Numero
                
            };
            if (e._Municipio != null)
            {
                p._IdMunicipio = e._Municipio._IdMunicipio;
                p._IdEstadoFederal = e._Municipio._IdEstado;
                p._Municipio = e._Municipio._Nome;
                if (e._Municipio._EstadoFederal != null)
                {
                    p._Uf = e._Municipio._EstadoFederal._Uf;
                    p._IdEstadoFederal = e._Municipio._EstadoFederal._IdEstado;
                }
            }

            return p;
        }

        #endregion

        public static string FormataCep(string cep)
        {
            string s = Strings.RemoverMascara(cep);
            if (s.Length == 8)
                s = s.Substring(0, 2) + "." + s.Substring(2, 3) + "-" + s.Substring(5, 3);
            return s;
        }
        public override void CloneDeep(ModelBase destino)
        {
            var _destino = (oEndereco)destino;

            if (this._Municipio != null)
            {
                _destino._Municipio = new oMunicipio();
                this._Municipio.CloneDeep(_destino._Municipio);
            }

            this.Clone(destino);
        }

        public override bool isEmpty()
        {

            return
              _Municipio == null &&
              string.IsNullOrWhiteSpace(_Bairro) &&
              string.IsNullOrWhiteSpace(_Cep) &&
              string.IsNullOrWhiteSpace(_Complemento) &&
              string.IsNullOrWhiteSpace(_IdMunicipio) &&
              string.IsNullOrWhiteSpace(_Logradouro) &&
              string.IsNullOrWhiteSpace(_Numero);
        }
    }
}
    
