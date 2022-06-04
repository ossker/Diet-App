using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Input;
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


namespace dietApp
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

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

        private void btnDoPogody_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(pogoda), null, new DrillInNavigationTransitionInfo());
        }
    }

  
}
