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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Incipiens.Modulos.Cliente.Wpf.botoes
{
    /// <summary>
    /// Interação lógica para btnCliente.xam
    /// </summary>
    public partial class btnCliente : UserControl
    {
        public btnCliente()
        {
            InitializeComponent();
        }
        public Button btn
        {
            get { return botao; }
        }
    }
}
