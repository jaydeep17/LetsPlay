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

namespace LetsPlay
{
    public partial class MainPage : PhoneApplicationPage
    {
        List<Clip> clips;
        private string[] fileNames;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            clips = new List<Clip>();
            //populateClips();
            // Sample code to localize the ApplicationBar
            
        }

        private void populateClips(){
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
            string decide = "";
            if (NavigationContext.QueryString.TryGetValue("pswd", out decide))
            {
                if (decide.Equals("true"))
                {
                    populateClips();
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

         //Sample code for building a localized ApplicationBar
        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

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
    }

    class Clip {
        public String Name { get; set; }
    }
}