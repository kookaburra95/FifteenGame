using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Fifteen.Services;
using FifteenLibrary;

namespace Fifteen
{
    public partial class MainWindow : Window
    {
        private const int Size = 4;
        private readonly Game _game;
        private bool _firstGame = true;
        private bool _pauseFlag = true;
        private bool _firstClick = true;    
        private bool _gameStart = false;

        readonly DispatcherTimer _timer = new DispatcherTimer();
        readonly Stopwatch _stopwatch = new Stopwatch();
        private string _currentTime = String.Empty;

        private readonly string _path = $"{Environment.CurrentDirectory}\\recordsList.json";
        private BindingList<Records> _record = new BindingList<Records>();
        private FIleIOService _fileIoService;

        public MainWindow()
        {   
            InitializeComponent();
            _game = new Game(Size); 
            HideButtons();
            
            ButtonShuffle.IsEnabled = false;
            ButtonPause.IsEnabled = false;

            _timer.Tick += timer_Tick;
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 1);

            //UI
            ButtonPause.Visibility = Visibility.Hidden;
            ButtonShuffle.Visibility = Visibility.Hidden;
            Grid.SetColumnSpan(ButtonStart, 4);
            Grid.SetColumnSpan(ButtonRecords, 4);
            Grid.SetColumn(ButtonRecords,0);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (_stopwatch.IsRunning)
            {
                TimeSpan timeSpan = _stopwatch.Elapsed;
                _currentTime = $"{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
                LabelTime.Content = _currentTime;
            }
        }

