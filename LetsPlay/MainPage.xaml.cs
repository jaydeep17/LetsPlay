using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using LetsPlay.Resources;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Tasks;
using System.Collections.ObjectModel;

namespace LetsPlay
{
    public partial class MainPage : PhoneApplicationPage
    {
        ObservableCollection<Clip> clips;
        private string[] fileNames;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            clips = new ObservableCollection<Clip>();
            //populateClips();
            // Sample code to localize the ApplicationBar
            
        }

        private void populateClips(){
            string decide = "";
            if (NavigationContext.QueryString.TryGetValue("pswd", out decide))
            {
                if (decide.Equals("true"))
                {
                    using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        var pattern = "\\Shared\\Media\\*";//Angel - Wonderful.mp4";
                        fileNames = store.GetFileNames(pattern);
                        foreach (var name in fileNames)
                        {
                            clips.Add(new Clip { Name = name });
                        }
                    }
                    myList.ItemsSource = clips;
                    BuildLocalizedApplicationBar();
                }
                else
                {
                    using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        var pattern = "\\Shared\\Media\\Angel - Wonderful.mp4";
                        fileNames = store.GetFileNames(pattern);
                        foreach (var name in fileNames)
                        {
                            clips.Add(new Clip { Name = name });
                        }
                    }
                    myList.ItemsSource = clips;
                }
            }
        }

        private void selectionChange(object sender, SelectionChangedEventArgs e)
        {
            int i = myList.SelectedIndex;
            if (i == -1)
                return;

            MediaPlayerLauncher mediaPlayerLauncher = new MediaPlayerLauncher();
            mediaPlayerLauncher.Media = new Uri("Shared/Media/" + fileNames[i], UriKind.Relative);
            mediaPlayerLauncher.Controls = MediaPlaybackControls.All;
            mediaPlayerLauncher.Location = MediaLocationType.Data;
            mediaPlayerLauncher.Show();

            myList.SelectedIndex = -1;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            NavigationService.RemoveBackEntry();
            if (myList.ItemsSource != null)
                return;
            populateClips();
        }

         //Sample code for building a localized ApplicationBar
        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();
            ApplicationBar.Mode = ApplicationBarMode.Minimized;

            // Create a new button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/settings.png", UriKind.Relative));
            appBarButton.Text = AppResources.AppBarSettings;
            appBarButton.Click += openSettingsPage;
            ApplicationBar.Buttons.Add(appBarButton);

            // Create a new menu item with the localized string from AppResources.
            //ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
            //ApplicationBar.MenuItems.Add(appBarMenuItem);
        }

        private void openSettingsPage(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
        }

        private void DeleteClip(object sender, RoutedEventArgs e)
        {
            ListBoxItem selectedListBoxItem = myList.ItemContainerGenerator.ContainerFromItem((sender as MenuItem).DataContext) as ListBoxItem;
            int index = myList.ItemContainerGenerator.IndexFromContainer(selectedListBoxItem);
            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                var pattern = "\\Shared\\Media\\" + fileNames[index];
                store.DeleteFile(pattern);
            }
            clips.RemoveAt(index);
            var temp = new List<string>(fileNames);
            temp.RemoveAt(index);
            fileNames = temp.ToArray();
        }

    }

    class Clip {
        public String Name { get; set; }
    }
}