using Incipiens.Base.Wpf.Controllers;
using Incipiens.Modulos.Fornecedor.Database;
using Incipiens.Modulos.Fornecedor.Object;
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

namespace Incipiens.Modulos.Fornecedor.Wpf.Windows
{
    /// <summary>
    /// Lógica interna para winCadastroFornecedorFisica.xaml
    /// </summary>
    public partial class winCadastroFornecedorFisica : Window
    {
        public winCadastroFornecedorFisica(oFornecedor fornecedor)
        {
            InitializeComponent();
            new ControllerWinSalvar<oFornecedor>(
               this,
               new dbFornecedor(),
               menuSalvar,
               fornecedor);
        }
    }
}
