using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Incipiens.Base.Funcoes;

namespace Incipiens.Modulos.Endereco.Conversores
{
    public class CepConversor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Endereco.Object.oEndereco.FormataCep(value.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Strings.RemoverMascara(value.ToString());
        }

    }
}
