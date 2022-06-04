using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Text.RegularExpressions;
using Windows.UI;
using Windows.UI.Xaml.Media.Animation;

namespace dietApp
{
    public sealed partial class kalkulatorKalorii : Page
    {
        public kalkulatorKalorii()
        {
            this.InitializeComponent();
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("CPM")
                && ApplicationData.Current.LocalSettings.Values.ContainsKey("bialka")
                && ApplicationData.Current.LocalSettings.Values.ContainsKey("tluszcze")
                && ApplicationData.Current.LocalSettings.Values.ContainsKey("weglowodany"))
            {
                
                DaneKalorii.CPM = (double)ApplicationData.Current.LocalSettings.Values["CPM"];
                DaneKalorii.bialka = (double)ApplicationData.Current.LocalSettings.Values["bialka"];
                DaneKalorii.tluszcze = (double)ApplicationData.Current.LocalSettings.Values["tluszcze"];
                DaneKalorii.weglowodany = (double)ApplicationData.Current.LocalSettings.Values["weglowodany"];
                
                zmienVisibility(borderPodsumowanie, borderDane, borderAktywnosc, borderCel);
               
                tbWyliczoneKalorie.Text = DaneKalorii.CPM.ToString() + " kcal";
                tbWyliczoneBialka.Text = DaneKalorii.bialka.ToString() + " g";
                tbWyliczoneTluszcze.Text = DaneKalorii.tluszcze.ToString() + " g";
                tbWyliczoneWeglowodany.Text = DaneKalorii.weglowodany.ToString() + " g";
            }
            else
            {
                zmienVisibility(borderDane, borderAktywnosc, borderCel, borderPodsumowanie);
            }

        }

       
        private void zmienVisibility(Border wlacz, Border wylacz1, Border wylacz2, Border wylacz3)
        {
            wlacz.Visibility = Visibility.Visible;
            wylacz1.Visibility = Visibility.Collapsed;
            wylacz2.Visibility = Visibility.Collapsed;
            wylacz3.Visibility = Visibility.Collapsed;
        }

        private void btnDalejDane_Click(object sender, RoutedEventArgs e)
        {
            
            if(cbPlec.SelectedItem != null && !String.IsNullOrEmpty(tbWiek.Text) 
                && !String.IsNullOrEmpty(tbWaga.Text) && !String.IsNullOrEmpty(tbWzrost.Text))
            {
                DaneKalorii.plec = (cbPlec.SelectedItem as ComboBoxItem).Tag.ToString();
                DaneKalorii.wiek = Int32.Parse(tbWiek.Text);
                DaneKalorii.waga = Int32.Parse(tbWaga.Text);
                DaneKalorii.wzrost = Int32.Parse(tbWzrost.Text);
                zmienVisibility(borderAktywnosc, borderDane, borderCel, borderPodsumowanie);
                tbWiek.Background = new SolidColorBrush(Colors.LightGreen);
                tbWaga.Background = new SolidColorBrush(Colors.LightGreen);
                tbWzrost.Background = new SolidColorBrush(Colors.LightGreen);
                cbPlec.Background = new SolidColorBrush(Colors.LightGreen);
            }
            else
            {
                if (String.IsNullOrEmpty(tbWiek.Text))
                    tbWiek.Background = new SolidColorBrush(Colors.Pink);
                else
                    tbWiek.Background = new SolidColorBrush(Colors.Gainsboro);
                if (String.IsNullOrEmpty(tbWaga.Text))
                    tbWaga.Background = new SolidColorBrush(Colors.Pink);
                else
                    tbWaga.Background = new SolidColorBrush(Colors.Gainsboro);
                if (String.IsNullOrEmpty(tbWzrost.Text))
                    tbWzrost.Background = new SolidColorBrush(Colors.Pink);
                else
                    tbWzrost.Background = new SolidColorBrush(Colors.Gainsboro);
                if (cbPlec.SelectedItem == null)
                    cbPlec.Background = new SolidColorBrush(Colors.Pink);
                else
                    cbPlec.Background = new SolidColorBrush(Colors.Gainsboro);

            }
  
        }
        private void btnDalejAktywnosc_Click(object sender, RoutedEventArgs e)
        {
            if(cbPlec.SelectedItem != null)
            {
                zmienVisibility(borderCel, borderDane, borderAktywnosc, borderPodsumowanie);
            }
            
        }

