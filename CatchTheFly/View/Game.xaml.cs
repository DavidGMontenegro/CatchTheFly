using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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

        private MediaPlayer playCatchSound = new MediaPlayer();
        private MediaPlayer playgameOver = new MediaPlayer();

        Uri catchSound;
        Uri gameOver;

        private int spawnRate = 50;
        private bool playing = true;
        private double speed = 10;
        private int currentRate;
        private int score = 0;
        private int posX, posY;

        private List<Ellipse> removeEllipse = new List<Ellipse>();
        private List<Image> removeImage = new List<Image>();

        Random rand = new Random();
        Brush brush;

        public Game()
        {
            InitializeComponent();

            catchSound = new Uri("\\catchSound.mp3", UriKind.RelativeOrAbsolute);
            gameOver = new Uri("pack://siteoforigin:,,,/Sounds/gameOver.mp3");

            gameTimer.Tick += createObjects;
            gameTimer.Interval = TimeSpan.FromMilliseconds(25);

            fallingTimer.Tick += fallEvent;
            fallingTimer.Interval = TimeSpan.FromMilliseconds(50);
            gameTimer.Start();
            fallingTimer.Start();

            currentRate = spawnRate;

        }

        public void createObjects(object sender, EventArgs e)
        {

            currentRate -= 2;

            if (currentRate < 1 && playing)
            {
                currentRate = spawnRate;

                posX = rand.Next(0, (int)(MyCanvas.ActualWidth - 50));
                posY = 0;

                if (rand.Next(1, 4) == 1)
                {
                    brush = new SolidColorBrush(Color.FromRgb((byte)rand.Next(1, 255), (byte)rand.Next(1, 255), (byte)rand.Next(1, 255)));
                    Ellipse circle = new Ellipse
                    {
                        Tag = "circle",
                        Height = 50,
                        Width = 50,
                        Stroke = Brushes.Black,
                        StrokeThickness = 1,
                        Fill = brush
                    };

                    Canvas.SetLeft(circle, posX);
                    Canvas.SetTop(circle, posY);

                    MyCanvas.Children.Add(circle);
                }
                else
                {
                    Image image = new Image();

                    switch (rand.Next(1, 6))
                    {
                        case 1:
                            image.Source = new BitmapImage(new Uri("\\flyLogo.png", UriKind.RelativeOrAbsolute));
                            break;
                        case 2:
                            image.Source = new BitmapImage(new Uri("\\fly.png", UriKind.RelativeOrAbsolute));
                            break;
                        case 3:
                            image.Source = new BitmapImage(new Uri("\\insect.png", UriKind.RelativeOrAbsolute));
                            break;
                        case 4:
                            image.Source = new BitmapImage(new Uri("\\bee.png", UriKind.RelativeOrAbsolute));
                            break;
                        case 5:
                            image.Source = new BitmapImage(new Uri("\\mosquito.png", UriKind.RelativeOrAbsolute));
                            break;
                    }

                    image.Height = 50;
                    image.Width = 50;

                    Canvas.SetLeft(image, posX);
                    Canvas.SetTop(image, posY);

                    MyCanvas.Children.Add(image);
                }
            }
        }

        public void fallEvent(object sender, EventArgs e)
        {
            double leftCatcherPos = Canvas.GetLeft(Catcher) - (GameWindow.ActualWidth - 475) / 2;
            if (playing)
            {
                removeImage = MyCanvas.Children.OfType<Image>().ToList<Image>();
                foreach (var x in removeImage)
                {

                    Canvas.SetTop(x, Canvas.GetTop(x) + speed);
                    speed += 0.05;

                    if (Canvas.GetTop(x) >= (GameWindow.ActualHeight - 150))
                    {
                        if (Canvas.GetLeft(x) >= (Canvas.GetLeft(Catcher) - Catcher.ActualWidth + 10) && ((Canvas.GetLeft(x) + x.ActualWidth) <= Canvas.GetLeft(Catcher) + 2 * Catcher.ActualWidth - 10))
                        {
                            score += 10;
                            Score.Content = "Score: " + score;

                            playCatchSound.Open(catchSound);
                            playCatchSound.Play();
                        }
                        else
                        {
                            playing = false;
                            goToLaunchScreen();
                        }

                        MyCanvas.Children.Remove(x);
                    }
                }

                removeEllipse = MyCanvas.Children.OfType<Ellipse>().ToList<Ellipse>();
                foreach (var x in removeEllipse)
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + speed);

                    if (Canvas.GetTop(x) >= GameWindow.ActualHeight - 150)
                    {
                        if (Canvas.GetLeft(x) >= (Canvas.GetLeft(Catcher) - Catcher.ActualWidth + 30) && ((Canvas.GetLeft(x) + x.ActualWidth) <= Canvas.GetLeft(Catcher) + 2 * Catcher.ActualWidth - 30))
                        {
                            playing = false;
                            goToLaunchScreen();
                        }

                        MyCanvas.Children.Remove(x);
                    }
                }
            }
        }

        public void moveCatcher(object sender, MouseEventArgs mouseAction)
        {
            Point mousePosition = mouseAction.GetPosition(this);

            if (mousePosition.X > leftControlRectangle.ActualWidth && mousePosition.X < leftControlRectangle.ActualWidth + MyCanvas.ActualWidth)
            {
                Canvas.SetLeft(Catcher, mousePosition.X - leftControlRectangle.ActualWidth - (Catcher.ActualWidth / 2));
            }
        }

        public void goToLaunchScreen()
        {

            MyCanvas.Children.Clear();
            playgameOver.Open(gameOver);
            playgameOver.Play();

            MessageBoxResult result = MessageBox.Show("  GAME OVER  ", "Game over", MessageBoxButton.OK);

            Launch launch = new Launch();
            GameWindow.Close();

            launch.Show();



        }
    }
}
