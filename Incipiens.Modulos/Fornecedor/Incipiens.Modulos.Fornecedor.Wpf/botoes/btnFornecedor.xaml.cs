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

namespace Incipiens.Modulos.Fornecedor.Wpf.botoes
{
    /// <summary>
    /// Interação lógica para btnFornecedor.xam
    /// </summary>
    public partial class btnFornecedor : UserControl
    {
        public btnFornecedor()
        {
            InitializeComponent();
        }
        public Button btn
        {
            get { return botao; }
        }
    }
}
