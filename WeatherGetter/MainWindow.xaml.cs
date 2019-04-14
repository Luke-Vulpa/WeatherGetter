using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
//using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WeatherGetter.Services;
using Newtonsoft;
using WeatherGetter.Models.WeatherServiceModels;
using System.Diagnostics;


namespace WeatherGetter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static HttpClient Client;
        PostcodeService _postcode;
        WeatherService weatherService;


        public MainWindow()
        {
            InitializeComponent();
            Client = new HttpClient();
            ResultDataGrid.Visibility = Visibility.Hidden;
            SiteGrid.Visibility = Visibility.Hidden;
        }


        private async void PostcodeButton_Click(object sender, RoutedEventArgs e)
        {
            _clearStatus();

            //debug
            //SiteGrid.ShowGridLines = true;

            if (_validatePostcode())
            {
                // get long lat from api
                _postcode = new PostcodeService(PostcodeTextBox.Text);
                var result = await _postcode.GetLongLatAsync();
                
                if(result == null)
                {
                    _updateStatus("Unknown postcode or service error");
                }
                else
                {
                    //do something with Coordinate result...
                    // get sitelist iterate over list

                    var locationService = new LocationService(result);
                    // if in range add to new list 
                    //

                    
                    _buildLocationUI(locationService.ValidLocations);

                    //ResultTextBlock.Text = locationService.ValidLocations[0].Name.ToString();
                    //locationService.ValidLocations[0].Id.
                }

                

            }
        }

        #region Helpers

        private void PostcodeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _clearStatus();
            _validatePostcode();
        }

        private void _updateStatus(string message)
        {
            StatusTextBlock.Text = message;
            StatusTextBlock.Foreground = Brushes.Tomato;
            StatusTextBlock.UpdateLayout();        
            

        }

        private void _clearStatus()
        {
            StatusTextBlock.Text = "";
            StatusTextBlock.Foreground = Brushes.Black;
            clearWeatherResults();
            clearLocationButtons();

        }

        private bool _validatePostcode()
        {
            if (PostcodeTextBox.Text == "")
            {

                ///empty postcode
                _updateStatus("Postcode cannot be empty");
                return false;
            }
            else
            {
                ///check for invalid chars
                foreach (char character in PostcodeTextBox.Text)
                {
                    if (!Char.IsLetterOrDigit(character) && !Char.IsWhiteSpace(character))
                    {
                        _updateStatus("Postcode must be alphanumeric");
                        return false;                        
                    }
                }
            }

            return true;

        }

        private void _buildLocationUI(Models.LocationModels.Location[] locations)
        {

            for (int i = 0; i < locations.Length - 1; i++)
            {
                Button button = new Button();
                button.Content = locations[i].Name.ToString();

                // site Id is added to tag so weather service can reference
                button.Tag = locations[i].Id.ToString();
                button.Padding = new Thickness(5);
                button.Margin = new Thickness(5);
                

                button.Click += new RoutedEventHandler(_LocationButton_Click);

                SiteGrid.Children.Add(button);
                SiteGrid.UpdateLayout();
            }

            SiteGrid.Visibility = Visibility.Visible;
            
        }

        private async void _LocationButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            
            weatherService = new WeatherService(btn.Tag.ToString());

            await weatherService.getWeatherModelDataAsync();

            _buildWeatherResultsUI(weatherService.SiteRep);
        }

        private void _buildWeatherResultsUI(SiteRep ws)
        {
            SiteGrid.Visibility = Visibility.Hidden;
            ResultDataGrid.Visibility = Visibility.Visible;

            DateValueLabel.Content = ws.Dv.DataDate.Date.ToString();

            //Day Values

            DayWindSpeedValueLabel.Content = ws.Dv.Location.Period[0].Rep[0].S.ToString() + "/" + ws.Dv.Location.Period[0].Rep[0].D.ToString();
            DayPrecipitationValueLabel.Content = ws.Dv.Location.Period[0].Rep[0].PPd.ToString() + " %";
            DayWeatherTypeValueLabel.Content = ws.Dv.Location.Period[0].Rep[0].W.ToString();
            DayTemperatureFeelsLikeValueLabel.Content = ws.Dv.Location.Period[0].Rep[0].FDm.ToString() + "°C";

            //Night Values
            NightWindSpeedValueLabel.Content = ws.Dv.Location.Period[0].Rep[1].S.ToString() + "/" + ws.Dv.Location.Period[0].Rep[1].D.ToString(); 
            NightPrecipitationValueLabel.Content = ws.Dv.Location.Period[0].Rep[1].PPn.ToString() + " %";
            NightWeatherTypeValueLabel.Content = ws.Dv.Location.Period[0].Rep[1].W.ToString();
            NightTemperatureFeelsLikeValueLabel.Content = ws.Dv.Location.Period[0].Rep[1].FNm.ToString() + "°C";
        }

        private void clearWeatherResults()
        {
            ResultDataGrid.Visibility = Visibility.Hidden;
            DayWindSpeedValueLabel.Content = "";
            DayPrecipitationValueLabel.Content = ""; 
            DayWeatherTypeValueLabel.Content = "";
            DayTemperatureFeelsLikeValueLabel.Content = "";


            NightWindSpeedValueLabel.Content = "";
            NightPrecipitationValueLabel.Content = "";
            NightWeatherTypeValueLabel.Content = "";
            NightTemperatureFeelsLikeValueLabel.Content = "";

        }

        private void clearLocationButtons()
        {

            for(int i = SiteGrid.Children.Count - 1; i >= 0; i--)
            {
                if(SiteGrid.Children[i] is Button)
                {
                    SiteGrid.Children.RemoveAt(i);
                }
            }

        }

        #endregion

        private void MenuItemAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutPopup.IsOpen = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PopupButton_Click(object sender, RoutedEventArgs e)
        {
            AboutPopup.IsOpen = false;
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
