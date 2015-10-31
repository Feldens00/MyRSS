using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MyRss.Resources;
using System.Xml.Linq;
using MyRss.Class;

namespace MyRss
{
    public partial class MainPage : PhoneApplicationPage
    {
        RssItem select = new RssItem();
        // Constructor
        public MainPage()
        {
            InitializeComponent();

         
           
            
        }

        private void RssClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            var rssData = from rss in XElement.Parse(e.Result).Descendants("item")
                          select new RssItem
                          {
                              title = rss.Element("title").Value,
                              pubDate = rss.Element("pubDate").Value,
                              description = rss.Element("description").Value,
                              link = rss.Element("link").Value
                            };
            lstRss.ItemsSource = rssData;
           
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            WebClient rssClient = new WebClient();

            rssClient.DownloadStringCompleted += RssClient_DownloadStringCompleted;
            rssClient.Encoding = System.Text.Encoding.GetEncoding("ISO8859-1");
            rssClient.DownloadStringAsync(new Uri(@" http://g1.globo.com/dynamo/tecnologia/rss2.xml"));


            

        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
             Browser browser = e.Content as Browser;
            if (browser!=null)
            {
                browser.pageBrowser = select;
            }  
           
        }

        private void onSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstRss!=null)
            {
                select = (sender as ListBox).SelectedItem as RssItem;
                NavigationService.Navigate(new Uri("/Browser.xaml", UriKind.Relative));
               
            }


        }
    }
}