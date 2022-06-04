using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

//Szablon elementu Pusta strona jest udokumentowany na stronie https://go.microsoft.com/fwlink/?LinkId=234238

namespace dietApp
{
    /// <summary>
    /// Pusta strona, która może być używana samodzielnie lub do której można nawigować wewnątrz ramki.
    /// </summary>
    public sealed partial class kalendarzDiety : Page
    {
        public kalendarzDiety()
        {
            this.InitializeComponent();
            try
            {
                
                dpKalendarz.SelectedDate = DateTime.Now.Date;
                dataGridKalendarz.ItemsSource = PosilkiDataLib.GetRecordsByData(dpKalendarz.Date.ToString("d") + " 00:00:00");
                tbKalorie.Text = PosilkiDataLib.GetKalorieByData(dpKalendarz.Date.ToString("d") + " 00:00:00")[0].ToString();
                
            }
            catch
            {
                tbKalorie.Text = "Brak danych";
            }
        }

        

        

        private void dpKalendarz_SelectedDateChanged(DatePicker sender, DatePickerSelectedValueChangedEventArgs args)
        {
            DatePicker datePicker = sender as DatePicker;

            try
            {
                dataGridKalendarz.ItemsSource = PosilkiDataLib.GetRecordsByData(datePicker.Date.ToString("d") + " 00:00:00");
                tbKalorie.Text = PosilkiDataLib.GetKalorieByData(datePicker.Date.ToString("d") + " 00:00:00")[0].ToString();
            }
            catch
            {
                tbKalorie.Text = "Brak danych";
            }
        }

        private void btnDoKalkulatoraKalorii_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(kalkulatorKalorii), null, new DrillInNavigationTransitionInfo());
        }

        private void btnDoDomu_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage), null, new DrillInNavigationTransitionInfo());
        }

        private void btnDoDziennikaZywnosci_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(dziennikZywnosci), null, new DrillInNavigationTransitionInfo());
        }

        private void btnDoZdjec_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(porownanieZdjec), null, new DrillInNavigationTransitionInfo());
        }

        private void btnDoPogody_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(pogoda), null, new DrillInNavigationTransitionInfo());
        }

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

        
    }
}
