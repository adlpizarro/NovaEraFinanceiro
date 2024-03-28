using Incipiens.Base.Wpf.Controllers;
using Incipiens.Base.Wpf.Temas.Menus;
using Incipiens.Modulos.Cliente.Database;
using Incipiens.Modulos.Cliente.Object;
using Incipiens.Modulos.Pessoa.Object;
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
    /// Lógica interna para winCadastroCliente.xaml
    /// </summary>
    public partial class winCadastroClienteFisica : Window
    {
        public winCadastroClienteFisica(oCliente cliente)
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
