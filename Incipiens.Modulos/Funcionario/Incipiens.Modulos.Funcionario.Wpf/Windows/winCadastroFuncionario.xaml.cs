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

using Incipiens.Base.Wpf.Controllers;
using Incipiens.Modulos.Funcionario.Object;
using Incipiens.Modulos.Funcionario.Database;

namespace Incipiens.Modulos.Funcionario.Wpf.Windows
{
    /// <summary>
    /// Lógica interna para winCadastroFuncionario.xaml
    /// </summary>
    public partial class winCadastroFuncionario : Window
    {
        public winCadastroFuncionario(oFuncionario func)
        {
            InitializeComponent();

            vFunc = func;

            new ControllerWinSalvar<oFuncionario>(
                this,
                new dbFuncionario(),
                menuSalvar, func);
        }

        private oFuncionario vFunc;

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.F5)
            {
                MessageBox.Show(vFunc.HasErrors.ToString());
            }
        }
    }
}
