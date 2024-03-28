using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

using Incipiens.Base.Funcoes;

namespace Incipiens.Modulos.Pessoa.Wpf.Conversores         
{
    public class TelefoneConversor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                string s = value.ToString();
                s = Pessoa.Object.Contato.oTelefone.FormataTelefone(s);
                return s;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString().RemoverMascara();
        }

    }
}
