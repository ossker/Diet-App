using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Services.Maps;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;


namespace dietApp
{
    public sealed partial class pogoda : Page
    {
        public pogoda()
        {
            this.InitializeComponent();
            GdzieJestem();
           
        }

        List<weatherAPI> danePogodowe = new List<weatherAPI>();
        List<locationAPI> daneLokalizacji = new List<locationAPI>();

        double szerokosc, dlugosc;

        private async void btnWyjscie_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new ContentDialog()
            {
                Title = "Czy na pewno chcesz wyjść?",
                PrimaryButtonText = "NIE",
                SecondaryButtonText = "TAK",
                DefaultButton = ContentDialogButton.Primary

            };

            var ss = new Windows.Media.SpeechSynthesis.SpeechSynthesizer();
            var stream = await ss.SynthesizeTextToStreamAsync(dlg.Title.ToString());
            MediaElement me = new MediaElement();
            me.SetSource(stream, stream.ContentType);

            var wynik = await dlg.ShowAsync();
            if (wynik == ContentDialogResult.Secondary)
                App.Current.Exit();
        }

        async void GdzieJestem()
        {
            try
            {
                var mojGPS = new Geolocator()
                {
                    DesiredAccuracy = PositionAccuracy.High
                };
                var wynik = await mojGPS.GetGeopositionAsync();
                szerokosc = wynik.Coordinate.Point.Position.Latitude;
                dlugosc = wynik.Coordinate.Point.Position.Longitude;

                var szerokosc_text = szerokosc.ToString().Split(",");
                var dlugosc_text = dlugosc.ToString().Split(",");

                string danePogoda = "http://api.weatherapi.com/v1/current.xml?key=2141f86e7bae4e0887d214103220106&q=" + szerokosc_text[0] + "." + szerokosc_text[1] + "," + dlugosc_text[0] + "." + dlugosc_text[1];
                var http = new HttpClient();

                var dane = await http.GetStringAsync(new Uri(danePogoda));

                XDocument daneXml = XDocument.Parse(dane);

                danePogodowe = (from item in daneXml.Descendants("current")
                                select new weatherAPI()
                                {
                                    temperature = item.Element("temp_c").Value,
                                    feels_like = item.Element("feelslike_c").Value,
                                    humidity = item.Element("humidity").Value,
                                    pressure = item.Element("pressure_mb").Value,
                                    wind_kph = item.Element("wind_kph").Value

                                }).ToList();

                daneLokalizacji = (from item in daneXml.Descendants("location")
                                   select new locationAPI()
                                   {
                                       miasto = item.Element("name").Value,
                                       kraj = item.Element("country").Value,
                                       data = item.Element("localtime").Value,
                                   }).ToList();

                lbPogoda.ItemsSource = danePogodowe;
                lbLokalizacja.ItemsSource = daneLokalizacji;
                prLokalizacja.Visibility = Visibility.Collapsed;
                prLPogoda.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }


        //NAVIGATION

        private void btnDoDomu_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage), null, new DrillInNavigationTransitionInfo());
        }

        private void btnDoKalkulatoraKalorii_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(kalkulatorKalorii), null, new DrillInNavigationTransitionInfo());
        }

        private void btnDoDziennikaZywnosci_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(dziennikZywnosci), null, new DrillInNavigationTransitionInfo());
        }

        private void btnDoKalendarzaDiety_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(kalendarzDiety), null, new DrillInNavigationTransitionInfo());
        }

        private void btnDoZdjec_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(porownanieZdjec), null, new DrillInNavigationTransitionInfo());
        }
    }
}
