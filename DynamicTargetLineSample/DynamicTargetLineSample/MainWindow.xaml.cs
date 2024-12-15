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

namespace DynamicTargetLineSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _previousValidText = string.Empty;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Y_Axis == null) return;
            var maxValue = Y_Axis.Maximum;

            if (sender is TextBox textBox)
            {
                textBox.TextChanged -= TextBox_TextChanged;

                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    viewModel.Y1 = double.MinValue;
                    textBox.Text = string.Empty;
                }
                else
                {
                    if (int.TryParse(textBox.Text, out int newValue))
                    {
                        if (newValue > maxValue)
                            newValue = (int)maxValue;
                        else if (newValue < 0)
                            newValue = 0;

                        viewModel.Y1 = newValue;

                        textBox.Text = newValue.ToString();
                        textBox.CaretIndex = textBox.Text.Length;
                    }
                    else
                    {
                        textBox.Text = ((int)viewModel.Y1).ToString();
                        textBox.CaretIndex = textBox.Text.Length;
                    }
                }

                textBox.TextChanged += TextBox_TextChanged;
            }
        }
    }
}