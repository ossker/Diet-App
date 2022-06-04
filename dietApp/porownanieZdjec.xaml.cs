using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Media.Capture;
using Windows.Devices.Enumeration;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.Media.MediaProperties;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media.Animation;

namespace dietApp
{
    public sealed partial class porownanieZdjec : Page
    {
        public porownanieZdjec()
        {
            this.InitializeComponent();
            ustawZdjeciePrzed();
            ustawZdjeciePo();
        }

        MediaCapture kamera;

        // -------------------------------------------------------------
        //PRZED
        // -------------------------------------------------------------

        bool isPreviewingPrzed = false;

        private async void odpalAparatPrzed()
        {
            if(!isPreviewingPrzed)
            {
                try
                {
                    cameraPreviewPrzed.Visibility = Visibility.Visible;
                    imagePrzed.Visibility = Visibility.Collapsed;
                    kamera = new MediaCapture();

                    var camDev = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);

                    var id = camDev.FirstOrDefault().Id;

                    await kamera.InitializeAsync(new MediaCaptureInitializationSettings { VideoDeviceId = id });

                    cameraPreviewPrzed.Source = kamera;
                    await kamera.StartPreviewAsync();
                    isPreviewingPrzed = true;
                    btnKameraPrzed.Content = "Wyłącz";

                    btnKameraPo.IsEnabled = false;
                    btnWybierzPlikPo.IsEnabled = false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                

            }
            else
            {
                try
                {
                    cameraPreviewPrzed.Visibility = Visibility.Collapsed;
                    imagePrzed.Visibility = Visibility.Visible;
                    await kamera.StopPreviewAsync();
                    isPreviewingPrzed = false;
                    btnKameraPrzed.Content = "Aparat";
                    btnWybierzPlikPrzed.Visibility = Visibility.Visible;
                    btnZrobZdjeciePrzed.Visibility = Visibility.Collapsed;
                    btnKameraPo.IsEnabled = true;
                    btnWybierzPlikPo.IsEnabled = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        

        private async void zrobZdjeciePrzed()
        {
            if (isPreviewingPrzed)
            {
                try
                {
                    var storageFolder = KnownFolders.SavedPictures;

                    var file = await storageFolder.CreateFileAsync("bodyPrzed.jpg", CreationCollisionOption.ReplaceExisting);

                    await kamera.CapturePhotoToStorageFileAsync(ImageEncodingProperties.CreateJpeg(), file);

                    using (IRandomAccessStream fStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                    {
                        var bmpImage = new BitmapImage();
                        await bmpImage.SetSourceAsync(fStream);
                        imagePrzed.Source = bmpImage;
                    }

                    cameraPreviewPrzed.Visibility = Visibility.Collapsed;
                    imagePrzed.Visibility = Visibility.Visible;
                    await kamera.StopPreviewAsync();
                    isPreviewingPrzed = false;
                    btnKameraPrzed.Content = "Aparat";
                    btnWybierzPlikPrzed.Visibility = Visibility.Visible;
                    btnZrobZdjeciePrzed.Visibility = Visibility.Collapsed;

                    btnKameraPo.IsEnabled = true;
                    btnWybierzPlikPo.IsEnabled = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
        }

        private async void ustawZdjeciePrzed()
        {
            imagePrzed.Visibility = Visibility.Visible;
            cameraPreviewPrzed.Visibility = Visibility.Collapsed;
            try
            {
                StorageFile file = await KnownFolders.SavedPictures.GetFileAsync("bodyPrzed.jpg");

                using (IRandomAccessStream fStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                {
                    var bmpImage = new BitmapImage();
                    await bmpImage.SetSourceAsync(fStream);
                    imagePrzed.Source = bmpImage;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void btnKameraPrzed_Click(object sender, RoutedEventArgs e)
        {
            
            odpalAparatPrzed();
            btnWybierzPlikPrzed.Visibility = Visibility.Collapsed;
            btnZrobZdjeciePrzed.Visibility = Visibility.Visible;

        }

        private void btnZrobZdjeciePrzed_Click(object sender, RoutedEventArgs e)
        {
            zrobZdjeciePrzed();
        }

        private async void btnWybierzPlikPrzed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var onPick = new FileOpenPicker()
                {
                    SuggestedStartLocation = PickerLocationId.PicturesLibrary,
                    ViewMode = PickerViewMode.List,
                    FileTypeFilter = { ".jpg", ".jpeg" }
                };

                var file = await onPick.PickSingleFileAsync();
                if (file == null)
                    return;

                await file.CopyAsync(KnownFolders.SavedPictures, "bodyPrzed.jpg", NameCollisionOption.ReplaceExisting);
                using (IRandomAccessStream fStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                {
                    var bmpImage = new BitmapImage();
                    await bmpImage.SetSourceAsync(fStream);
                    imagePrzed.Source = bmpImage;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        // -------------------------------------------------------------
        // PO
        // -------------------------------------------------------------

        bool isPreviewingPo = false;

        private async void odpalAparatPo()
        {
            if (!isPreviewingPo)
            {
                try
                {
                    cameraPreviewPo.Visibility = Visibility.Visible;
                    imagePo.Visibility = Visibility.Collapsed;
                    kamera = new MediaCapture();

                    var camDev = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);

                    var id = camDev.FirstOrDefault().Id;

                    await kamera.InitializeAsync(new MediaCaptureInitializationSettings { VideoDeviceId = id });

                    cameraPreviewPo.Source = kamera;

                    await kamera.StartPreviewAsync();

                    isPreviewingPo = true;
                    btnKameraPo.Content = "Wyłącz";
                    btnKameraPrzed.IsEnabled = false;
                    btnWybierzPlikPrzed.IsEnabled = false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                try
                {
                    cameraPreviewPo.Visibility = Visibility.Collapsed;
                    imagePo.Visibility = Visibility.Visible;
                    await kamera.StopPreviewAsync();
                    isPreviewingPo = false;
                    btnKameraPo.Content = "Aparat";
                    btnWybierzPlikPo.Visibility = Visibility.Visible;
                    btnZrobZdjeciePo.Visibility = Visibility.Collapsed;
                    btnKameraPrzed.IsEnabled = true;
                    btnWybierzPlikPrzed.IsEnabled = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }


        private async void zrobZdjeciePo()
        {
            if (isPreviewingPo)
            {
                try
                {
                    var storageFolder = KnownFolders.SavedPictures;

                    var file = await storageFolder.CreateFileAsync("bodyPo.jpg", CreationCollisionOption.ReplaceExisting);

                    await kamera.CapturePhotoToStorageFileAsync(ImageEncodingProperties.CreateJpeg(), file);

                    using (IRandomAccessStream fStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                    {
                        var bmpImage = new BitmapImage();
                        await bmpImage.SetSourceAsync(fStream);
                        imagePo.Source = bmpImage;
                    }

                    cameraPreviewPo.Visibility = Visibility.Collapsed;
                    imagePo.Visibility = Visibility.Visible;
                    await kamera.StopPreviewAsync();
                    isPreviewingPo = false;
                    btnKameraPo.Content = "Aparat";
                    btnWybierzPlikPo.Visibility = Visibility.Visible;
                    btnZrobZdjeciePo.Visibility = Visibility.Collapsed;

                    btnKameraPrzed.IsEnabled = true;
                    btnWybierzPlikPrzed.IsEnabled = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private async void ustawZdjeciePo()
        {
            imagePo.Visibility = Visibility.Visible;
            cameraPreviewPo.Visibility = Visibility.Collapsed;
            try
            {
                StorageFile file = await KnownFolders.SavedPictures.GetFileAsync("bodyPo.jpg");

                using (IRandomAccessStream fStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                {
                    var bmpImage = new BitmapImage();
                    await bmpImage.SetSourceAsync(fStream);
                    imagePo.Source = bmpImage;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

       

        private void btnKameraPo_Click(object sender, RoutedEventArgs e)
        {
            odpalAparatPo();
            btnWybierzPlikPo.Visibility = Visibility.Collapsed;
            btnZrobZdjeciePo.Visibility = Visibility.Visible;
        }

        private void btnZrobZdjeciePo_Click(object sender, RoutedEventArgs e)
        {
            zrobZdjeciePo();
        }

        private async void btnWybierzPlikPo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var onPick = new FileOpenPicker()
                {
                    SuggestedStartLocation = PickerLocationId.PicturesLibrary,
                    ViewMode = PickerViewMode.List,
                    FileTypeFilter = { ".jpg", ".jpeg" }
                };

                var file = await onPick.PickSingleFileAsync();
                if (file == null)
                    return;

                await file.CopyAsync(KnownFolders.SavedPictures, "bodyPo.jpg", NameCollisionOption.ReplaceExisting);

                using (IRandomAccessStream fStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                {
                    var bmpImage = new BitmapImage();
                    await bmpImage.SetSourceAsync(fStream);
                    imagePo.Source = bmpImage;
                }
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
