using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CatchTheFly.View
{
    /// <summary>
    /// Lógica de interacción para Launch.xaml
    /// </summary>
    public partial class Launch : Window
    {
        public Launch()
        {
            InitializeComponent();
        }

        public void StartGame(object sender, RoutedEventArgs e)
        {
            Game game = new Game();
            game.Show();
            this.Close();
        }
    }
}
