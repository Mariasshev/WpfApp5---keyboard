using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _generatedText = string.Empty;
        private int _currentIndex = 0;
        private int _errors = 0;
        private DateTime _startTime;

        public MainWindow()
        {
            InitializeComponent();
            GenerateKeyboard();
        }

        private void GenerateKeyboard()
        {
            string[] keys = { "`1234567890-=Backspace", "Tabqwertyuiop[]\\", "CapsLocksasdfghjkl;'", "Shiftzxcvbnm,./", "Ctrl Win Alt Space Alt Win Ctrl" };
            foreach (var row in keys)
            {
                foreach (char key in row)
                {
                    var button = new Button
                    {
                        Content = key.ToString(),
                        Margin = new Thickness(2),
                        Width = 50,
                        Height = 50,
                        IsEnabled = false // Не интерактивные клавиши
                    };
                    KeyboardGrid.Children.Add(button);
                }
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            _generatedText = GenerateRandomString((int)DifficultySlider.Value, CaseSensitiveCheckBox.IsChecked == true);
            GeneratedString.Text = _generatedText;
            _currentIndex = 0;
            _errors = 0;
            _startTime = DateTime.Now;

            FailsText.Text = "0";
            SpeedText.Text = "0 chars/min";

            StartButton.IsEnabled = false;
            StopButton.IsEnabled = true;
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            StopTraining();
        }

        private void StopTraining()
        {
            StartButton.IsEnabled = true;
            StopButton.IsEnabled = false;
            GeneratedString.Text = string.Empty;
        }

        private string GenerateRandomString(int difficulty, bool caseSensitive)
        {
            var random = new Random();
            string chars = caseSensitive ? "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789" : "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, difficulty).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrEmpty(_generatedText) || _currentIndex >= _generatedText.Length)
                return;

            char expectedChar = _generatedText[_currentIndex];
            char typedChar = e.Key.ToString().Length == 1 ? e.Key.ToString()[0] : '\0';

            if (expectedChar == typedChar)
            {
                _currentIndex++;
            }
            else
            {
                _errors++;
                FailsText.Text = _errors.ToString();
            }

            if (_currentIndex == _generatedText.Length)
            {
                var timeTaken = DateTime.Now - _startTime;
                double speed = _generatedText.Length / timeTaken.TotalMinutes;
                SpeedText.Text = $"{Math.Round(speed)} chars/min";
                StopTraining();
            }
        }
    }
}