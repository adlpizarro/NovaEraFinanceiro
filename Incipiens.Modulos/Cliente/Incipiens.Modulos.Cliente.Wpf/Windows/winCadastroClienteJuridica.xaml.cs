using Incipiens.Base.Wpf.Controllers;
using Incipiens.Modulos.Cliente.Database;
using Incipiens.Modulos.Cliente.Object;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Incipiens.Modulos.Cliente.Wpf.Windows
{
    /// <summary>
    /// Lógica interna para winCadastroClienteJuridica.xaml
    /// </summary>
    public partial class winCadastroClienteJuridica : Window
    {
        public winCadastroClienteJuridica(oCliente cliente)
        {
            InitializeComponent();
            new ControllerWinSalvar<oCliente>(
               this,
               new dbCliente(),
               menuSalvar,
               cliente);
        }
    }
}
