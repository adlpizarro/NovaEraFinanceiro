using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using Incipiens.Base.Model;
using Incipiens.Modulos.Pessoa.Object.Contato;
using Incipiens.Modulos.Endereco.Object;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using Incipiens.Base.Funcoes;
using System.Security.Cryptography;
using System.IO;
using Incipiens.Base.Model.Interfaces;

namespace Incipiens.Modulos.Pessoa.Object
{
    [DisplayName("Pessoa")]
    public abstract class oPessoa : ModelBase, IEqualityComparer<oPessoa>, IRowVersion, INotifyPropertyChanged
    {
        public oPessoa()
        {
            _IdPessoa = 0;

            _Emails = new ObservableCollection<oEmail>();
            _Celulares = new ObservableCollection<oCelular>();
            _Telefones = new ObservableCollection<oTelefone>();
            _Contatos = new ObservableCollection<oContato>();

            _Endereco = new oEndereco();
        }

        #region Propriedades

        [Key]
        [DisplayName("Id Pessoa")]
        public long _IdPessoa
        {
            get { return GetValue(() => _IdPessoa); }
            set { SetValue(() => _IdPessoa, value); }
        }

        #region Enedereco

        [Display(AutoGenerateField = false)]
        public long? _IdEndereco
        {
            get { return GetValue(() => _IdEndereco); }
            set { SetValue(() => _IdEndereco, value); }
        }

        [Display(AutoGenerateField = false)]
        public oEndereco _Endereco
        {
            get { return GetValue(() => _Endereco); }
            set
            {
                SetValue(() => _Endereco, value);
                if (_Endereco != null)
                    _Endereco.PropertyChanged += _Endereco_PropertyChanged;
            }
        }