        private void b_Click(object sender, RoutedEventArgs e)
        {
            if (_game.IsSolved())
            {
                return;
            }

            Button button = (Button)sender;

            int x = int.Parse(button.Name.Substring(1, 1));
            int y = int.Parse(button.Name.Substring(2, 1));

            _game.CLickAt(x, y);
            ShowButtons();
            ColoredGameButtons();

            if (_firstClick)
            {
                _stopwatch.Start();
                _timer.Start();

                _firstClick = false;

                ButtonPause.IsEnabled = true;
            }

            if (_game.IsSolved())
            {
                if (_stopwatch.IsRunning)
                {
                    _stopwatch.Stop();
                    _firstClick = true;
                }

                _gameStart = false;

                LabelMoves.Content = $"You win, {_game.Moves} moves!";
                ButtonStart.IsEnabled = false;
                EnabledGameButtons(false);

                ButtonPause.IsEnabled = false;


                NameWindow nw = new NameWindow();

                if (nw.ShowDialog() == true)
                {
                    _fileIoService = new FIleIOService(_path);

                    try
                    {
                        _record = _fileIoService.LoadData();
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message);
                        Close();
                    }

                    _record.Add(new Records((nw.UserName).TrimStart(), _game.Moves, _currentTime, (_stopwatch.Elapsed).TotalMilliseconds));

                    try
                    {
                        _fileIoService.SaveData(_record);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message);
                        Close();
                    }

                    if (!IsWindowOpen<WindowRecords>())
                    {
                        WindowRecords records = new WindowRecords();
                        records.Show();
                    }

                    //UI
                    ButtonRecords.Visibility = Visibility.Visible;
                    Grid.SetColumnSpan(ButtonRecords, 2);
                    Grid.SetColumn(ButtonRecords, 2);

                    Grid.SetColumnSpan(ButtonShuffle, 2);
                }
            }   
        }

        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            if (_firstGame)
            {
                _game.Start();
                ShowButtons();
                ColoredGameButtons();

                ShuffleAnimation();

                ButtonShuffle.IsEnabled = true;
                _firstGame = false;
            }

            ButtonStart.IsEnabled = false;
            EnabledGameButtons(true);
            ButtonPause.IsEnabled = true;

            _gameStart = true;

            //UI
            ButtonPause.Visibility = Visibility.Visible;
            ButtonShuffle.Visibility = Visibility.Visible;

            ButtonStart.Visibility = Visibility.Hidden;
            ButtonRecords.Visibility = Visibility.Hidden;

            ButtonPause.IsEnabled = false;

            Grid.SetColumnSpan(ButtonShuffle, 4);
            Grid.SetColumnSpan(ButtonPause, 4);
            Grid.SetColumn(ButtonPause, 0);
        }

        private void HideButtons()
        {
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    ShowDigitAt(0, x, y);
                }
            }
        }

        private void ShowButtons()
        {
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    ShowDigitAt(_game.GetDigitAt(x, y), x, y);
                }
            }

            LabelMoves.Content = $"{_game.Moves}";
        }

        private void ShowDigitAt(int digit, int x, int y)
        {
            var button = (Button) this.FindName("B" + x + y);

            if (digit == 0)
            {
                if (button != null) button.Content = "";
            }
            else
            {
                if (button != null) button.Content = digit.ToString();
            }

            if (button != null) button.IsEnabled = digit > 0;
        }   

        private void buttonShuffle_Click(object sender, RoutedEventArgs e)
        {
            _game.Shuffle();

            ShuffleAnimation();

            ShowButtons();
            ColoredGameButtons();

            

            ButtonStart.IsEnabled = true;
            EnabledGameButtons(false);
            ButtonPause.IsEnabled = false;

            _stopwatch.Reset();
            LabelTime.Content = "00:00";

            _firstClick = true;
            _gameStart = false;

            //UI
            ButtonPause.Visibility = Visibility.Hidden;
            ButtonStart.Visibility = Visibility.Visible;
            ButtonRecords.Visibility = Visibility.Visible;

            Grid.SetColumnSpan(ButtonRecords, 2);
            Grid.SetColumn(ButtonRecords, 2);

            Grid.SetColumnSpan(ButtonShuffle, 2);
        }

        private void EnabledGameButtons(bool flag) //true - enabled, false - disabled
        {
            foreach (var button in GetGameButtons()) 
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

        private void VisibleGameButtons(bool flag) //true - enabled, false - disabled
        {
            foreach (var button in GetGameButtons())
            {
                if (flag)
                {
                    ShowButtons();
                }
                else
                {
                    for (int x = 0; x < Size; x++)
                    {
                        for (int y = 0; y < Size; y++)
                        {
                            button.Content = "";
                        }
                    }
                }
            }   
        }

        private void ColoredGameButtons()
        {
            foreach (var button in GetGameButtons())
            {
                switch (button.Content)
                {
                    case "1": case "2": case "3": case "4": button.Foreground = (LinearGradientBrush)TryFindResource("FirstRowGradient"); break;
                    case "5": case "6": case "7": case "8": button.Foreground = (LinearGradientBrush)TryFindResource("SecondRowGradient"); break;
                    case "9": case "10": case "11": case "12": button.Foreground = (LinearGradientBrush)TryFindResource("ThirdRowGradient"); break;
                    case "13": case "14": case "15": case "": button.Foreground = (LinearGradientBrush)TryFindResource("FourthRowGradient"); break;
                }
            }
        }

        private void buttonPause_Click(object sender, RoutedEventArgs e)
        {
            if (_pauseFlag)
            {
                Pause();

                //UI
                ButtonShuffle.Visibility = Visibility.Hidden;
                ButtonRecords.Visibility = Visibility.Visible;

                ButtonPause.Background = (SolidColorBrush) TryFindResource("ButtonStartBackground");
                ButtonPause.Foreground = (SolidColorBrush) TryFindResource("ButtonStartForeground");

                Grid.SetColumnSpan(ButtonRecords,4);
                Grid.SetColumn(ButtonRecords,0);
            }
            else
            {
                Continue();

                //UI
                ButtonShuffle.Visibility = Visibility.Visible;
                ButtonRecords.Visibility = Visibility.Hidden;

                ButtonPause.Background = (SolidColorBrush) TryFindResource("ButtonPauseBackground");
                ButtonPause.Foreground = (SolidColorBrush) TryFindResource("ButtonPauseForeground");
            }
        }

        private void Pause()
        {
            ButtonPause.Content = "▶ Continue";
            _pauseFlag = false;
            ButtonShuffle.IsEnabled = false;

            _stopwatch.Stop();

            EnabledGameButtons(false);
            VisibleGameButtons(false);
        }

        private void Continue()
        {
            ButtonPause.Content = "𝅛𝅛 Pause";
            _pauseFlag = true;
            ButtonShuffle.IsEnabled = true;

            _stopwatch.Start();

            EnabledGameButtons(true);
            VisibleGameButtons(true);
        }

        private void Fifteen_Deactivated(object sender, EventArgs e)
        {
            if (_gameStart)
            {
                if (!_firstClick)
                {
                    Pause();

                    //UI
                    ButtonShuffle.Visibility = Visibility.Hidden;
                    ButtonRecords.Visibility = Visibility.Visible;

                    ButtonPause.Background = (SolidColorBrush)TryFindResource("ButtonStartBackground");
                    ButtonPause.Foreground = (SolidColorBrush)TryFindResource("ButtonStartForeground");

                    Grid.SetColumnSpan(ButtonRecords, 4);
                    Grid.SetColumn(ButtonRecords, 0);
                }
            }
        }

        private void buttonRecords_Click(object sender, RoutedEventArgs e)
        {
            if (!IsWindowOpen<WindowRecords>())
            {
                var fileExist = File.Exists(_path);

                if (fileExist)
                {
                    WindowRecords records = new WindowRecords();
                    records.Show();
                }
                else
                {
                    MessageBox.Show("Records does not exist!\nPlay first game!");
                }
            }
        }
            
        public static bool IsWindowOpen<T>(string name = "") where T : Window
        {
            if (string.IsNullOrEmpty(name))
            {
                return Application.Current.Windows.OfType<T>().Any();
            }
            else
            {
                return Application.Current.Windows.OfType<T>().Any(w => w.Name.Equals(name));
            }
        }

        private List<Button> GetGameButtons()
        {
            var buttons = new List<Button>()
            {
                B00, B01, B02, B03,
                B10, B11, B12, B13,
                B20, B21, B22, B23,
                B30, B31, B32, B33
            };

            return buttons;
        }

        private void ShuffleAnimation()
        {
            foreach (var button in GetGameButtons())
            {
                var doubleAnimation = new DoubleAnimation(360, 0, new Duration(TimeSpan.FromSeconds(0.5)));
                var rotateTransform = new RotateTransform();

                button.RenderTransform = rotateTransform;
                button.RenderTransformOrigin = new Point(0.5, 0.5);
                rotateTransform.BeginAnimation(RotateTransform.AngleProperty, doubleAnimation);
            }
        }
    }
}