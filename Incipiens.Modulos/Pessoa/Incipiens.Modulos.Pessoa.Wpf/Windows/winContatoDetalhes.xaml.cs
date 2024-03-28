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

using Incipiens.Modulos.Pessoa.Object.Contato;
using Incipiens.Modulos.Pessoa.Database;
using Incipiens.Modulos.Pessoa.Object;

using Incipiens.Base.Wpf.CustomControlLibrary.Outros;

using Incipiens.Base.Model;
using Incipiens.Base.GerenciadorEF.InterfacesEntityFramework;

using Incipiens.Base.Funcoes;
using Incipiens.Base.Wpf.CustomControlLibrary.Mensagens;


namespace Incipiens.Modulos.Pessoa.Wpf.Windows
{
    /// <summary>
    /// Lógica interna para winContatoDetalhes.xaml
    /// </summary>
    public partial class winContatoDetalhes : Window
    {
        private oContato contatoRetorno;
        private oContato dataContext;

        public winContatoDetalhes(oContato contato)
        {
            InitializeComponent();

            contatoRetorno = contato;
            dataContext = new oContato();
            this.DataContext = dataContext;
            contato.CloneDeep(dataContext);
            menuConfirmar.lblConfirmar.Content = "F1-Confirmar";
            menuConfirmar.btnConfirmar.Click += BtnConfirmar_Click;
            menuConfirmar.btnVoltar.Click += BtnVoltar_Click;

            this.KeyDown += WinContatoDetalhes_KeyDown;
        }

        private void WinContatoDetalhes_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F1:
                    Confirmar();
                    break;

                case Key.Escape:
                    Sair();
                    break;
            }
        }

        private void Sair()
        {
            if (MessagesBoxPadroes.msgSairSemSalvar() == MessageBoxResult.Yes)
                this.Close();
        }
        private void BtnVoltar_Click(object sender, RoutedEventArgs e)
        {
            Sair();
        }

        private void Confirmar()
        {
            if (dataContext.HasErrors)
                dataContext.msgErrosValidacao();
            else
            {
                dataContext.CloneDeep(contatoRetorno);
                this.Close();
            }
        }
        private void BtnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            Confirmar();
        }
    }
}
