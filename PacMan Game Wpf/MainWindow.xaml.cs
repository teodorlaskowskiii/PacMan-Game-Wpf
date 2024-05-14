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
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Threading;

namespace PacMan_Game_Wpf
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();

        bool goLeft, goRight, goDown, goUp; 
        bool noLeft, noRight, noDown, noUp; 
        int speed = 8; 
        Rect pacmanHitBox; 
        int ghostSpeed = 10; 
        int ghostMoveStep = 160; 
        int currentGhostStep; 
        int score = 0; 




        public MainWindow()
        {
            InitializeComponent();
        }

        private void MyCanvas_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void GameSetUp()
        {

        }

        private void GameOver()
        {

        }
    }
}
