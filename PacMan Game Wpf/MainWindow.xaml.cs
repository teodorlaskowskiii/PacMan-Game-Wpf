using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PacMan_Game_Wpf
{
    public partial class MainWindow : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();
        DateTime startTime; 

        bool goLeft, goRight, goDown, goUp;
        bool noLeft, noRight, noDown, noUp;
        int speed = 5;
        Rect pacmanHitBox;
        int ghostSpeed = 6;
        int ghostMoveStep = 150;
        int GhostRotation = 0;
        int currentGhostStep;
        int score = 0;

        int minutes = 0;
        int seconds = 0;

        public MainWindow()
        {
            InitializeComponent();
            GameSetUp();
        }

        private void MyCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left && noLeft == false)
            {
                goRight = goUp = goDown = false;
                noRight = noUp = noDown = false;

                goLeft = true;

                pacman.RenderTransform = new RotateTransform(-180, pacman.Width / 2, pacman.Height / 2);


            }

            if (e.Key == Key.Right && noRight == false)
            {
                noLeft = noUp = noDown = false;
                goLeft = goUp = goDown = false;

                goRight = true;

                pacman.RenderTransform = new RotateTransform(0, pacman.Width / 2, pacman.Height / 2);
            }

            if (e.Key == Key.Up && noUp == false)
            {
                noRight = noDown = noLeft = false;
                goRight = goDown = goLeft = false;

                goUp = true;

                pacman.RenderTransform = new RotateTransform(-90, pacman.Width / 2, pacman.Height / 2);
            };

            if (e.Key == Key.Down && noDown == false)
            {
                noUp = noLeft = noRight = false;
                goUp = goLeft = goRight = false;

                goDown = true;
                pacman.RenderTransform = new RotateTransform(90, pacman.Width / 2, pacman.Height / 2);
            };

        }

        private void GameSetUp()
        {
            MyCanvas.Focus();

            gameTimer.Tick += GameLoop;
            startTime = DateTime.Now;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            gameTimer.Start();
            currentGhostStep = ghostMoveStep;

            

            // Set up images for pacman and ghosts
            ImageBrush pacmanImage = new ImageBrush();
            pacmanImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/pacman.jpg"));
            pacman.Fill = pacmanImage;

            ImageBrush redGhostI = new ImageBrush();
            redGhostI.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/red.jpg"));
            redGhost.Fill = redGhostI;

            ImageBrush orangeGhostI = new ImageBrush();
            orangeGhostI.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/orange.jpg"));
            orangeGhost.Fill = orangeGhostI;

            ImageBrush pinkGhostI = new ImageBrush();
            pinkGhostI.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/pink.jpg"));
            pinkGhost.Fill = pinkGhostI;
        }

        private void GameLoop(object sender, EventArgs e)
        {
            txtScore.Content = "Score: " + score;

            
            //TimeSpan elapsedTime = DateTime.Now - startTime;
            UpdateElapsedTime();

            if (goRight)
            {
                Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) + speed);

            }
            if (goLeft)
            {
                Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) - speed);

            }
            if(goUp)
            {
                Canvas.SetTop(pacman, Canvas.GetTop(pacman) - speed);
            }
            if(goDown)
            {
                Canvas.SetTop(pacman, Canvas.GetTop(pacman) + speed);
            }

            if (goDown && Canvas.GetTop(pacman) + 80 > Application.Current.MainWindow.Height)
            {
                
                noDown = true;
                goDown = false;
            }
            if (goUp && Canvas.GetTop(pacman) < 1)
            {
                
                noUp = true;
                goUp = false;
            }
            if (goLeft && Canvas.GetLeft(pacman) - 10 < 1)
            {
                
                noLeft = true;
                goLeft = false;
            }
            if (goRight && Canvas.GetLeft(pacman) + 70 > Application.Current.MainWindow.Width)
            {
                
                noRight = true;
                goRight = false;
            }

            pacmanHitBox = new Rect(Canvas.GetLeft(pacman), Canvas.GetTop(pacman), pacman.Width, pacman.Height);

            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
               
                Rect hitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height); 
                
                //walls
                if ((string)x.Tag == "wall")
                {
                    
                    if (goLeft == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) + 10);
                        noLeft = true;
                        goLeft = false;
                    }
                    
                    if (goRight == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) - 10);
                        noRight = true;
                        goRight = false;
                    }
                    
                    if (goDown == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(pacman, Canvas.GetTop(pacman) - 10);
                        noDown = true;
                        goDown = false;
                    }
                    
                    if (goUp == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(pacman, Canvas.GetTop(pacman) + 10);
                        noUp = true;
                        goUp = false;
                    }
                }

                //coins
                if ((string) x.Tag == "coin")
                {
                    if(pacmanHitBox.IntersectsWith(hitBox) && x.Visibility == Visibility.Visible)
                    {
                        x.Visibility = Visibility.Hidden;
                        score++;
                    }

                }

                //ghost

                if((string) x.Tag == "ghost")
                {
                    if (pacmanHitBox.IntersectsWith(hitBox))
                    {
                        GameOver("FAILED", minutes, seconds,false);
                    }
                    //ORANGE
                    if (x.Name.ToString() == "orangeGhost" && GhostRotation == 0)
                    {
                       
                        Canvas.SetTop(x, Canvas.GetTop(x) - ghostSpeed);
                    }
                    if (x.Name.ToString() == "orangeGhost" && GhostRotation == 3)
                    {

                        Canvas.SetTop(x, Canvas.GetTop(x) - ghostSpeed);
                    }
                    if (x.Name.ToString() == "orangeGhost" && GhostRotation == 1)
                    {

                        Canvas.SetLeft(x, Canvas.GetLeft(x) - ghostSpeed);
                    }
                    if (x.Name.ToString() == "orangeGhost" && GhostRotation == 2)
                    {

                        Canvas.SetLeft(x, Canvas.GetLeft(x) - ghostSpeed);
                    }
                    //PINK
                    if (x.Name.ToString() == "pinkGhost" && GhostRotation == 0)
                    {

                        Canvas.SetLeft(x, Canvas.GetLeft(x) - ghostSpeed);
                    }
                    if (x.Name.ToString() == "pinkGhost" && GhostRotation == 3)
                    {

                        Canvas.SetTop(x, Canvas.GetTop(x) + ghostSpeed);
                    }
                    if (x.Name.ToString() == "pinkGhost" && GhostRotation == 1)
                    {

                        Canvas.SetLeft(x, Canvas.GetLeft(x) - ghostSpeed);
                    }
                    if (x.Name.ToString() == "pinkGhost" && GhostRotation == 2)
                    {

                        Canvas.SetTop(x, Canvas.GetTop(x) + ghostSpeed);
                    }

                    //RED
                    if (x.Name.ToString() == "redGhost")
                    {
                        
                        Canvas.SetLeft(x, Canvas.GetLeft(x) - ghostSpeed);
                    }
                    
                    currentGhostStep--;
                    
                    if (currentGhostStep < 1)
                    {
                        
                        currentGhostStep = ghostMoveStep;
                        
                        ghostSpeed = -ghostSpeed;

                        GhostRotation++;
                        if (GhostRotation > 3)
                        {
                            GhostRotation = 0;
                        }
                    }
 
                }

            }
            if (score == 110)
            {
                
                GameOver("SUCCESS", minutes, seconds,true);
            }


        }

        private void UpdateElapsedTime()
        {
            
            TimeSpan elapsedTime = DateTime.Now - startTime;

            
            minutes = elapsedTime.Minutes;
            seconds = elapsedTime.Seconds;

           
            Application.Current.Dispatcher.Invoke(() =>
            {
                txtTime.Content = "";
                txtTime.Content = $"Time: {minutes:D2}:{seconds:D2}"; 

                //Console.WriteLine($"Updating elapsed time: {DateTime.Now}");
            });
        }
        private void GameOver(string massage, int min, int sec, bool win)
        {
            gameTimer.Stop();
            MessageBox.Show(massage + " in Time of " + min +":" + sec, "The PAC MAN game");
            //System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            //Application.Current.Shutdown();
            if (win)
            {
                var menuWindow = new MenuWindow(min, sec);
                menuWindow.Show();
            }
            else
            {
                var menuWindow = new MenuWindow(100,100);
                menuWindow.Show();
            }
            
            this.Close();
        }
    }
}
