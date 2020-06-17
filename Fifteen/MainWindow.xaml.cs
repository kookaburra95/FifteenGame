using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
            
            buttonShuffle.IsEnabled = false;
            buttonPause.IsEnabled = false;

            _timer.Tick += timer_Tick;
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (_stopwatch.IsRunning)
            {
                TimeSpan timeSpan = _stopwatch.Elapsed;
                _currentTime = $"{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
                labelTime.Content = _currentTime;
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

            if (_firstClick)
            {
                _stopwatch.Start();
                _timer.Start();

                _firstClick = false;
            }

            if (_game.IsSolved())
            {
                if (_stopwatch.IsRunning)
                {
                    _stopwatch.Stop();
                    _firstClick = true;
                }

                _gameStart = false;

                labelMoves.Content = $"You win, {_game.Moves} moves!";
                buttonStart.IsEnabled = false;
                EnabledGameButtons(false);

                buttonPause.IsEnabled = false;


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
                }
            }   
        }

        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            if (_firstGame)
            {
                _game.Start();
                ShowButtons();
                buttonShuffle.IsEnabled = true;
                _firstGame = false;
            }

            buttonStart.IsEnabled = false;
            EnabledGameButtons(true);
            buttonPause.IsEnabled = true;

            _gameStart = true;
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

            labelMoves.Content = $"{_game.Moves}";
        }

        private void ShowDigitAt(int digit, int x, int y)
        {
            var button = (Button) this.FindName("b" + x + y);

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
            ShowButtons();
            buttonStart.IsEnabled = true;
            EnabledGameButtons(false);
            buttonPause.IsEnabled = false;

            _stopwatch.Reset();
            labelTime.Content = "00:00";

            _firstClick = true;
            _gameStart = false;
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

        private void VisibleGameButtons(bool flag) //true - enabled, false - disabled
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

        private void buttonPause_Click(object sender, RoutedEventArgs e)
        {
            if (_pauseFlag)
            {
                Pause();
            }
            else
            {
                Continue();
            }
        }

        private void Pause()
        {
            buttonPause.Content = "Continue";
            _pauseFlag = false;
            buttonShuffle.IsEnabled = false;

            _stopwatch.Stop();

            EnabledGameButtons(false);
            VisibleGameButtons(false);
        }

        private void Continue()
        {
            buttonPause.Content = "Pause";
            _pauseFlag = true;
            buttonShuffle.IsEnabled = true;

            _stopwatch.Start();

            EnabledGameButtons(true);
            VisibleGameButtons(true);
        }

        private void Fifteen_Deactivated(object sender, EventArgs e)
        {
            if (_gameStart)
            {
                Pause();
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
    }
}