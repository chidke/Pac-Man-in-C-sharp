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

namespace PAC_Man_Game_WPF_MOO_ICT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DispatcherTimer gameTimer = new DispatcherTimer();

        bool goLeft,goRight,goDown,goUp;
        bool noLeft,noRight,noDown,noUp;

        int speed = 8;

        Rect pacmanHitBox;                              // Everything from Dispatcher timer to score all the 
                                                        // Variables that need to be defined for the code
        int ghostSpeed = 10;
        int ghostMoveStep = 170;
        int currentGhostStep;
        int score = 0;
       
        
        
        public MainWindow()
        {
            InitializeComponent();

            GameSetUp();                           // Whenever game resets it will call back to game reset up
        }

        private void CanvasKeyDown(object sender, KeyEventArgs e)
        {
               if (e.Key == Key.Left && noLeft == false)
            {
                goRight = goUp = goDown = false;
                noRight = noUp = noDown = false;                       // These codes are saying that we the person wants to make pacman go left will it go left is its true
                                                                       // pacman cant go anywhere else because it false
                goLeft = true;

                pacman.RenderTransform =new RotateTransform(0, pacman.Width/2, pacman.Height/2); // This code here is rotating pacman to turn left (pacman is rotating from its center -180 degrees to turn left)
            }


            if (e.Key == Key.Right && noRight == false)
            {
                noLeft = noDown = noLeft = false;
                goRight = goDown= goLeft = false;                       // These codes are saying that we the person wants to make pacman go Right will it go Right is its true
                                                                       // pacman cant go anywhere else because it false
                goRight = true;

                pacman.RenderTransform = new RotateTransform(-180, pacman.Width / 2, pacman.Height / 2); // This code here is rotating pacman to turn right (pacman is rotating from its center 0 degrees to turn Right)
            }


            if (e.Key == Key.Up && noUp == false)
            {
                noRight = noDown = noLeft = false;
                goRight = goDown = goLeft = false;                       // These codes are saying that we the person wants to make pacman go Up will it go Up is its true
                                                                         // pacman cant go anywhere else because it false
                goUp = true;

                pacman.RenderTransform = new RotateTransform(90, pacman.Width / 2, pacman.Height / 2); // This code here is rotating pacman to turn Up (pacman is rotating from its center 90 degrees to turn Up)
            }


            if (e.Key == Key.Down && noDown == false)
            {
                noUp = noLeft = noRight = false;
                goUp = goLeft= goRight= false;                       // These codes are saying that we the person wants to make pacman go Down will it go Up is its true
                                                                         // pacman cant go anywhere else because it false
                goDown = true;

                pacman.RenderTransform = new RotateTransform(-90, pacman.Width / 2, pacman.Height / 2); // This code here is rotating pacman to turn Down (pacman is rotating from its center -90 degrees to turn Down)
            }

        }

        private void GameSetUp()
        {
            MyCanvas.Focus();                   // The Canvas Keydown event will register when we press a key

            gameTimer.Tick += GameLoop;        // We are setting up the clock
            gameTimer.Interval = TimeSpan.FromMilliseconds(20); // The clock will tick every millisecond
            gameTimer.Start();       // This starts the timer
            currentGhostStep = ghostMoveStep;

            ImageBrush pacmanImage= new ImageBrush();
            pacmanImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/pacman.jpg"));
            pacman.Fill = pacmanImage;

            ImageBrush redGhost = new ImageBrush();                                                                 // The brush images thing imports the pacman/ ghost images
            redGhost.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/red.jpg"));
            redGuy.Fill = redGhost;

            ImageBrush orangeGhost = new ImageBrush();
            orangeGhost.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/orange.jpg"));
            orangeGuy.Fill = orangeGhost;

            ImageBrush pinkGhost = new ImageBrush();
            pinkGhost.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/pink.jpg"));
            pinkGuy.Fill = pinkGhost;

            ImageBrush fruit = new ImageBrush();
            fruit.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/apple-removebg-preview.png"));
            apple.Fill = fruit;
        }

        private void GameLoop(object sender, EventArgs e)
        {
            txtScore.Content = " Score " + score;      // calculates the score in real time

            if(goRight)
            {
                Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) + speed);  // when the pacman is going right, the left side of the canvas will add speed (or space) to the pacman to go right
            }

            if (goLeft)
            {
                Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) -speed);  // when the pacman is going left, the left side of the canvas will subtract speed (or space) from the pacman to go left
            }

            if (goUp)
            {
                Canvas.SetTop(pacman, Canvas.GetTop(pacman) - speed);  // when the pacman is going up, the top side of the canvas will subtract speed (or space) from the pacman to go up
            }

            if (goDown)
            {
                Canvas.SetTop(pacman, Canvas.GetTop(pacman) + speed);  // when the pacman is going down, the top side of the canvas will add speed (or space) to the pacman to go down
            }


            if (goDown && Canvas.GetTop(pacman) + 80 > Application.Current.MainWindow.Height)   // (more) This sets the boundaries for down. the floor starts from 0 when the number gets bigger the floor (boundaries) are elevated
            {
                noDown = true;
                goDown = false;
            }

            if (goUp && Canvas.GetTop(pacman) < 100)  // (add more to the number)    ^^^ (read the top one)
            {
                noUp = true;
                goUp = false;
            }

            if (goLeft && Canvas.GetLeft(pacman) -80< 1) // leave the - and make number bigger  ^^^
            {
                noLeft = true;
                goLeft = false;
            }

            if (goRight && Canvas.GetLeft(pacman) + 65 > Application.Current.MainWindow.Width)  // less to the number ^^^
            {
                noRight = true;
                goRight = false;
            }


            pacmanHitBox = new Rect(Canvas.GetLeft(pacman), Canvas.GetTop(pacman), pacman.Width, pacman.Height);

            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                Rect hitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height); 

                if((string)x.Tag == "Wall")  // x.Tag == "Wall" means that all the functions below will affect the wall
                {
                    if (goLeft == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) + 10);
                        noLeft = true;
                        goRight = false;
                    }

                    if (goRight == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetLeft(pacman, Canvas.GetLeft(pacman)-10);                          // adds borders to the walls in the game ^^^ VVV
                        noRight= true;
                        goLeft = false;
                    }

                    if (goDown ==true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(pacman, Canvas.GetTop(pacman) - 10);
                        noDown= true;
                        goDown = false;
                    }

                    if (goUp ==true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(pacman, Canvas.GetTop(pacman) + 10);
                        noUp= true;
                        goUp= false;
             
                    
                     }
          
                
                }
            
                    if ((string) x.Tag =="coin") // x.Tag == "coin" means that all the functions below will affect the coin
                    {
                        if (pacmanHitBox.IntersectsWith(hitBox) && x.Visibility == Visibility.Visible) // This function makes the coins disappear
                        {
                           x.Visibility = Visibility.Hidden;
                           score++;                                    // When the coins disappear the score goes up   //61
                        }
                    }

                    if ((string)x.Tag == "fruit") // x.Tag == "fruit" means that all the functions below will affect the fruit
                    {
                        if (pacmanHitBox.IntersectsWith(hitBox) && x.Visibility == Visibility.Visible) // This function makes the fruits disappear
                        {
                        x.Visibility = Visibility.Hidden;
                       // score++;                                    // When the coins disappear the score goes up   //61
                        }
                    }

                if ((string) x.Tag == "ghost")
                   {  
                        if (pacmanHitBox.IntersectsWith(hitBox))
                        {
                              GameOver("You Lose, Click ok to play again");
                        }
                        if (x.Name.ToString() == "orangeGuy")
                        {
                               Canvas.SetLeft(x, Canvas.GetLeft(x) - ghostSpeed);
                        }

                        else
                        {
                               Canvas.SetLeft(x, Canvas.GetLeft(x) + ghostSpeed);

                        }

                        currentGhostStep--;
                        
                        if (currentGhostStep < 1)
                        {
                                currentGhostStep = ghostMoveStep;
                                ghostSpeed = -ghostSpeed;
                        }
                
                
                   }
                
            
            }

                     if (score == 61)
                     {
                        GameOver("You Win, You collected all the coins");
                     }
        
        
        }


        private void PowerUp()
        {

        }

        private void MakeApple()
        {

        }

        private void GameOver(string message)
        {
            gameTimer.Stop();
            MessageBox.Show(message, "The PACMAN GAME");

            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location); // This helps restart the game
            Application.Current.Shutdown();                                          // This is showdown the previous game that was played

        }
    }
}
