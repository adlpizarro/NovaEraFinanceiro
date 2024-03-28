using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using Incipiens.Base.Model;
using Incipiens.Modulos.Pessoa.Object.Contato;
using Incipiens.Base.Funcoes;

namespace Incipiens.Modulos.Pessoa.Object
{
    public class oPessoaJuridica: oPessoa
    {
        public oPessoaJuridica() : base()
        {
            _Cnpj = null;
            _RazaoSocial = "";
        }

        public override string dadosPrincipais
        {
            get {
                if (!string.IsNullOrEmpty(_RazaoSocial) && !string.IsNullOrEmpty(_NomeFantasia))
                    return _NomeFantasia + "/" + _RazaoSocial;
                else if (string.IsNullOrEmpty(_RazaoSocial))
                    return _NomeFantasia;
                return _RazaoSocial;
            }
        }

        #region Propriedades

        [DisplayName("CNPJ")]
        [MaxLength(14, ErrorMessage = "CNPJ Inválido")]
        [CnpjValidation(ErrorMessage = "CNPJ Inválido")]
        public string _Cnpj
        {
            get { return GetValue(() => _Cnpj); }
            set { SetValue(() => _Cnpj, value); }
        }

        [MaxLength(120, ErrorMessage = "Razão Social não pode ultrapassar 120 caracteres")]
        [DisplayName("Razão Social")]
        [Required(ErrorMessage = "Razão Social precisa ser preenchida")]
        [MinLength(1, ErrorMessage = "Razão Social precisa ser preenchida")]
        public string _RazaoSocial
        {
            get { return GetValue(() => _RazaoSocial); }
            set { SetValue(() => _RazaoSocial, value);
                NotifyPropertyChanged("dadosPrincipais");
            }
        }

        [MaxLength(120, ErrorMessage = "Nome Fantasia Não pode ultrapassar 120 caracteres")]
        [DisplayName("Nome Fantasia")]
        public string? _NomeFantasia
        {
            get { return GetValue(() => _NomeFantasia); }
            set { SetValue(() => _NomeFantasia, value);
                NotifyPropertyChanged("dadosPrincipais");
            }
        }

        #endregion

        #region Validacao

        #region Cnpj

        public class CnpjValidation : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value != null)
                {
                    string cnpj = value.ToString();
                    if (!String.IsNullOrEmpty(cnpj))
                        if (Incipiens.Base.Funcoes.Strings.ContemSomenteNumeros(cnpj))
                            if (ValidaCnpj(cnpj))
                                return ValidationResult.Success;
                            else
                                return new ValidationResult("CNPJ Inválido");
                        else
                            return new ValidationResult("CNPJ deve conter somente números");
                }
                return ValidationResult.Success;
            }

            public static bool ValidaCnpj(string sCNPJ)
            {
                int[] vetCNPJ = new int[14];
                int iInd = 0, iCont = 0, iCont2, iSoma = 0, iMult, iResto, iDigi1, iDigi2;
                char cDigi1, cDigi2;
                if (sCNPJ.Length == 14)
                {
                    cDigi1 = sCNPJ[12];
                    cDigi2 = sCNPJ[13];
                    while (iCont != 14)
                    {
                        vetCNPJ[iCont] = int.Parse(sCNPJ[iCont].ToString());
                        iCont++;
                    }
                    iInd = 0;
                    iCont = 5;
                    iCont2 = 9;
                    while (iCont > 1)
                    {
                        iMult = vetCNPJ[iInd] * iCont;
                        iSoma = iSoma + iMult;
                        iCont--;
                        iInd++;
                    }
                    while (iCont2 > 1)
                    {
                        iMult = vetCNPJ[iInd] * iCont2;
                        iSoma = iSoma + iMult;
                        iCont2--;
                        iInd++;
                    }
                    iResto = iSoma % 11;
                    if (iResto < 2)
                        iDigi1 = 0;
                    else
                        iDigi1 = 11 - iResto;
                    vetCNPJ[12] = iDigi1;
                    iInd = 0;
                    iSoma = 0;
                    iCont = 6;
                    iCont2 = 9;
                    while (iCont > 1)
                    {
                        iMult = vetCNPJ[iInd] * iCont;
                        iSoma = iSoma + iMult;
                        iCont--;
                        iInd++;
                    }
                    while (iCont2 > 1)
                    {
                        iMult = vetCNPJ[iInd] * iCont2;
                        iSoma = iSoma + iMult;
                        iCont2--;
                        iInd++;
                    }
                    iResto = iSoma % 11;
                    if (iResto < 2)
                        iDigi2 = 0;
                    else
                        iDigi2 = 11 - iResto;
                    if (iDigi1 == int.Parse(cDigi1.ToString()) && iDigi2 == int.Parse(cDigi2.ToString()))
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
        }

        #endregion

        #endregion

        public static string FormataCnpj(string cnpj)
        {
            string s = cnpj.RemoverMascara();
            if (s.Length == 14)
                s = s.Substring(0, 2) + "." + s.Substring(2, 3) + "." + s.Substring(5, 3) + "/" + s.Substring(8, 4) + "-" + s.Substring(12, 2);
            return s;
        }

        public oPessoaFisica ToFisica()
        {
            oPessoaFisica pf = new oPessoaFisica();
            oPessoa pessoa = this;
            pessoa.CloneDeep(pf);
            pf._Apelido = this._NomeFantasia;
            pf._Nome = this._RazaoSocial;
            return pf;
        }

        public override bool isEmpty()
        {
            return string.IsNullOrWhiteSpace(dadosPrincipais);
        }

    }
}