        private void btnWrocAktywnosc_Click(object sender, RoutedEventArgs e)
        {
            zmienVisibility(borderDane, borderAktywnosc, borderCel, borderPodsumowanie);
        }

        private void btnOdNowa_Click(object sender, RoutedEventArgs e)
        {
            zmienVisibility(borderDane, borderPodsumowanie, borderAktywnosc, borderCel);
        }

        private void btnWrocCel_Click(object sender, RoutedEventArgs e)
        {
            zmienVisibility(borderAktywnosc, borderDane, borderCel, borderPodsumowanie);
        }

        private void btnObliczCel_Click(object sender, RoutedEventArgs e)
        {

            zmienVisibility(borderPodsumowanie, borderAktywnosc, borderDane, borderCel);
            tbWyliczoneKalorie.Text = Math.Round(DaneKalorii.ObliczKalorie()).ToString() + " kcal";
            tbWyliczoneBialka.Text = Math.Round(DaneKalorii.ObliczBialka()).ToString() + " g";
            tbWyliczoneTluszcze.Text = Math.Round(DaneKalorii.ObliczTluszcze()).ToString() + " g";
            tbWyliczoneWeglowodany.Text = Math.Round(DaneKalorii.ObliczWeglowodany()).ToString() + " g";


            ApplicationData.Current.LocalSettings.Values["CPM"] = Math.Round(DaneKalorii.ObliczKalorie());
            ApplicationData.Current.LocalSettings.Values["bialka"] = Math.Round(DaneKalorii.ObliczBialka());
            ApplicationData.Current.LocalSettings.Values["tluszcze"] = Math.Round(DaneKalorii.ObliczTluszcze());
            ApplicationData.Current.LocalSettings.Values["weglowodany"] = Math.Round(DaneKalorii.ObliczWeglowodany());
        }

        private void RadioButtonCel_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rbCel = sender as RadioButton;

            string stringCel = rbCel.Tag.ToString();

            if (rbCel != null)
            {
                switch (stringCel)
                {
                    case "schudnac":
                        DaneKalorii.dodatek = (DaneKalorii.plec == "M") ? -300 : -200;
                        break;
                    case "utrzymac":
                        DaneKalorii.dodatek = 0;
                        break;
                    case "przytyc":
                        DaneKalorii.dodatek = (DaneKalorii.plec == "M") ? 300 : 200;
                        break;
                    
                }

                btnObliczCel.IsEnabled = true;
            }
        }

        

        private void RadioButtonAktywnosc_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rbAktywnosc = sender as RadioButton;

            string stopienAktywnosci = rbAktywnosc.Tag.ToString();

            if (rbAktywnosc != null)
            {
                switch (stopienAktywnosci)
                {
                    case "znikoma":
                        DaneKalorii.wspolczynnik = 1.2;
                        break;
                    case "niska":
                        DaneKalorii.wspolczynnik = 1.375;
                        break;
                    case "umiarkowana":
                        DaneKalorii.wspolczynnik = 1.55;
                        break;
                    case "wysoka":
                        DaneKalorii.wspolczynnik = 1.725;
                        break;
                    case "bardzoWysoka":
                        DaneKalorii.wspolczynnik = 1.9;
                        break;
                }

                btnDalejAktywnosc.IsEnabled = true;
            }
        }

        private void tbWiek_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbWiek.Text = Regex.Replace(tbWiek.Text, "[^0-9]+", "");
        }

        private void tbWaga_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbWaga.Text = Regex.Replace(tbWaga.Text, "[^0-9]+", "");
        }

        private void tbWzrost_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbWzrost.Text = Regex.Replace(tbWzrost.Text, "[^0-9]+", "");
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

        private void btnDoKalendarzaDiety_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(kalendarzDiety), null, new DrillInNavigationTransitionInfo());
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
