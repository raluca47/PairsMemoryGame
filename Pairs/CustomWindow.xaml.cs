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
using System.Windows.Shapes;

namespace Pairs
{
    /// <summary>
    /// Interaction logic for CustomWindow.xaml
    /// </summary>
    public partial class CustomWindow : Window
    {
        public CustomWindow()
        {
            InitializeComponent();
        }

        public event EventHandler<DataEnteredEvent> DataEntered;

        private void Button_Click(object sender, RoutedEventArgs e) // Submit Button
        {
            int rows = int.Parse(rowsBox.Text);
            int columns = int.Parse(columnsBox.Text);
            if (rows * columns % 2 == 0)
            {
                DataEntered?.Invoke(this, new DataEnteredEvent(rows, columns));
            }
            else
            {
                MessageBox.Show("The number of tiles should be divisible by 2!", "Error", MessageBoxButton.OK);
            }
            this.Close();
        }
    }
}
