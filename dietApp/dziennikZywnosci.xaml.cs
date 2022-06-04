using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;


namespace dietApp
{
    
    public sealed partial class dziennikZywnosci : Page
    {
        public dziennikZywnosci()
        {
            this.InitializeComponent();
            PosilkiDataLib.InitializeDB();

            

            dataGridSniadanie.ItemsSource = PosilkiDataLib.GetRecordsByTypAndData("S", DateTime.Now.Date.ToString());
            dataGridObiad.ItemsSource = PosilkiDataLib.GetRecordsByTypAndData("O", DateTime.Now.Date.ToString());
            dataGridKolacja.ItemsSource = PosilkiDataLib.GetRecordsByTypAndData("K", DateTime.Now.Date.ToString());
            dataGridInne.ItemsSource = PosilkiDataLib.GetRecordsByTypAndData("I", DateTime.Now.Date.ToString());

            tbSniadanieKalorie.Text = PosilkiDataLib.GetKalorieByTypAndData("S", DateTime.Now.Date.ToString()).ToString();
            tbObiadKalorie.Text = PosilkiDataLib.GetKalorieByTypAndData("O", DateTime.Now.Date.ToString()).ToString();
            tbKolacjaKalorie.Text = PosilkiDataLib.GetKalorieByTypAndData("K", DateTime.Now.Date.ToString()).ToString();
            tbInneKalorie.Text = PosilkiDataLib.GetKalorieByTypAndData("I", DateTime.Now.Date.ToString()).ToString();

            
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("CPM")
                && ApplicationData.Current.LocalSettings.Values.ContainsKey("bialka")
                && ApplicationData.Current.LocalSettings.Values.ContainsKey("tluszcze")
                && ApplicationData.Current.LocalSettings.Values.ContainsKey("weglowodany"))
            {

                DaneKalorii.CPM = (double)ApplicationData.Current.LocalSettings.Values["CPM"];
                DaneKalorii.bialka = (double)ApplicationData.Current.LocalSettings.Values["bialka"];
                DaneKalorii.tluszcze = (double)ApplicationData.Current.LocalSettings.Values["tluszcze"];
                DaneKalorii.weglowodany = (double)ApplicationData.Current.LocalSettings.Values["weglowodany"];

                try
                {
                    double potrzebneKalorie = DaneKalorii.CPM - PosilkiDataLib.GetKalorieByData(DateTime.Now.Date.ToString())[0];
                    double potrzebneBialka = DaneKalorii.bialka - PosilkiDataLib.GetKalorieByData(DateTime.Now.Date.ToString())[1];
                    double potrzebneTluszcze = DaneKalorii.tluszcze - PosilkiDataLib.GetKalorieByData(DateTime.Now.Date.ToString())[2];
                    double potrzebneWeglowodany = DaneKalorii.weglowodany - PosilkiDataLib.GetKalorieByData(DateTime.Now.Date.ToString())[3];

                    ProgressBar.Value = 100 - Math.Round((potrzebneKalorie / DaneKalorii.CPM) * 100);
                    

                    if (potrzebneKalorie > 0)
                        tbPotrzebneKalorie.Text = (potrzebneKalorie).ToString() + " kcal";
                    else
                    {
                        borderPotrzebneKalorie.Background = new SolidColorBrush(Colors.Pink);
                        tbPotrzebneKalorie.Text = "Przekroczone!";
                    }
                        

                    if (potrzebneBialka > 0)
                        tbPotrzebneBialka.Text = (potrzebneBialka).ToString() + " g";
                    else
                        tbPotrzebneBialka.Text = "0";
                    if (potrzebneTluszcze > 0)
                        tbPotrzebneTluszcze.Text = (potrzebneTluszcze).ToString() + " g";
                    else
                        tbPotrzebneTluszcze.Text = "0";
                    if (potrzebneWeglowodany > 0)
                        tbPotrzebneWeglowodany.Text = (potrzebneWeglowodany).ToString() + " g";
                    else
                        tbPotrzebneWeglowodany.Text = "0";
                }
                catch
                {
                    tbPotrzebneKalorie.Text = DaneKalorii.CPM.ToString() + " kcal";
                    tbPotrzebneBialka.Text = DaneKalorii.bialka.ToString() + " g";
                    tbPotrzebneTluszcze.Text = DaneKalorii.tluszcze.ToString() + " g";
                    tbPotrzebneWeglowodany.Text = DaneKalorii.weglowodany.ToString() + " g";
                }
                
            }
            else
            {
                tbPotrzebneKalorie.Text = "Brak danych";
                ProgressBar.Value = 0;
            }

        }


