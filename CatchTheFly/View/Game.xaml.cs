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
using System.Windows.Threading;

namespace CatchTheFly.View
{
    /// <summary>
    /// Lógica de interacción para Game.xaml
    /// </summary>
    public partial class Game : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();
        DispatcherTimer fallingTimer = new DispatcherTimer();

        private int spawnRate = 50;
        private int currentRate;
        private int score = 0;
        private int posX, posY;
        private bool playing = true;
        private int fallingSpeed = 10;

        Random rand = new Random();
        Brush brush;

        public Game()
        {
            InitializeComponent();

            gameTimer.Tick += fallingLoop;
            gameTimer.Interval = TimeSpan.FromMilliseconds(10);
            gameTimer.Start();

            /*
            fallingTimer.Tick += fallEvent;
            fallingTimer.Interval = TimeSpan.FromSeconds(20);
            fallingTimer.Start();
            */

            currentRate = spawnRate;
        }

        public void fallingLoop(object sender, EventArgs e)
        {
            

            playing = true;

            currentRate -= 2;

            if (currentRate < 1)
            {
                currentRate = spawnRate;

                posX = rand.Next(10, 700);
                posY = 0;

                brush = new SolidColorBrush(Color.FromRgb((byte)rand.Next(1,255), (byte)rand.Next(1, 255), (byte)rand.Next(1, 255)));
                Ellipse circle = new Ellipse
                {
                    Tag = "circle",
                    Height = 20,
                    Width = 20,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                    Fill = brush
                };

                Canvas.SetLeft(circle, posX);
                Canvas.SetTop(circle, posY);

                MyCanvas.Children.Add(circle);

                score++;
                Score.Content = "Score: " + score;

                foreach (var x in MyCanvas.Children.OfType<Ellipse>())
                {

                    Canvas.SetTop(x, Canvas.GetTop(x) + 20);

                    if (Canvas.GetBottom(x) == Application.Current.MainWindow.Height)
                    {
                        MyCanvas.Children.Remove(x);
                    }
                }
            }
        }
    }
}
