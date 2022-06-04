using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
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

    public sealed partial class pomocLiczenie : Page
    {
        public pomocLiczenie()
        {
            this.InitializeComponent();
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



        private void btnWroc_Click(object sender, RoutedEventArgs e)
        {
            
            Frame.GoBack(new DrillInNavigationTransitionInfo());
        }

        private void btnWyliczWroc_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(tbGramyEtykieta.Text)
                && !String.IsNullOrEmpty(tbGramyMojPosilek.Text) 
                && !String.IsNullOrEmpty(tbKaloriePomoc.Text) 
                && !String.IsNullOrEmpty(tbBialkaPomoc.Text)
                && !String.IsNullOrEmpty(tbTluszczePomoc.Text)
                && !String.IsNullOrEmpty(tbWeglowodanyPomoc.Text))
            {
                int gramyEtykieta = int.Parse(tbGramyEtykieta.Text);
                int gramyMojegoPosilku = int.Parse(tbGramyMojPosilek.Text);
                int kaloriePomoc = int.Parse(tbKaloriePomoc.Text);
                int bialkaPomoc = int.Parse(tbBialkaPomoc.Text);
                int tluszczePomoc = int.Parse(tbTluszczePomoc.Text);
                int weglowodanyPomoc = int.Parse(tbWeglowodanyPomoc.Text);

                int mnoznik = gramyMojegoPosilku / gramyEtykieta;

                kaloriePomoc *= mnoznik;
                bialkaPomoc *= mnoznik;
                tluszczePomoc *= mnoznik;
                weglowodanyPomoc *= mnoznik;

                List<int> tablica = new List<int>();
                tablica.Add(kaloriePomoc);
                tablica.Add(bialkaPomoc);
                tablica.Add(tluszczePomoc);
                tablica.Add(weglowodanyPomoc);

                this.Frame.Navigate(typeof(dziennikZywnosci), tablica, new DrillInNavigationTransitionInfo());
            }
            else
            {
                if (String.IsNullOrEmpty(tbGramyEtykieta.Text))
                    tbGramyEtykieta.Background = new SolidColorBrush(Colors.Pink);
                else
                    tbGramyEtykieta.Background = new SolidColorBrush(Colors.Gainsboro);
                if (String.IsNullOrEmpty(tbGramyMojPosilek.Text))
                    tbGramyMojPosilek.Background = new SolidColorBrush(Colors.Pink);
                else
                    tbGramyMojPosilek.Background = new SolidColorBrush(Colors.Gainsboro);
                if (String.IsNullOrEmpty(tbKaloriePomoc.Text))
                    tbKaloriePomoc.Background = new SolidColorBrush(Colors.Pink);
                else
                    tbKaloriePomoc.Background = new SolidColorBrush(Colors.Gainsboro);
                if (String.IsNullOrEmpty(tbBialkaPomoc.Text))
                    tbBialkaPomoc.Background = new SolidColorBrush(Colors.Pink);
                else
                    tbBialkaPomoc.Background = new SolidColorBrush(Colors.Gainsboro);
                if (String.IsNullOrEmpty(tbTluszczePomoc.Text))
                    tbTluszczePomoc.Background = new SolidColorBrush(Colors.Pink);
                else
                    tbTluszczePomoc.Background = new SolidColorBrush(Colors.Gainsboro);
                if (String.IsNullOrEmpty(tbWeglowodanyPomoc.Text))
                    tbWeglowodanyPomoc.Background = new SolidColorBrush(Colors.Pink);
                else
                    tbWeglowodanyPomoc.Background = new SolidColorBrush(Colors.Gainsboro);
            }

            
        }

        private void tbGramyEtykieta_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbGramyEtykieta.Text = Regex.Replace(tbGramyEtykieta.Text, "[^0-9]+", "");
        }

        private void tbKaloriePomoc_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbKaloriePomoc.Text = Regex.Replace(tbKaloriePomoc.Text, "[^0-9]+", "");
        }

        private void tbBialkaPomoc_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbBialkaPomoc.Text = Regex.Replace(tbBialkaPomoc.Text, "[^0-9]+", "");
        }

        private void tbTluszczePomoc_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbTluszczePomoc.Text = Regex.Replace(tbTluszczePomoc.Text, "[^0-9]+", "");
        }

        private void tbWeglowodanyPomoc_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbWeglowodanyPomoc.Text = Regex.Replace(tbWeglowodanyPomoc.Text, "[^0-9]+", "");
        }

        private void tbGramyMojPosilek_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbGramyMojPosilek.Text = Regex.Replace(tbGramyMojPosilek.Text, "[^0-9]+", "");
        }
    }
}
