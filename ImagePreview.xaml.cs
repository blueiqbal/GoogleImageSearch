using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using GoogleImageSearchViewModel;
using System.Windows.Media.Imaging;


namespace GoogleImageSearch
{
    public partial class Page1 : PhoneApplicationPage
    {
        // Reference to App
        private App app = App.Current as App;

        // Selected image url
        private string selectedUrl;

        // BitmapImage for loading Images 
        private BitmapImage bitmapImage;            

        public Page1()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Find selected image index from parameters
            IDictionary<string, string> parameters = this.NavigationContext.QueryString;
            if (parameters.ContainsKey("SelectedIndex"))
            {
                
                app.selectedImageIndex = Int32.Parse(parameters["SelectedIndex"]);
            }
            else
            {
                app.selectedImageIndex = 0;
            }
            // Find selected item url to load the image
            if (parameters.ContainsKey("url"))
            {
                selectedUrl = parameters["url"];
            }
            else
            {
                selectedUrl = "No url";
            }

             // Load image from Google
            LoadImage();
        }

        // Load Image from Google
        private void LoadImage()
        {
            // Load a new image
           bitmapImage = new BitmapImage(new Uri(selectedUrl, UriKind.RelativeOrAbsolute));
           bitmapImage.DownloadProgress += new EventHandler<DownloadProgressEventArgs>(bitmapImage_DownloadProgress);
           // Set the image to show in View2
           img.Source = bitmapImage;
        }

        // Navigate from this page
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
           bitmapImage.DownloadProgress -= new EventHandler<DownloadProgressEventArgs>(bitmapImage_DownloadProgress);
        }

        // Image is being loaded
        void bitmapImage_DownloadProgress(object sender, DownloadProgressEventArgs e)
        {
            // Hide loading... progressbar
            
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            progressBar1.Value = 39;
            bitmapImage.DownloadProgress -= new EventHandler<DownloadProgressEventArgs>(bitmapImage_DownloadProgress);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            // Back button to go back on main view
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));

        }

    }
}