        private void _Endereco_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == informacoesPropriedade<oEndereco>.GetNameProperty(e => e._Municipio))
            {
                var endereco = (oEndereco)sender;
                if (endereco._Municipio != null)
                {
                    NotifyPropertyChanged("_InscricaoEstadual");
                    NotifyPropertyChanged("getMascaraIe");
                    //endereco._Municipio.PropertyChanged += _Municipio_PropertyChanged;
                    //endereco._Municipio.NotifyPropertyChanged(informacoesPropriedade<oMunicipio>.GetNameProperty(e => e._IdEstado));
                }
            }
        }

       /* private void _Municipio_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == informacoesPropriedade<oMunicipio>.GetNameProperty(e => e._IdEstado))
            {
                var mun = (oMunicipio)sender;
                if (!String.IsNullOrWhiteSpace(mun._IdEstado))
                {
                    NotifyPropertyChanged("_InscricaoEstadual");
                    NotifyPropertyChanged("getMascaraIe");
                }
            }
        }
        */
        #endregion

        [DisplayName("Inscrição Estadual")]
        [MaxLength(15, ErrorMessage = "Inscrição Estadual Inválida")]
        [InscricaoEstadualValidation]
        public string? _InscricaoEstadual
        {
            get { return GetValue(() => _InscricaoEstadual); }
            set { 
                SetValue(() => _InscricaoEstadual, value);

            }
        }


        [Display(AutoGenerateField = false)]
        public ObservableCollection<oTelefone> _Telefones
        {
            get { return GetValue(() => _Telefones); }
            set { 
                SetValue(() => _Telefones, value); 
                if(_Telefones!=null)
                    _Telefones.CollectionChanged += _Telefones_CollectionChanged;
            }
        }
        private void _Telefones_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                int cont = ((ObservableCollection<oTelefone>)sender).Max(t => t._PosicaoTelefone);
                foreach (var item in e.NewItems)
                {
                    var telefone = (oTelefone)item;
                    if (telefone._PosicaoTelefone == 0)
                    {
                        cont++;
                        telefone._PosicaoTelefone = cont;
                    }
                }
            }
        }


        [Display(AutoGenerateField = false)]
        public ObservableCollection<oCelular> _Celulares
        {
            get { return GetValue(() => _Celulares); }
            set { 
                SetValue(() => _Celulares, value);
                if (_Celulares != null)
                    _Celulares.CollectionChanged += _Celulares_CollectionChanged;
            }
        }
        private void _Celulares_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                int cont = ((ObservableCollection<oCelular>)sender).Max(t => t._PosicaoCelular);
                foreach (var item in e.NewItems)
                {
                    var celular = (oCelular)item;
                    if (celular._PosicaoCelular == 0)
                    {
                        cont++;
                        celular._PosicaoCelular = cont;
                    }
                }
            }
        }


        [Display(AutoGenerateField = false)]
        public ObservableCollection<oEmail> _Emails
        {
            get { return GetValue(() => _Emails); }
            set { SetValue(() => _Emails, value); 
                if(_Emails != null)
                    _Emails.CollectionChanged += _Emails_CollectionChanged;
            }
        }
        private void _Emails_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                int cont = ((ObservableCollection<oEmail>)sender).Max(t => t._PosicaoEmail);
                foreach (var item in e.NewItems)
                {
                    var email = (oEmail)item;
                    if (email._PosicaoEmail == 0)
                    {
                        cont++;
                        email._PosicaoEmail = cont;
                    }
                }
            }
        }


        [Display(AutoGenerateField = false)]
        public ObservableCollection<oContato> _Contatos
        {
            get { return GetValue(() => _Contatos); }
            set { 
                SetValue(() => _Contatos, value); 
                if(_Contatos != null)
                    _Contatos.CollectionChanged += _Contatos_CollectionChanged;
            }
        }
        private void _Contatos_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                int cont = ((ObservableCollection<oContato>)sender).Max(t => t._PosicaoContato);
                foreach (var item in e.NewItems)
                {
                    if (item is oContato c)
                    {
                        if (c._PosicaoContato == 0)
                        {
                            cont++;
                            c._PosicaoContato = cont;
                        }
                        c.PropertyChanged += ContatoCpfIgual_PropertyChanged;
                        if (this is oPessoaFisica pf)
                            CheckCpfContatoIgual(pf, c._DadosContato);
                    }
                   
                }
            }

        }

       

        [Display(AutoGenerateField = false)]
        public long _VersaoLinha { get; set; }

        #endregion

        #region Validacao

        #region Inscrição Estadual 

        public class InscricaoEstadualValidation : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var pessoa = (oPessoa)validationContext.ObjectInstance;

                if (pessoa._Endereco != null)
                {
                    if (pessoa._Endereco._IdMunicipio == null)
                        return ValidationResult.Success;
                }
                else
                    return ValidationResult.Success;


                if (String.IsNullOrEmpty(pessoa._InscricaoEstadual))
                    return ValidationResult.Success;

                if (pessoa._Endereco == null)
                    return new ValidationResult("Selecione um Municipio");
                if (pessoa._Endereco._Municipio == null)
                    return new ValidationResult("Selecione um Municipio");

                if (ValidacaoIe(pessoa._InscricaoEstadual, pessoa._Endereco._Municipio._IdEstado))
                    return ValidationResult.Success;
                else
                    return new ValidationResult("Inscrição Estadual Inválida");
            }

            public static bool ValidacaoIe(string ie, string id_estado)
            {
                switch (id_estado)
                {
                    case "12":
                        return validaIE_AC(ie);
                    case "27":
                        return validaIE_AL(ie);
                    case "13":
                        return validaIE_AM(ie);
                    case "16":
                        return validaIE_AP(ie);
                    case "29":
                        return validaIE_BA(ie);
                    case "23":
                        return validaIE_CE(ie);
                    case "53":
                        return validaIE_DF(ie);
                    case "32":
                        return validaIE_ES(ie);
                    case "52":
                        return validaIE_GO(ie);
                    case "21":
                        return validaIE_MA(ie);
                    case "31":
                        return validaIE_MG(ie);
                    case "50":
                        return validaIE_MS(ie);
                    case "51":
                        return validaIE_MT(ie);
                    case "15":
                        return validaIE_PA(ie);
                    case "25":
                        return validaIE_PB(ie);
                    case "26":
                        return validaIE_PE(ie);
                    case "22":
                        return validaIE_PI(ie);
                    case "41":
                        return validaIE_PR(ie);
                    case "33":
                        return validaIE_RJ(ie);
                    case "24":
                        return validaIE_RN(ie);
                    case "11":
                        return validaIE_RO(ie);
                    case "14":
                        return validaIE_RR(ie);
                    case "43":
                        return validaIE_RS(ie);
                    case "42":
                        return validaIE_SC(ie);
                    case "28":
                        return validaIE_SE(ie);
                    case "35":
                        return validaIE_SP(ie);
                    case "17":
                        return validaIE_TO(ie);
                    default:
                        throw new ApplicationException("UF Inválida");
                }

            }

            private static bool validaIE_AC(string ie)
            {
                if (ie.Count() != 13)
                    return false;

                for (int i = 0; i < 2; i++)
                    if (Convert.ToInt32(ie[i].ToString()) != i)
                        return false;

                int soma = 0;
                int pesoInicial = 4;
                int pesoFinal = 9;
                int d1 = 0; //primeiro digito verificador
                int d2 = 0; //segundo digito verificador

                for (int i = 0; i < ie.Count() - 2; i++)
                    if (i < 3)
                    {
                        soma += Convert.ToInt32(ie[i].ToString()) * pesoInicial;
                        pesoInicial--;
                    }
                    else
                    {
                        soma += Convert.ToInt32(ie[i].ToString()) * pesoFinal;
                        pesoFinal--;
                    }

                d1 = 11 - (soma % 11);
                if (d1 == 10 || d1 == 11)
                    d1 = 0;

                soma = d1 * 2;
                pesoInicial = 5;
                pesoFinal = 9;
                for (int i = 0; i < ie.Count() - 2; i++)
                    if (i < 4)
                    {
                        soma += Convert.ToInt32(ie[i].ToString()) * pesoInicial;
                        pesoInicial--;
                    }
                    else
                    {
                        soma += Convert.ToInt32(ie[i].ToString()) * pesoFinal;
                        pesoFinal--;
                    }

                d2 = 11 - (soma % 11);
                if (d2 == 10 || d2 == 11)
                    d2 = 0;

                String dv = d1 + "" + d2;
                if (!dv.Equals(ie.Substring(ie.Count() - 2, 2)))
                    return false;
                return true;
            }

            private static bool validaIE_AL(string ie)
            {
                if (ie.Count() != 9)
                    return false;

                if (!ie.Substring(0, 2).Equals("24"))
                    return false;

                int[]
                digits = { 0, 3, 5, 7, 8 };
                Boolean check = false;
                for (int i = 0; i < digits.Count(); i++)
                    if (Convert.ToInt32(ie[2].ToString()) == digits[i])
                    {
                        check = true;
                        break;
                    }

                if (!check)
                    return false;

                int soma = 0;
                int peso = 9;
                int d = 0; //d&#65533;gito verificador
                for (int i = 0; i < ie.Count() - 1; i++)
                {
                    soma += Convert.ToInt32(ie[i].ToString()) * peso;
                    peso--;
                }
                d = ((soma * 10) % 11);
                if (d == 10)
                    d = 0;

                String dv = d + "";
                if (!ie.Substring(ie.Count() - 1, 1).Equals(dv))
                    return false;
                return true;
            }

            private static bool validaIE_AP(string ie)
            {
                if (ie.Count() != 9)
                    return false;

                if (!ie.Substring(0, 2).Equals("03"))
                    return false;

                int d1 = -1;
                int soma = -1;
                int peso = 9;

                long x = Convert.ToInt64(ie.Substring(0, ie.Count() - 1)); //x = inscri&#65533;&#65533;o estadual sem o d&#65533;gito verificador
                if (x >= 3017001L && x <= 3019022L)
                {
                    d1 = 1;
                    soma = 9;
                }
                else if (x >= 3000001L && x <= 3017000L)
                {
                    d1 = 0;
                    soma = 5;
                }
                else if (x >= 3019023L)
                {
                    d1 = 0;
                    soma = 0;
                }

                for (int i = 0; i < ie.Count() - 1; i++)
                {
                    soma += Convert.ToInt32(ie[i].ToString()) * peso;
                    peso--;
                }

                int d = 11 - ((soma % 11)); //d = armazena o d&#65533;gito verificador ap&#65533;s c&#65533;lculo
                if (d == 10)
                    d = 0;
                else
                    if (d == 11)
                    d = d1;

                String dv = d + "";
                if (!ie.Substring(ie.Count() - 1, 1).Equals(dv))
                    return false;
                return true;
            }

            private static bool validaIE_AM(string ie)
            {
                if (ie.Count() != 9)
                    return false;

                int soma = 0;
                int peso = 9;
                int d = -1; //d&#65533;gito verificador
                for (int i = 0; i < ie.Count() - 1; i++)
                {
                    soma += Convert.ToInt32(ie[i]) * peso;
                    peso--;
                }

                if (soma < 11)
                    d = 11 - soma;
                else
                    if ((soma % 11) <= 1)
                    d = 0;
                else
                    d = 11 - (soma % 11);

                String dv = d + "";
                if (!ie.Substring(ie.Count() - 1, 1).Equals(dv))
                    return false;
                return true;
            }

            private static bool validaIE_BA(string ie)
            {
                if (ie.Count() != 8 && ie.Count() != 9)
                    return false;

                int modulo = 10;
                int firstDigit = Convert.ToInt32(ie[ie.Count() == 8 ? 0 : 1].ToString());
                if (firstDigit == 6 || firstDigit == 7 || firstDigit == 9)
                    modulo = 11;

                int d2 = -1; //segundo d&#65533;gito verificador
                int soma = 0;
                int peso = ie.Count() == 8 ? 7 : 8;
                for (int i = 0; i < ie.Count() - 2; i++)
                {
                    soma += Convert.ToInt32(ie[i].ToString()) * peso;
                    peso--;
                }

                int resto = soma % modulo;

                if (resto == 0 || (modulo == 11 && resto == 1))
                    d2 = 0;
                else
                    d2 = modulo - resto;

                int d1 = -1; //primeiro d&#65533;gito verificador
                soma = d2 * 2;
                peso = ie.Count() == 8 ? 8 : 9;
                for (int i = 0; i < ie.Count() - 2; i++)
                {
                    soma += Convert.ToInt32(ie[i].ToString()) * peso;
                    peso--;
                }

                resto = soma % modulo;

                if (resto == 0 || (modulo == 11 && resto == 1))
                    d1 = 0;
                else
                    d1 = modulo - resto;

                String dv = d1 + "" + d2;
                if (!dv.Equals(ie.Substring(ie.Count() - 2, 2)))
                    return false;
                return true;
            }

            private static bool validaIE_CE(string ie)
            {
                if (ie.Count() != 9)
                    return false;

                int soma = 0;
                int peso = 9;
                int d = -1; //d&#65533;gito verificador
                for (int i = 0; i < ie.Count() - 1; i++)
                {
                    soma += Convert.ToInt32(ie[i].ToString()) * peso;
                    peso--;
                }

                d = 11 - (soma % 11);
                if (d == 10 || d == 11)
                    d = 0;
                String dv = d + "";
                if (!ie.Substring(ie.Count() - 1, 1).Equals(dv))
                    return false;
                return true;
            }

            private static bool validaIE_ES(string ie)
            {
                if (ie.Count() != 9)
                    return false;

                int soma = 0;
                int peso = 9;
                int d = -1; //d&#65533;gito verificador
                for (int i = 0; i < ie.Count() - 1; i++)
                {
                    soma += Convert.ToInt32(ie[i].ToString()) * peso;
                    peso--;
                }

                int resto = soma % 11;
                if (resto < 2)
                    d = 0;
                else
                    if (resto > 1)
                    d = 11 - resto;

                String dv = d + "";
                if (!ie.Substring(ie.Count() - 1, 1).Equals(dv))
                    return false;
                return true;
            }

            private static bool validaIE_GO(string ie)
            {
                if (ie.Count() != 9)
                    return false;

                if (!"10".Equals(ie.Substring(0, 2)))
                    if (!"11".Equals(ie.Substring(0, 2)))
                        if (!"15".Equals(ie.Substring(0, 2)))
                            return false;

                if (ie.Substring(0, ie.Count() - 1).Equals("11094402"))
                {
                    if (!ie.Substring(ie.Count() - 1, ie.Count()).Equals("0"))
                    {
                        if (!ie.Substring(ie.Count() - 1, ie.Count()).Equals("1"))
                            return false;
                        else
                            return true;
                    }
                    else
                        return true;
                }
                else
                {
                    int soma = 0;
                    int peso = 9;
                    int d = -1; //d&#65533;gito verificador
                    for (int i = 0; i < ie.Count() - 1; i++)
                    {
                        soma += Convert.ToInt32(ie[i].ToString()) * peso;
                        peso--;
                    }

                    int resto = soma % 11;
                    long faixaInicio = 10103105;
                    long faixaFim = 10119997;
                    long insc = Convert.ToInt64(ie.Substring(0, ie.Count() - 1));
                    if (resto == 0)
                        d = 0;
                    else
                        if (resto == 1)
                        if (insc >= faixaInicio && insc <= faixaFim)
                            d = 1;
                        else
                            d = 0;
                    else
                            if (resto != 0 && resto != 1)
                        d = 11 - resto;

                    String dv = d + "";
                    if (!ie.Substring(ie.Count() - 1, ie.Count()).Equals(dv))
                        return false;
                    return true;
                }

            }

            private static bool validaIE_MA(string ie)
            {
                if (ie.Count() != 9)
                    return false;

                if (!ie.Substring(0, 2).Equals("12"))
                    return false;

                int soma = 0;
                int peso = 9;
                int d = -1; //d&#65533;gito verificador
                for (int i = 0; i < ie.Count() - 1; i++)
                {
                    soma += Convert.ToInt32(ie[i].ToString()) * peso;
                    peso--;
                }

                d = 11 - (soma % 11);
                if ((soma % 11) == 0 || (soma % 11) == 1)
                    d = 0;
                String dv = d + "";
                if (!ie.Substring(ie.Count() - 1, 1).Equals(dv))
                    return false;
                return true;
            }

            private static bool validaIE_MT(string ie)
            {
                if (ie.Count() != 11)
                    return false;
                int soma = 0;
                int pesoInicial = 3;
                int pesoFinal = 9;
                int d = -1;

                for (int i = 0; i < ie.Count() - 1; i++)
                    if (i < 2)
                    {
                        soma += Convert.ToInt32(ie[i].ToString()) * pesoInicial;
                        pesoInicial--;
                    }
                    else
                    {
                        soma += Convert.ToInt32(ie[i].ToString()) * pesoFinal;
                        pesoFinal--;
                    }
                d = 11 - (soma % 11);
                if ((soma % 11) == 0 || (soma % 11) == 1)
                    d = 0;

                String dv = d + "";
                if (!ie.Substring(ie.Count() - 1, 1).Equals(dv))
                    return false;
                return true;
            }

            private static bool validaIE_MS(string ie)
            {
                if (ie.Count() != 9)
                    return false;

                if (!ie.Substring(0, 2).Equals("28"))
                    return false;

                int soma = 0;
                int peso = 9;
                int d = -1; //d&#65533;gito verificador
                for (int i = 0; i < ie.Count() - 1; i++)
                {
                    soma += Convert.ToInt32(ie[i].ToString()) * peso;
                    peso--;
                }

                int resto = soma % 11;
                int result = 11 - resto;
                if (resto == 0)
                    d = 0;
                else
                    if (resto > 0)
                    if (result > 9)
                        d = 0;
                    else
                        if (result < 10)
                        d = result;
                String dv = d + "";
                if (!ie.Substring(ie.Count() - 1, 1).Equals(dv))
                    return false;
                return true;
            }

            private static bool validaIE_MG(string ie)
            {
                if (ie.Count() != 13)
                    return false;
                String str = "";
                for (int i = 0; i < ie.Count() - 2; i++)
                    if (char.IsDigit(ie[i]))
                        if (i == 3)
                        {
                            str += "0";
                            str += ie[i].ToString();
                        }
                        else
                            str += ie[i].ToString();
                int soma = 0;
                int pesoInicio = 1;
                int pesoFim = 2;
                int d1 = -1; //primeiro d&#65533;gito verificador
                for (int i = 0; i < str.Count(); i++)
                    if (i % 2 == 0)
                    {
                        int x = Convert.ToInt32(str[i].ToString()) * pesoInicio;
                        String strX = x.ToString();
                        for (int j = 0; j < strX.Count(); j++)
                            soma += Convert.ToInt32(strX[j].ToString());
                    }
                    else
                    {
                        int y = Convert.ToInt32(str[i].ToString()) * pesoFim;
                        String strY = y.ToString();
                        for (int j = 0; j < strY.Count(); j++)
                            soma += Convert.ToInt32(strY[j].ToString());
                    }

                int dezenaExata = soma;
                while (dezenaExata % 10 != 0)
                    dezenaExata++;
                d1 = dezenaExata - soma; //resultado - primeiro d&#65533;gito verificador

                soma = d1 * 2;
                pesoInicio = 3;
                pesoFim = 11;
                int d2 = -1;
                for (int i = 0; i < ie.Count() - 2; i++)
                    if (i < 2)
                    {
                        soma += Convert.ToInt32(ie[i].ToString()) * pesoInicio;
                        pesoInicio--;
                    }
                    else
                    {
                        soma += Convert.ToInt32(ie[i].ToString()) * pesoFim;
                        pesoFim--;
                    }

                d2 = 11 - (soma % 11); //resultado - segundo d&#65533;gito verificador
                if ((soma % 11 == 0) || (soma % 11 == 1))
                    d2 = 0;

                String dv = d1 + "" + d2;
                if (!dv.Equals(ie.Substring(ie.Count() - 2, 2)))
                    return false;
                return true;
            }

            private static bool validaIE_PA(string ie)
            {
                if (ie.Count() != 9)
                    return false;

                if (!ie.Substring(0, 2).Equals("15"))
                    return false;

                int soma = 0;
                int peso = 9;
                int d = -1; //d&#65533;gito verificador
                for (int i = 0; i < ie.Count() - 1; i++)
                {
                    soma += Convert.ToInt32(ie[i].ToString()) * peso;
                    peso--;
                }

                d = 11 - (soma % 11);
                if ((soma % 11) == 0 || (soma % 11) == 1)
                    d = 0;

                String dv = d + "";
                if (!ie.Substring(ie.Count() - 1, 1).Equals(dv))
                    return false;
                return true;
            }

            private static bool validaIE_PB(string ie)
            {
                if (ie.Count() != 9)
                    return false;

                int soma = 0;
                int peso = 9;
                int d = -1; //d&#65533;gito verificador
                for (int i = 0; i < ie.Count() - 1; i++)
                {
                    soma += Convert.ToInt32(ie[i].ToString()) * peso;
                    peso--;
                }

                d = 11 - (soma % 11);
                if (d == 10 || d == 11)
                    d = 0;

                String dv = d + "";
                if (!ie.Substring(ie.Count() - 1, 1).Equals(dv))
                    return false;
                return true;
            }

            private static bool validaIE_PR(string ie)
            {
                if (ie.Count() != 10)
                    return false;

                int soma = 0;
                int pesoInicio = 3;
                int pesoFim = 7;
                int d1 = -1; //d&#65533;gito verificador
                for (int i = 0; i < ie.Count() - 2; i++)
                    if (i < 2)
                    {
                        soma += Convert.ToInt32(ie[i].ToString()) * pesoInicio;
                        pesoInicio--;
                    }
                    else
                    {
                        soma += Convert.ToInt32(ie[i].ToString()) * pesoFim;
                        pesoFim--;
                    }

                d1 = 11 - (soma % 11);
                if ((soma % 11) == 0 || (soma % 11) == 1)
                    d1 = 0;

                soma = d1 * 2;
                pesoInicio = 4;
                pesoFim = 7;
                int d2 = -1; //segundo d&#65533;gito
                for (int i = 0; i < ie.Count() - 2; i++)
                    if (i < 3)
                    {
                        soma += Convert.ToInt32(ie[i].ToString()) * pesoInicio;
                        pesoInicio--;
                    }
                    else
                    {
                        soma += Convert.ToInt32(ie[i].ToString()) * pesoFim;
                        pesoFim--;
                    }

                d2 = 11 - (soma % 11);
                if ((soma % 11) == 0 || (soma % 11) == 1)
                    d2 = 0;

                String dv = d1 + "" + d2;
                if (!dv.Equals(ie.Substring(ie.Count() - 2, 2)))
                    return false;
                return true;
            }

            private static bool validaIE_PE(string ie)
            {
                if (ie.Count() != 9)
                    return false;
                int soma = 0;
                int pesoInicio = 8;
                int d = -1; //d&#65533;gito verificador

                for (int i = 0; i < ie.Count() - 1; i++)
                    if (i < 7)
                    {
                        soma += Convert.ToInt32(ie[i].ToString()) * pesoInicio;
                        pesoInicio--;
                    }

                d = soma % 11;
                if (d < 2)
                    d = 0;
                else
                    d = 11 - d;

                String dv = d + "";
                if (!ie.Substring(ie.Count() - 2, 1).Equals(dv))
                    return false;

                pesoInicio = 9; soma = 0;
                for (int i = 0; i < ie.Count() - 1; i++)
                    if (i < 8)
                    {
                        soma += Convert.ToInt32(ie[i].ToString()) * pesoInicio;
                        pesoInicio--;
                    }
                d = soma % 11;
                if (d < 2)
                    d = 0;
                else
                    d = 11 - d;
                dv = d + "";
                if (!ie.Substring(ie.Count() - 1, 1).Equals(dv))
                    return false;
                return true;
            }

            private static bool validaIE_PI(string ie)
            {
                if (ie.Count() != 9)
                    return false;

                int soma = 0;
                int peso = 9;
                int d = -1; //d&#65533;gito verificador
                for (int i = 0; i < ie.Count() - 1; i++)
                {
                    soma += Convert.ToInt32(ie[i].ToString()) * peso;
                    peso--;
                }

                d = 11 - (soma % 11);
                if (d == 11 || d == 10)
                    d = 0;

                String dv = d + "";
                if (!ie.Substring(ie.Count() - 1, 1).Equals(dv))
                    return false;
                return true;
            }

            private static bool validaIE_RJ(string ie)
            {
                if (ie.Count() != 8)
                    return false;

                int soma = 0;
                int peso = 7;
                int d = -1; //d&#65533;gito verificador
                for (int i = 0; i < ie.Count() - 1; i++)
                    if (i == 0)
                        soma += Convert.ToInt32(ie[i].ToString()) * 2;
                    else
                    {
                        soma += Convert.ToInt32(ie[i].ToString()) * peso;
                        peso--;
                    }

                d = 11 - (soma % 11);
                if ((soma % 11) <= 1)
                    d = 0;

                String dv = d + "";
                if (!ie.Substring(ie.Count() - 1, 1).Equals(dv))
                    return false;
                return true;
            }

            private static bool validaIE_RN(string ie)
            {
                if (ie.Count() != 10 && ie.Count() != 9)
                    return false;

                if (!ie.Substring(0, 2).Equals("20"))
                    return false;

                if (ie.Count() == 9)
                {
                    int soma = 0;
                    int peso = 9;
                    int d = -1; //d&#65533;gito verificador
                    for (int i = 0; i < ie.Count() - 1; i++)
                    {
                        soma += Convert.ToInt32(ie[i].ToString()) * peso;
                        peso--;
                    }

                    d = ((soma * 10) % 11);
                    if (d == 10)
                        d = 0;

                    String dv = d + "";
                    if (!ie.Substring(ie.Count() - 1, 1).Equals(dv))
                        return false;
                    return true;
                }
                else
                {
                    int soma = 0;
                    int peso = 10;
                    int d = -1; //d&#65533;gito verificador
                    for (int i = 0; i < ie.Count() - 1; i++)
                    {
                        soma += Convert.ToInt32(ie[i].ToString()) * peso;
                        peso--;
                    }
                    d = ((soma * 10) % 11);
                    if (d == 10)
                        d = 0;
                    String dv = d + "";
                    if (!ie.Substring(ie.Count() - 1, 1).Equals(dv))
                        return false;
                    return true;
                }

            }

            private static bool validaIE_RS(string ie)
            {
                if (ie.Count() != 10)
                    return false;

                int soma = Convert.ToInt32(ie[0].ToString()) * 2;
                int peso = 9;
                int d = -1; //d&#65533;gito verificador
                for (int i = 1; i < ie.Count() - 1; i++)
                {
                    soma += Convert.ToInt32(ie[i].ToString()) * peso;
                    peso--;
                }

                d = 11 - (soma % 11);
                if (d == 10 || d == 11)
                    d = 0;

                String dv = d + "";
                if (!ie.Substring(ie.Count() - 1, 1).Equals(dv))
                    return false;
                return true;
            }

            private static bool validaIE_RO(string ie)
            {
                if (ie.Count() != 14)
                    return false;

                int soma = 0;
                int pesoInicio = 6;
                int pesoFim = 9;
                int d = -1; //d&#65533;gito verificador
                for (int i = 0; i < ie.Count() - 1; i++)
                    if (i < 5)
                    {
                        soma += Convert.ToInt32(ie[i].ToString()) * pesoInicio;
                        pesoInicio--;
                    }
                    else
                    {
                        soma += Convert.ToInt32(ie[i].ToString()) * pesoFim;
                        pesoFim--;
                    }

                d = 11 - (soma % 11);
                if (d == 11 || d == 10)
                    d -= 10;

                String dv = d + "";
                if (!ie.Substring(ie.Count() - 1, 1).Equals(dv))
                    return false;
                return true;
            }

            private static bool validaIE_RR(string ie)
            {
                if (ie.Count() != 9)
                    return false;

                if (!ie.Substring(0, 2).Equals("24"))
                    return false;

                int soma = 0;
                int peso = 1;
                int d = -1; //d&#65533;gito verificador
                for (int i = 0; i < ie.Count() - 1; i++)
                {
                    soma += Convert.ToInt32(ie[i].ToString()) * peso;
                    peso++;
                }

                d = soma % 9;

                String dv = d + "";
                if (!ie.Substring(ie.Count() - 1, 1).Equals(dv))
                    return false;
                return true;
            }

            private static bool validaIE_SC(string ie)
            {
                if (ie.Count() != 9)
                    return false;

                int soma = 0;
                int peso = 9;
                int d = -1; //d&#65533;gito verificador
                for (int i = 0; i < ie.Count() - 1; i++)
                {
                    soma += Convert.ToInt32(ie[i].ToString()) * peso;
                    peso--;
                }

                d = 11 - (soma % 11);
                if ((soma % 11) == 0 || (soma % 11) == 1)
                    d = 0;

                String dv = d + "";
                if (!ie.Substring(ie.Count() - 1, 1).Equals(dv))
                    return false;
                return true;
            }

            private static bool validaIE_SP(string ie)
            {
                if (ie.Count() != 12 && ie.Count() != 13)
                    return false;

                if (ie.Count() == 12)
                {
                    Int32 soma = 0;
                    int peso = 1;
                    int d1 = -1; //primeiro d&#65533;gito verificador
                    for (int i = 0; i < ie.Count() - 4; i++)
                    {
                        if (i == 1 || i == 7)
                        {
                            soma += Convert.ToInt32(ie[i].ToString()) * ++peso;
                            peso++;
                        }
                        else
                        {
                            soma += Convert.ToInt32(ie[i].ToString()) * peso;
                            peso++;
                        }
                    }


                    d1 = soma % 11;
                    String strD1 = d1.ToString(); //O d&#65533;gito &#65533; igual ao algarismo mais a direita do resultado de (soma % 11)
                    d1 = Convert.ToInt16(strD1[strD1.Count() - 1].ToString());

                    int soma1 = 0;
                    int pesoInicio = 3;
                    int pesoFim = 10;
                    int d2 = -1; //segundo d&#65533;gito verificador
                    for (int i = 0; i < ie.Count() - 1; i++)
                    {
                        if (i < 2)
                        {
                            soma1 += Convert.ToInt16(ie[i].ToString()) * pesoInicio;
                            pesoInicio--;
                        }
                        else
                        {
                            soma1 += Convert.ToInt16(ie[i].ToString()) * pesoFim;
                            pesoFim--;
                        }
                    }

                    d2 = soma1 % 11;
                    String strD2 = d2.ToString(); //O d&#65533;gito &#65533; igual ao algarismo mais a direita do resultado de (soma % 11)
                    d2 = Convert.ToInt16(strD2[strD2.Count() - 1].ToString());
                    String auxsub = ie.Substring(8, 2);

                    if (auxsub.Equals(d1.ToString()))
                        return false;
                    if (!ie.Substring(11, 1).Equals(d2 + ""))
                        return false;
                }
                else
                {
                    if (ie[0] != 'P')
                        return false;

                    String strIE = ie.Substring(1, 10); //Obt&#65533;m somente os d&#65533;gitos utilizados no c&#65533;lculo do d&#65533;gito verificador
                    int soma = 0;
                    int peso = 1;
                    int d1 = -1; //primeiro d&#65533;gito verificador

                    for (int i = 0; i < strIE.Count() - 1; i++)
                    {
                        if (i == 1 || i == 7)
                        {
                            soma += Convert.ToInt16(strIE[i].ToString()) * ++peso;
                            peso++;
                        }
                        else
                        {
                            soma += Convert.ToInt16(strIE[i].ToString()) * peso;
                            peso++;
                        }
                    }

                    d1 = soma % 11;
                    String strD1 = d1.ToString(); //O d&#65533;gito &#65533; igual ao algarismo mais a direita do resultado de (soma % 11)
                    d1 = Convert.ToInt16(strD1[strD1.Count() - 1].ToString());

                    if (!ie.Substring(9, 1).Equals(d1 + ""))
                        return false;
                }

                return true;
            }

            private static bool validaIE_SE(string ie)
            {
                if (ie.Count() != 9)
                    return false;

                int soma = 0;
                int peso = 9;
                int d = -1; //d&#65533;gito verificador
                for (int i = 0; i < ie.Count() - 1; i++)
                {
                    soma += Convert.ToInt32(ie[i].ToString()) * peso;
                    peso--;
                }

                d = 11 - (soma % 11);
                if (d == 11 || d == 11 || d == 10)
                    d = 0;

                String dv = d + "";
                if (!ie.Substring(ie.Count() - 1, 1).Equals(dv))
                    return false;
                return true;
            }

            private static bool validaIE_TO(string ie)
            {
                if (ie.Count() != 9 && ie.Count() != 11)
                    return false;
                else
                    if (ie.Count() == 9)
                    ie = ie.Substring(0, 2) + "02" + ie.Substring(2);

                int soma = 0;
                int peso = 9;
                int d = -1; //d&#65533;gito verificador
                for (int i = 0; i < ie.Count() - 1; i++)
                    if (i != 2 && i != 3)
                    {
                        soma += Convert.ToInt32(ie[i].ToString()) * peso;
                        peso--;
                    }
                d = 11 - (soma % 11);
                if ((soma % 11) < 2)
                    d = 0;

                String dv = d + "";
                if (!ie.Substring(ie.Count() - 1, 1).Equals(dv))
                    return false;
                return true;
            }

            private static bool validaIE_DF(string ie)
            {
                if (ie.Count() != 13)
                    return false;
                int soma = 0;
                int pesoInicio = 4;
                int pesoFim = 9;
                int d1 = -1; //primeiro d&#65533;gito verificador
                for (int i = 0; i < ie.Count() - 2; i++)
                    if (i < 3)
                    {
                        soma += Convert.ToInt32(ie[i].ToString()) * pesoInicio;
                        pesoInicio--;
                    }
                    else
                    {
                        soma += Convert.ToInt32(ie[i].ToString()) * pesoFim;
                        pesoFim--;
                    }
                d1 = 11 - (soma % 11);
                if (d1 == 11 || d1 == 10)
                    d1 = 0;

                soma = d1 * 2;
                pesoInicio = 5;
                pesoFim = 9;
                int d2 = -1; //segundo d&#65533;gito verificador
                for (int i = 0; i < ie.Count() - 2; i++)
                    if (i < 4)
                    {
                        soma += Convert.ToInt32(ie[i].ToString()) * pesoInicio;
                        pesoInicio--;
                    }
                    else
                    {
                        soma += Convert.ToInt32(ie[i].ToString()) * pesoFim;
                        pesoFim--;
                    }
                d2 = 11 - (soma % 11);
                if (d2 == 11 || d2 == 10)
                    d2 = 0;

                String dv = d1 + "" + d2;
                if (!dv.Equals(ie.Substring(ie.Count() - 2, 2)))
                    return false;
                return true;
            }
        }

        #region MascaraIe

        [NotMapped]
        public string getMascaraIe
        {
            get
            {
                string valor = "";
                char c = '9';
                if(_Endereco != null)
                    if(_Endereco._Municipio != null)
                switch (_Endereco._Municipio._IdEstado)
                {
                    case "12":
                        valor = c.ToString() + c.ToString() + "," + c.ToString() + c.ToString() + c.ToString() + "," + c.ToString() + c.ToString() + c.ToString() + "/" + c.ToString() + c.ToString() + c.ToString() + "-" + c.ToString() + c.ToString();
                        break;

                    case "27":
                        valor = c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + "";
                        break;

                    case "16":
                        valor = c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + "";
                        break;

                    case "13":
                        valor = c.ToString() + c.ToString() + "," + c.ToString() + c.ToString() + c.ToString() + "," + c.ToString() + c.ToString() + c.ToString() + "-" + c.ToString();
                        break;

                    case "29":
                        valor = c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + "-" + c.ToString() + c.ToString();
                        break;

                    case "23":
                        valor = c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + "-" + c.ToString();
                        break;

                    case "53":
                        valor = c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + "-" + c.ToString() + c.ToString();
                        break;

                    case "32":
                        valor = c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + "-" + c.ToString();
                        break;

                    case "52":
                        valor = c.ToString() + c.ToString() + "," + c.ToString() + c.ToString() + c.ToString() + "," + c.ToString() + c.ToString() + c.ToString() + "-" + c.ToString();
                        break;

                    case "21":
                        valor = c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + "-" + c.ToString();
                        break;

                    case "51":
                        valor = c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + "-" + c.ToString();
                        break;

                    case "50":
                        valor = c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + "-" + c.ToString();
                        break;

                    case "31":
                        valor = c.ToString() + c.ToString() + c.ToString() + "," + c.ToString() + c.ToString() + c.ToString() + "," + c.ToString() + c.ToString() + c.ToString() + "/" + c.ToString() + c.ToString() + c.ToString() + c.ToString();
                        break;

                   case "15":
                        valor = c.ToString() + c.ToString() + "-" + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + "-" + c.ToString();
                        break;

                   case "25":
                        valor = c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + "-" + c.ToString();
                        break;

                   case "41":
                        valor = c.ToString() + c.ToString() + c.ToString() + "," + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + "-" + c.ToString() + c.ToString();
                        break;

                   case "26":
                        valor = c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + "-" + c.ToString() + c.ToString();
                        break;

                   case "22":
                        valor = c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + "-" + c.ToString();
                        break;

                   case "33":
                        valor = c.ToString() + c.ToString() + "," + c.ToString() + c.ToString() + c.ToString() + "," + c.ToString() + c.ToString() + "-" + c.ToString();
                        break;

                   case "24":
                        valor = c.ToString() + c.ToString() + "," + c.ToString() + c.ToString() + c.ToString() + "," + c.ToString() + c.ToString() + c.ToString() + "-" + c.ToString();
                        break;

                   case "43":
                        valor = c.ToString() + c.ToString() + c.ToString() + "/" + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString();
                        break;

                   case "11":
                        valor = c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + "-" + c.ToString();
                        break;

                   case "14":
                        valor = c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + "-" + c.ToString();
                        break;

                   case "35":
                        valor = c.ToString() + c.ToString() + c.ToString() + "," + c.ToString() + c.ToString() + c.ToString() + "," + c.ToString() + c.ToString() + c.ToString() + "," + c.ToString() + c.ToString() + c.ToString();
                        break;

                   case "42":
                        valor = c.ToString() + c.ToString() + c.ToString() + "," + c.ToString() + c.ToString() + c.ToString() + "," + c.ToString() + c.ToString() + c.ToString();
                        break;

                   case "28":
                        valor = c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + "-" + c.ToString();
                        break;

                   case "17":
                        valor = c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + c.ToString() + "-" + c.ToString();
                        break;

                    default:
                        valor = "";
                        break;
                }

                return valor;
            }
        }

        #endregion

        #endregion

        #region ContatoCpfIgual

        private void ContatoCpfIgual_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var contato = (oContato)sender;
            if (e.PropertyName == informacoesPropriedade<oContato>.GetNameProperty(c => c._DadosContato))
                if (contato._DadosContato != null)
                {
                    if (this is oPessoaFisica pf)
                    {
                        contato.PropertyChanged += _DadosContato_PropertyChanged;
                    }
                }
            if(this is oPessoaFisica pf2)
                CheckCpfContatoIgual(pf2, contato._DadosContato);
        }

        private void _DadosContato_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == informacoesPropriedade<oPessoaFisica>.GetNameProperty(pf => pf._Cpf))
                if (this is oPessoaFisica pf)
                    CheckCpfContatoIgual(pf, (oPessoaFisica)sender);
        }

        protected static void CheckCpfContatoIgual(oPessoaFisica pessoa, oPessoaFisica contato)
        {
            string erro = "CPF do Contato não pode ser Igual ao CPF do Cadastro Principal";
            if (!string.IsNullOrWhiteSpace(pessoa._Cpf) && !string.IsNullOrWhiteSpace(contato._Cpf))
                if (pessoa._Cpf == contato._Cpf)
                    contato.AddError(informacoesPropriedade<oPessoaFisica>.GetNameProperty(pf => pf._Cpf), erro);
                else
                    contato.RemoveError(informacoesPropriedade<oPessoaFisica>.GetNameProperty(pf => pf._Cpf), erro);
            else
                contato.RemoveError(informacoesPropriedade<oPessoaFisica>.GetNameProperty(pf => pf._Cpf), erro);
        }

        #endregion

        #endregion

        #region IEqualityComparer

        public bool Equals(oPessoa x, oPessoa y)
        {
            return x._IdPessoa == y._IdPessoa;
        }

        public int GetHashCode(oPessoa obj)
        {
            return obj._IdPessoa.GetHashCode();
        }

        #endregion


        public bool cloneContato = true;
        public override void CloneDeep(ModelBase destino)
        {
            this.Clone(destino);

            var _destino = (oPessoa)destino;

            if (this._Endereco != null)
            {
                _destino._Endereco = new oEndereco();
                this._Endereco.CloneDeep(_destino._Endereco);
            }

            _destino._Telefones = new ObservableCollection<oTelefone>();
            foreach(var telefone in this._Telefones)
            {
                var dTelefone = new oTelefone();
                telefone.CloneDeep(dTelefone);
                _destino._Telefones.Add(dTelefone);
            }

            _destino._Celulares = new ObservableCollection<oCelular>();
            foreach (var celular in this._Celulares)
            {
                var dCelular = new oCelular();
                celular.CloneDeep(dCelular);
                _destino._Celulares.Add(dCelular);
            }

            _destino._Emails = new ObservableCollection<oEmail>();
            foreach (var email in this._Emails)
            {
                var dEmail = new oEmail();
                email.CloneDeep(dEmail);
                _destino._Emails.Add(dEmail);
            }

            if(cloneContato)
            {
                _destino._Contatos = new ObservableCollection<oContato>();
                foreach (var contato in this._Contatos)
                {
                    var dContato = new oContato();
                    contato.CloneDeep(dContato);
                    _destino._Contatos.Add(dContato);
                }
            }  
        }

        public override string dadosPrincipais
        {
            get
            {
                if (this is oPessoaFisica pf)
                    return pf.dadosPrincipais;
                else if (this is oPessoaJuridica pj)
                    return pj.dadosPrincipais;
                return "OPA";
            }
        }
    }
}
