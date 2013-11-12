using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace XnaControls.Controls
{

    #region StringToIntConverter
    public class StringToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return int.Parse(value as string);
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
    }
    #endregion StringToIntConverter

    #region IntRangeRule Validation Rule
    public class IntRangeRule : ValidationRule
    {
        private int _min;
        private int _max;

        public IntRangeRule()
        {
        }

        public int Min
        {
            get { return _min; }
            set { _min = value; }
        }

        public int Max
        {
            get { return _max; }
            set { _max = value; }
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int val = 0;

            if (int.TryParse((string)value, out val) == false)
                return new ValidationResult(false,
                  "Please enter an integer.");

            if ((val < Min) || (val > Max))
            {
                return new ValidationResult(false,
                  "Please enter a number in the range: " + Min + " - " + Max + ".");
            }
            else
            {
                return new ValidationResult(true, null);
            }
        }
    }
    #endregion IntRangeRule Validation Rule

    #region NumberBox
    /// <summary>
    /// Interaction logic for NumberBox.xaml
    /// </summary>
    public partial class NumberBox : UserControl
    {

        #region NumberBox Dependency Properties
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(int), typeof(NumberBox), new PropertyMetadata(1));
        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty BoxLabelProperty = DependencyProperty.Register("BoxLabel", typeof(String), typeof(NumberBox), new PropertyMetadata(String.Empty));
        public String BoxLabel
        {
            get { return (String)GetValue(BoxLabelProperty); }
            set { SetValue(BoxLabelProperty, value); }
        }
        #endregion NumberBox Dependency Properties

        #region NumberBox Routed Events

        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NumberBox));

        public event RoutedEventHandler ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        void RaiseValueChangedEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(NumberBox.ValueChangedEvent);
            RaiseEvent(newEventArgs);
        }

        #endregion NumberBox Routed Events

        public NumberBox()
        {
            DataContext = this;
            InitializeComponent();
        }

        #region NumberBox Event Handlers
        private void IncreaseValueButton_Click(object sender, RoutedEventArgs e)
        {
            RepeatButton b = sender as RepeatButton;

            if (Value >= 999)
                return;

            Value += 1;

            RaiseValueChangedEvent();
        }

        private void DecreaseValueButton_Click(object sender, RoutedEventArgs e)
        {
            RepeatButton b = sender as RepeatButton;

            if (Value <= 1)
                return;

            Value -= 1;

            RaiseValueChangedEvent();
        }

        private void InputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Validation.GetHasError(sender as TextBox) == true)
                return;

            RaiseValueChangedEvent();
        }
        #endregion NumberBox Event Handlers
    }
    #endregion NumberBox
}
