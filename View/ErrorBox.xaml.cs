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

namespace Cerise_Lommeregner.View
{
    /// <summary>
    /// Interaction logic for ErrorBox.xaml
    /// </summary>
    public partial class ErrorBox : Window
    {
        public ErrorBox()
        {
            InitializeComponent();
        }

        private void btnOkay_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        public void SetErrorMessage(string errorMessage)
        {
            txtErrorMessage.Text = errorMessage;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            this.Close();
        }
    }
}
 
