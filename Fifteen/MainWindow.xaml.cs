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
using FifteenLibrary;

namespace Fifteen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int size = 4;
        private Game game;
        private bool firstGame = true;

        public MainWindow()
        {
            InitializeComponent();
            game = new Game(size);
            HideButtons();
            
            buttonShuffle.IsEnabled = false;
            buttonRecords.IsEnabled = false;
        }

        private void b_Click(object sender, RoutedEventArgs e)
        {
            if (game.IsSolved())
            {
                return;
            }

            Button button = (Button)sender;

            int x = int.Parse(button.Name.Substring(1, 1));
            int y = int.Parse(button.Name.Substring(2, 1));

            game.CLickAt(x, y);
            ShowButtons();

            if (game.IsSolved())
            {
                labelMoves.Content = $"You win, {game.Moves} moves!";
                buttonStart.IsEnabled = false;
            }
        }

        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            if (firstGame)
            {
                game.Start();//1000 + DateTime.Now.DayOfYear);
                ShowButtons();
                buttonShuffle.IsEnabled = true;
                firstGame = false;
            }

            buttonStart.IsEnabled = false;
            EnabledGameButtons(true);
        }

        private void HideButtons()
        {
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    ShowDigitAt(0, x, y);
                }
            }
        }

        private void ShowButtons()
        {
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    ShowDigitAt(game.GetDigitAt(x, y), x, y);
                }
            }

            labelMoves.Content = $"{game.Moves}";
        }

        private void ShowDigitAt(int digit, int x, int y)
        {
            var button = (Button) this.FindName("b" + x + y);

            if (digit == 0)
            {
                button.Content = "";
            }
            else
            {
                button.Content = digit.ToString();
            }
            
            button.IsEnabled = digit > 0;
        }   

        private void buttonShuffle_Click(object sender, RoutedEventArgs e)
        {
            game.Shuffle();
            ShowButtons();
            buttonStart.IsEnabled = true;
            EnabledGameButtons(false);
        }

        private void EnabledGameButtons(bool flag) //true - enabled, false - disabled
        {
            var buttons = new List<Button>()
            {
                b00, b01, b02, b03,
                b10, b11, b12, b13,
                b20, b21, b22, b23,
                b30, b31, b32, b33
            };

            foreach (var button in buttons) 
            {
                if (flag)
                {
                    button.IsEnabled = true;
                }
                else
                {
                    button.IsEnabled = false;
                }
            }
        }
    }
}