        private async void btnDodajPosilek_Click(object sender, RoutedEventArgs e)
        {

            if (cbTypPosilku.SelectedItem != null && !String.IsNullOrEmpty(tbNazwa.Text)
                && !String.IsNullOrEmpty(tbKalorie.Text) && !String.IsNullOrEmpty(tbBialka.Text)
                && !String.IsNullOrEmpty(tbTluszcze.Text) && !String.IsNullOrEmpty(tbWeglowodany.Text))
            {
                string nazwa = tbNazwa.Text;
                int kalorie = Int32.Parse(tbKalorie.Text);
                int bialka = Int32.Parse(tbBialka.Text);
                int tluszcze = Int32.Parse(tbTluszcze.Text);
                int weglowodany = Int32.Parse(tbWeglowodany.Text);
                string typ = (cbTypPosilku.SelectedItem as ComboBoxItem).Tag.ToString();


                ContentDialog confirmAddRecord = new ContentDialog()
                {
                    Title = "Czy na pewno chcesz dodać posiłek?",
                    Content = "Nazwa: " + nazwa + "\nKalorie: " + kalorie,

                    PrimaryButtonText = "Nie",
                    SecondaryButtonText = "Tak",
                    DefaultButton = ContentDialogButton.Secondary
                };



                try
                {
                    var result = await confirmAddRecord.ShowAsync();
                    if (result == ContentDialogResult.Secondary)
                    {
                        PosilkiDataLib.addRecord(nazwa, kalorie, bialka, tluszcze, weglowodany, typ, DateTime.Now.Date.ToString());
                        this.Frame.Navigate(typeof(dziennikZywnosci));

                    }
                }
                catch (Exception ex) 
                { 
                    Console.WriteLine(ex.Message);
                }
                
            }
            else
            {
                if (String.IsNullOrEmpty(tbNazwa.Text))
                    tbNazwa.Background = new SolidColorBrush(Colors.Pink);
                else
                    tbNazwa.Background = new SolidColorBrush(Colors.Gainsboro);
                if (String.IsNullOrEmpty(tbKalorie.Text))
                    tbKalorie.Background = new SolidColorBrush(Colors.Pink);
                else
                    tbKalorie.Background = new SolidColorBrush(Colors.Gainsboro);
                if (String.IsNullOrEmpty(tbBialka.Text))
                    tbBialka.Background = new SolidColorBrush(Colors.Pink);
                else
                    tbBialka.Background = new SolidColorBrush(Colors.Gainsboro);
                if (String.IsNullOrEmpty(tbTluszcze.Text))
                    tbTluszcze.Background = new SolidColorBrush(Colors.Pink);
                else
                    tbTluszcze.Background = new SolidColorBrush(Colors.Gainsboro);
                if (String.IsNullOrEmpty(tbWeglowodany.Text))
                    tbWeglowodany.Background = new SolidColorBrush(Colors.Pink);
                else
                    tbWeglowodany.Background = new SolidColorBrush(Colors.Gainsboro);
                if (cbTypPosilku.SelectedItem == null)
                    cbTypPosilku.Background = new SolidColorBrush(Colors.Pink);
                else
                    cbTypPosilku.Background = new SolidColorBrush(Colors.Gainsboro);
            }

           

        }

        private void btnDoDomu_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage), null, new DrillInNavigationTransitionInfo());
        }

        private void btnDoKalkulatoraKalorii_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(kalkulatorKalorii), null, new DrillInNavigationTransitionInfo());
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

        int kaloriePomoc, bialkaPomoc, tluszczePomoc, weglowodanyPomoc;


        private void tbBialka_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbBialka.Text = Regex.Replace(tbBialka.Text, "[^0-9]+", "");
        }

        private void tbTluszcze_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbTluszcze.Text = Regex.Replace(tbTluszcze.Text, "[^0-9]+", "");
        }

        private void tbWeglowodany_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbWeglowodany.Text = Regex.Replace(tbWeglowodany.Text, "[^0-9]+", "");
        }

        

        private void tbKalorie_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbKalorie.Text = Regex.Replace(tbKalorie.Text, "[^0-9]+", "");
        }

        private void btnPomoc_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(pomocLiczenie), null, new DrillInNavigationTransitionInfo());
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter != null)
            {
                List<int> tab = (List<int>)e.Parameter;
                kaloriePomoc = tab[0];
                bialkaPomoc = tab[1];
                tluszczePomoc = tab[2];
                weglowodanyPomoc = tab[3];

                tbKalorie.Text = kaloriePomoc.ToString();
                tbBialka.Text = bialkaPomoc.ToString();
                tbTluszcze.Text = tluszczePomoc.ToString();
                tbWeglowodany.Text = weglowodanyPomoc.ToString();
            }    
        }
    }
}
