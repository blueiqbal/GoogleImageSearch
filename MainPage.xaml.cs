using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using GoogleImageSearchViewModel;
using Microsoft.Phone.Controls;
using Newtonsoft.Json;
using System.Collections.Generic;


namespace GoogleImageSearch
{
    public partial class MainPage : PhoneApplicationPage
    {
        WebClient wc = new WebClient();
        public string uri { get; set; }
      //  List<ImageSearchItem> imageSearchItem = new List<ImageSearchItem>();

        private App app = App.Current as App;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(MainPage_Loaded);
        }
    
      private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
        }

      private void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
      {
          try
          {
              if (e.Error != null)
              {
                  MessageBox.Show("error");
                  return;
              }
              // ImageSearch item list is being clear here befoe filling it with new search data
              app.imageSearchItem.Clear();
              var json = e.Result; // Parse as JObject
              var rootObject = JsonConvert.DeserializeObject<RootObject>(json);

              // Parse json data to fetch thubmnail image, description and url for downloading the image
              foreach (var result in rootObject.responseData.results)
              {
                      string tbUrl = result.tbUrl;
                      string contentNoFormatting = result.contentNoFormatting;
                      string url = result.url;
                      app.imageSearchItem.Add(new ImageSearchItem(tbUrl, contentNoFormatting, url));
              }

              TransactionList.ItemsSource = null;
             // Listbox item is set with ImageSearchItem
              TransactionList.ItemsSource = app.imageSearchItem;

          }
          catch
          {
          }

      }

      /// <summary>
      /// Decide what to do now that the selection has changed on MainMenuList
      /// </summary>
      /// <param name="sender">MainMenuList passed as an object</param>
      /// <param name="e">The selection change data</param>
      private void TransactionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
          ListBox listBox = sender as ListBox;
          if (listBox == null || listBox.SelectedIndex < 0)
          {
              return;
          }

          // If selected index is -1 (no selection) do nothing
          if (TransactionList.SelectedIndex == -1)
              return;

          app.selectedImageIndex = TransactionList.SelectedIndex;

          // Navigate to the new page
          NavigationService.Navigate(new Uri("/ImagePreview.xaml?selectedItem=" + TransactionList.SelectedIndex + "&url=" + app.imageSearchItem[TransactionList.SelectedIndex].url, UriKind.Relative));

          TransactionList.SelectedIndex = -1;
      }

      protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
      {
      }
      protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
      {
          base.OnNavigatedTo(e);
          // Show previous searched items after back button is pressed from View2
          TransactionList.ItemsSource = app.imageSearchItem;
          TransactionList.SelectedIndex = -1;
      }

      // Start Search button event handler
      private void btnSearch_Click(object sender, RoutedEventArgs e)
      {
          // Request for image search
          wc.DownloadStringAsync(new Uri("https://ajax.googleapis.com/ajax/services/search/images?rsz=8&"
            + "start=" + 0 + "&v=1.0&q=" + uri));
          wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted);
      }

      private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
      {
        // Search box keyword
         uri = SearchBox.Text;
      }
    }

}