using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using Incipiens.Base.Model;
using Incipiens.Modulos.Pessoa.Object.Contato;
using System.Data.SqlTypes;
using System.ComponentModel.DataAnnotations.Schema;
using Incipiens.Base.Funcoes;
using System.Reflection;
using Incipiens.Modulos.Endereco.Object;
using System.Security.Cryptography;

namespace Incipiens.Modulos.Pessoa.Object
{
    [DisplayName("Pessoa Fisíca")]
    public class oPessoaFisica: oPessoa
    {
        public override string dadosPrincipais
        {
            get {
                if (!string.IsNullOrEmpty(_Nome) && !string.IsNullOrEmpty(_Apelido))
                    return _Apelido + "/" + _Nome;
                else if (string.IsNullOrEmpty(_Nome))
                    return _Apelido;
                return _Nome;
            }
        }

        public oPessoaFisica() : base()
        {
            _Cpf = null;
            _Apelido = "";
            _Nome = "";
            _DataNascimento = null;
        }

        #region Propriedades

        [DisplayName("CPF")]
        [CpfValidation]
        public string _Cpf
        {
            get { return GetValue(() => _Cpf); }
            set
            {
                SetValue(() => _Cpf, value);
                foreach (oContato contato in _Contatos)
                    if (contato._DadosContato != null)
                        CheckCpfContatoIgual(this, contato._DadosContato);
            }
        }

        [DisplayName("RG")]
        [MaxLength(15, ErrorMessage = "RG Inválido")]
        public string _Rg
        {
            get { return GetValue(() => _Rg); }
            set { SetValue(() => _Rg, value); }
        }

        [MaxLength(120, ErrorMessage = "Nome: São permitidos apenas 120 caracteres")]
        [DisplayName("Nome")]
        [NomeValidation]
        public string _Nome
        {
            get { return GetValue(() => _Nome); }
            set { 
                SetValue(() => _Nome, value);
                NotifyPropertyChanged("dadosPrincipais");
            }
        }

        [MaxLength(120, ErrorMessage = "Apelido: São permitidos apenas 120 caracteres")]
        [DisplayName("Apelido")]
        [ApelidoValidation]
        public string _Apelido
        {
            get { return GetValue(() => _Apelido); }


            set { 
                SetValue(() => _Apelido, value);
                NotifyPropertyChanged("dadosPrincipais");
            }
        }

        [DisplayName("Data Nascimento")]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "dd/MM/yyyy", HtmlEncode = true, NullDisplayText = "")]
        public DateTime? _DataNascimento
        {
            get { return GetValue(() => _DataNascimento); }
            set
            {
                SetValue(() => _DataNascimento, value);
                NotifyPropertyChanged("_Idade");
                if (_Idade < 0 || _Idade > 200)
                    AddError("_Idade", "Idade Inválida");
                else
                    RemoveError("_Idade");
            }
        }

        [DisplayName("Idade")]
        [NotMapped]
        public int _Idade
        {
            get
            {
                if (_DataNascimento == null)
                    return 0;
                else
                {
                    dateDiferency dateDifference = new dateDiferency(DateTime.Now, Convert.ToDateTime(_DataNascimento));
                    if (DateTime.Now > _DataNascimento)
                        return dateDifference.Years;
                    else
                        return dateDifference.Years * -1;
                }
            }
        }

        #endregion

        #region Validacao

        public class CpfValidation : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value != null)
                { 
                    string cpf = value.ToString();
                    if (!String.IsNullOrEmpty(cpf))
                        if (Incipiens.Base.Funcoes.Strings.ContemSomenteNumeros(cpf))
                            if (ValidaCPF(cpf))
                                return ValidationResult.Success;
                            else
                                return new ValidationResult("CPF Inválido");
                        else
                            return new ValidationResult("CPF deve conter somente números");
                }
                return ValidationResult.Success;
            }

            public static bool ValidaCPF(string cpf)
            {

                int iCont = 0, iInd = 0, iSoma = 0, iMult = 0, iResto, iDigi1, iDigi2;
                char cDigi1, cDigi2;
                int[] vetCPF = new int[10];
                int teste = cpf.Length;
                if (cpf.Length == 11)
                {
                    cDigi1 = cpf[9];
                    cDigi2 = cpf[10];
                    while (iCont <= 9)
                    {

                        vetCPF[iCont] = int.Parse(cpf[iCont].ToString());
                        iCont++;
                    }
                    iCont = 10;
                    iInd = 0;
                    while (iCont > 1)
                    {
                        iMult = vetCPF[iInd] * iCont;
                        iSoma = iSoma + iMult;
                        iInd++;
                        iCont--;
                    }
                    iResto = iSoma % 11;
                    if (iResto < 2)
                        iDigi1 = 0;
                    else
                        iDigi1 = 11 - iResto;
                    vetCPF[9] = iDigi1;
                    iCont = 11;
                    iInd = 0;
                    iSoma = 0;
                    while (iCont > 1)
                    {
                        iMult = vetCPF[iInd] * iCont;
                        iSoma = iSoma + iMult;
                        iInd++;
                        iCont--;
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

        public class NomeValidation : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                string error = "Nome ou Apelido devem ser preenchidos";
                var pf = (oPessoaFisica)validationContext.ObjectInstance;
                if (!String.IsNullOrEmpty((string)value))
                {
                    pf.RemoveError(informacoesPropriedade<oPessoaFisica>.GetNameProperty(p => p._Apelido), error);
                    return ValidationResult.Success;
                }
                if (!String.IsNullOrEmpty(pf._Apelido))
                    return ValidationResult.Success;
                return new ValidationResult(error);
            }
        }

        public class ApelidoValidation : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                string error = "Nome ou Apelido devem ser preenchidos";
                var pf = (oPessoaFisica)validationContext.ObjectInstance;
                if (!String.IsNullOrEmpty((string)value))
                {
                    pf.RemoveError(informacoesPropriedade<oPessoaFisica>.GetNameProperty(p => p._Nome), error);
                    return ValidationResult.Success;
                }
                if (!String.IsNullOrEmpty(pf._Nome))
                    return ValidationResult.Success;
                return new ValidationResult(error);
            }
        }

            #endregion


        public static string FormataCpf(string cpf)
        {
            string s = cpf.RemoverMascara();
            if (s.Length == 11)
                s = s.Substring(0, 3) + "." + s.Substring(3, 3) + "." + s.Substring(6, 3) + "-" + s.Substring(9, 2);
            return s;
        }

        public oPessoaJuridica ToPessoaJuridica()
        {
            oPessoaJuridica pf = new oPessoaJuridica();
            oPessoa thisP = this;
            thisP.CloneDeep(pf);
            pf._RazaoSocial = this._Nome;
            pf._NomeFantasia = this._Apelido;
            return pf;
        }

        public override bool isEmpty()
        {
            return string.IsNullOrWhiteSpace(dadosPrincipais);
        }
    }
}
