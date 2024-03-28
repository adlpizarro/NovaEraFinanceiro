using System;
using System.Collections.Generic;
using System.Text;
using Incipiens.Base.Model;
using System.ComponentModel.DataAnnotations;
using Incipiens.Base.Funcoes.Atributos;
using System.ComponentModel;

namespace Incipiens.Modulos.Endereco.Object.projection
{
    [DisplayName("Endereços")]
    public class projectionEndereco : ModelBase, IEqualityComparer<projectionEndereco>
    {

        public projectionEndereco()
        {
            _Bairro = "";
            _Cep = "";
            _Complemento = "";
            _IdEndereco = 0;
            _IdEstadoFederal = "";
            _IdMunicipio = "";
            _Logradouro = "";
            _Municipio = "";
            _Numero = "";
            _Uf = "";

        }


        public override string dadosPrincipais
        {
            get { return this._Logradouro + ", " + this._Numero; }
        }

        #region Propriedades

        [Grid(Position: 0, Header: "Id Endereço", Width: "0,5*", PrintDocumentWidth: "0,85*")]
        public long _IdEndereco
        {
            get { return GetValue(() => _IdEndereco); }
            set { SetValue(() => _IdEndereco, value); }
        }

        [Grid(Position: 1, Header: "Cep", Width: "0,5*", Mask: "00.000-000", PrintDocumentWidth: "1*")]
        public string _Cep
        {
            get { return GetValue(() => _Cep); }
            set { SetValue(() => _Cep, value); }
        }

        public string _IdEstadoFederal
        {
            get { return GetValue(() => _IdEstadoFederal); }
            set { SetValue(() => _IdEstadoFederal, value); }
        }

        [Grid(Position: 2, Header: "Uf", Width: "0,25*", PrintDocumentWidth: "0,5*")]
        public string _Uf
        {
            get { return GetValue(() => _Uf); }
            set { SetValue(() => _Uf, value); }
        }

        public string _IdMunicipio
        {
            get { return GetValue(() => _IdMunicipio); }
            set { SetValue(() => _IdMunicipio, value); }
        }

        [Grid(Position: 3, Header: "Municipio", Width: "1*", PrintDocumentWidth: "1*")]
        public string _Municipio
        {
            get { return GetValue(() => _Municipio); }
            set { SetValue(() => _Municipio, value); }
        }

        [Grid(Position: 4, Header: "Bairro", Width: "1*", PrintDocumentWidth: "2*")]
        public string _Bairro
        {
            get { return GetValue(() => _Bairro); }
            set { SetValue(() => _Bairro, value); }
        }

        [Grid(Position: 5, Header: "Logradouro", Width: "2*", PrintDocumentWidth: "2,5*")]
        public string _Logradouro
        {
            get { return GetValue(() => _Logradouro); }
            set
            {
                SetValue(() => _Logradouro, value);
                NotifyPropertyChanged("dadosPrincipais");
            }
        }

        [Grid(Position: 6, Header: "Numero", Width: "0,5*", PrintDocumentWidth: "0,75*")]
        public string _Numero
        {
            get { return GetValue(() => _Numero); }
            set
            {
                SetValue(() => _Numero, value);
                NotifyPropertyChanged("dadosPrincipais");
            }
        }

        [Grid(Position: 7, Header: "Complemento", Width: "1*", PrintDocumentWidth: "1,5*")]
        public string _Complemento
        {
            get { return GetValue(() => _Complemento); }
            set { SetValue(() => _Complemento, value); }
        }

        #endregion

        #region IEqualityComparer

        public bool Equals(projectionEndereco x, projectionEndereco y)
        {
            return x._IdEndereco == y._IdEndereco;
        }

        public int GetHashCode(projectionEndereco obj)
        {
            return obj._IdEndereco.GetHashCode();
        }

        #endregion

        public oEndereco ToEndereco()
        {
            return new oEndereco()
            {
                _IdEndereco = this._IdEndereco,
                _Bairro = this._Bairro,
                _Cep = this._Cep,
                _Complemento = this._Complemento,
                _IdMunicipio = this._IdMunicipio,
                _Logradouro = this._Logradouro,

                _Municipio = new oMunicipio()
                {
                    _IdMunicipio = this._IdMunicipio,
                    _EstadoFederal = new oEstadoFederal()
                    {
                        _IdEstado = this._IdEstadoFederal,
                    },
                    _IdEstado = this._IdEstadoFederal
                },
                _Numero = this._Numero
            };
        }

        public override void CloneDeep(ModelBase destino)
        {
            this.Clone(destino);
        }

        public override bool isEmpty()
        {
            return
                string.IsNullOrWhiteSpace(_Bairro) &&
                string.IsNullOrWhiteSpace(_Cep) &&
                string.IsNullOrWhiteSpace(_Complemento) &&
                _IdEndereco <= 0 &&
                string.IsNullOrWhiteSpace(_IdEstadoFederal) &&
                string.IsNullOrWhiteSpace(_IdMunicipio) &&
                string.IsNullOrWhiteSpace(_Logradouro) &&
                string.IsNullOrWhiteSpace(_Municipio) &&
                string.IsNullOrWhiteSpace(_Numero) &&
                string.IsNullOrWhiteSpace(_Uf);

        }
    }
}
