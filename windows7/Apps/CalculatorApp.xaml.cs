using System.Windows;
using System.Windows.Controls;

namespace ChromeOS.Apps
{
    public partial class CalculatorApp : UserControl
    {
        private double _currentValue;
        private double _previousValue;
        private string _operation = "";
        private bool _isNewEntry = true;

        public CalculatorApp()
        {
            InitializeComponent();
        }

        private void UpdateDisplay()
        {
            Display.Text = _currentValue.ToString(_currentValue % 1 == 0 ? "N0" : "G");
        }

        private void OnNumberClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                if (_isNewEntry)
                {
                    _currentValue = double.Parse(btn.Content.ToString()!);
                    _isNewEntry = false;
                }
                else
                {
                    _currentValue = _currentValue * 10 + double.Parse(btn.Content.ToString()!);
                }
                UpdateDisplay();
            }
        }

        private void OnDecimalClick(object sender, RoutedEventArgs e)
        {
            if (_isNewEntry)
            {
                _currentValue = 0;
                _isNewEntry = false;
            }
        }

        private void OnClearClick(object sender, RoutedEventArgs e)
        {
            _currentValue = 0;
            _previousValue = 0;
            _operation = "";
            _isNewEntry = true;
            UpdateDisplay();
        }

        private void OnNegateClick(object sender, RoutedEventArgs e)
        {
            _currentValue = -_currentValue;
            UpdateDisplay();
        }

        private void OnPercentClick(object sender, RoutedEventArgs e)
        {
            _currentValue /= 100;
            UpdateDisplay();
        }

        private void OnAddClick(object sender, RoutedEventArgs e) => SetOperation("+");
        private void OnSubtractClick(object sender, RoutedEventArgs e) => SetOperation("-");
        private void OnMultiplyClick(object sender, RoutedEventArgs e) => SetOperation("*");
        private void OnDivideClick(object sender, RoutedEventArgs e) => SetOperation("/");

        private void SetOperation(string op)
        {
            if (_operation != "")
            {
                Calculate();
            }
            _previousValue = _currentValue;
            _operation = op;
            _isNewEntry = true;
        }

        private void OnEqualsClick(object sender, RoutedEventArgs e)
        {
            Calculate();
            _operation = "";
            _isNewEntry = true;
        }

        private void Calculate()
        {
            switch (_operation)
            {
                case "+": _currentValue = _previousValue + _currentValue; break;
                case "-": _currentValue = _previousValue - _currentValue; break;
                case "*": _currentValue = _previousValue * _currentValue; break;
                case "/": 
                    if (_currentValue != 0)
                        _currentValue = _previousValue / _currentValue; 
                    break;
            }
            UpdateDisplay();
        }
    }
}
