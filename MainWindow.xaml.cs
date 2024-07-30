using Cerise_Lommeregner.View;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;



namespace Cerise_Lommeregner
{

    public partial class MainWindow : Window
    {
        decimal usdRate = 684.64m;

        decimal eurRate = 745.71m;

        decimal gbpRate = 873.09m;

        decimal Valutakurs = 1.01m;

        public MainWindow()
        {
            LoadXmlData("https://www.nationalbanken.dk/api/currencyratesxmlhistory?lang=da");
            InitializeComponent();

        }

        private void txtPris_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtPris.Text == "Indtast Pris" || txtPris.Text == "Webpris")
            {
                txtPris.Text = "";
                txtPris.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#542e3d"));
            }
        }

        private void txtPris_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPris.Text))
            {
                txtPris.Text = "Indtast Pris";
            }
            else if (txtPris.Text == "Webpris")
            {
                txtPris.Text = "Webpris";
            }

            txtPris.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#542e3d"));
        }


        private void LoadXmlData(string url)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    string xmlData = client.DownloadString(url);
                    XDocument doc = XDocument.Parse(xmlData);

                    XNamespace ns = "http://www.ecb.int/vocabulary/2002-08-01/eurofxref";
                    var cubeNodes = doc.Descendants(ns + "Cube")
                                       .Where(cube => cube.Attribute("time") != null);

                    foreach (var cubeNode in cubeNodes.Elements(ns + "Cube"))
                    { 
                        string currency = cubeNode.Attribute("currency").Value;
                        string rate = cubeNode.Attribute("rate").Value;

                        switch (currency)
                        {
                            case "USD":
                                usdRate = decimal.Parse(rate);
                                break;
                            case "EUR":
                                eurRate = decimal.Parse(rate);
                                break;
                            case "GBP":
                                gbpRate = decimal.Parse(rate);
                                break;
                        }
                    }


                }
            }
            catch
            {
                ErrorMessageNONET();
            }
        }







        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }


        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
        private string selectedValue;

        private void DropDownMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)DropDownMenu.SelectedItem;
            selectedValue = selectedItem.Content.ToString();
        }
        private void ErrorMessage()
        {
            ErrorBox errorBox = new ErrorBox();
            errorBox.SetErrorMessage("Indtast korrekt Pris");
            errorBox.Show();
        }
        private void ErrorMessageNONET()
        {
            ErrorBox errorBox = new ErrorBox();
            errorBox.SetErrorMessage("Ingen forbindelse\nGenerel Kurs");
            errorBox.Show();
        }
        private void Del_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                txtPris.Text = "";
            }
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (Vis1.Visibility == Visibility.Visible)
                {

                    decimal rate;
                    if (selectedValue == "EUR")
                    {
                        rate = eurRate * Valutakurs;
                    }
                    else if (selectedValue == "USD")
                    {
                        rate = usdRate * Valutakurs;
                    }
                    else if (selectedValue == "GBP")
                    {
                        rate = gbpRate * Valutakurs;
                    }
                    else if (selectedValue == "DKK")
                    {
                        rate = 100;
                    }
                    else
                    {
                        ErrorMessage();
                        return;
                    }

                    decimal pris;
                    if (decimal.TryParse(txtPris.Text, out pris) && !txtPris.Text.Contains("."))
                    {
                        UdenMoms.Text = (pris * rate / 100).ToString("N2") + " KR";
                        MedMoms.Text = ((pris * rate / 100) * 3.125m).ToString("N2") + " KR";
                    }
                    else
                    {
                        ErrorMessage();
                    }
                }
                else
                {
                    decimal pris;

                    if (decimal.TryParse(txtPris.Text, out pris) && !txtPris.Text.Contains("."))
                    {

                        MedMoms.Text = (pris / 3.125m).ToString("N2") + " KR";

                    }
                    else
                    {
                        ErrorMessage();
                    }
                }
            }
        }


        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            decimal rate;
            if (selectedValue == "EUR")
            {
                rate = eurRate * Valutakurs;
            }
            else if (selectedValue == "USD")
            {
                rate = usdRate * Valutakurs;
            }
            else if (selectedValue == "GBP")
            {
                rate = gbpRate * Valutakurs;
            }
            else if (selectedValue == "DKK")
            {
                rate = 100;
            }
            else
            {
                ErrorMessage();
                return;
            }

            decimal pris;


            if (decimal.TryParse(txtPris.Text, out pris) && !txtPris.Text.Contains("."))
            {

                UdenMoms.Text = (pris * rate / 100).ToString("N2") + " KR";
                MedMoms.Text = ((pris * rate / 100) * 3.125m).ToString("N2") + " KR";

            }
            else
            {
                ErrorMessage();
            }
        }



        private void btnOn_Checked(object sender, RoutedEventArgs e)
        {
            DockPanel.SetDock(btnOff, Dock.Right);
            Vis1.Visibility = Visibility.Collapsed;

            BowClick.Visibility = Visibility.Collapsed;
            BowClick1.Visibility = Visibility.Visible;
            Total.Opacity = 0;
            UdenMoms.Opacity = 0;
            Totalmoms.Text = "Indkøbs pris: ";
            Copyright.Margin = new Thickness(180, 10, 0, 0);
            txtPris.Text = "Webpris";
            MedMoms.Text = "";
        }

        private void btnOn_Unchecked(object sender, RoutedEventArgs e)
        {
            DockPanel.SetDock(btnOff, Dock.Left);
            Vis1.Visibility = Visibility.Visible;

            BowClick.Visibility = Visibility.Visible;
            BowClick1.Visibility = Visibility.Collapsed;

            Total.Opacity = 0.7;
            UdenMoms.Opacity = 0.7;
            Total.Text = "Kost pris: ";
            Totalmoms.Text = "Web pris: ";
            Copyright.Margin = new Thickness(180, -20, 0, 0);
            MedMoms.Text = "";
            txtPris.Text = "IndtastPris";
            // TEXT TEST
            // TEXT TEST// TEXT TEST
            // TEXT TEST
            // TEXT TEST
            // TEXT TEST
            // TEXT TEST

        }
        private void btnOff_Checked(object sender, RoutedEventArgs e)
        {


        }
        private void btnOff_Unchecked(object sender, RoutedEventArgs e)
        {



        }
        private void BowClick1_MouseDown(object sender, MouseButtonEventArgs e)
        {

            decimal pris;

            if (decimal.TryParse(txtPris.Text, out pris) && !txtPris.Text.Contains("."))
            {

                MedMoms.Text = (pris / 3.125m).ToString("N2") + " KR";

            }
            else
            {
                ErrorMessage();
            }

        }
    }
}

//Dette er en test...
