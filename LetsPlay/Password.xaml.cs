using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;

namespace LetsPlay
{
    public partial class Password : PhoneApplicationPage
    {
        private IsolatedStorageSettings appSettings;
        private string pass, fakePass;
        public Password()
        {
            InitializeComponent();
            appSettings = IsolatedStorageSettings.ApplicationSettings;
            if (!appSettings.Contains("pass")) {
                appSettings.Add("pass", "blue");
            }
            if (!appSettings.Contains("fake")) {
                appSettings.Add("fake", "red");
            }
            pass = (string)appSettings["pass"];
            fakePass = (string)appSettings["fake"];
        }

        private void checkPass(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // true password
            if (pswdBox.Password == pass) {
                NavigationService.Navigate(new Uri("/MainPage.xaml?pswd=true", UriKind.Relative));
            }
            
            // trick password
            if (pswdBox.Password == fakePass) {
                NavigationService.Navigate(new Uri("/MainPage.xaml?pswd=false", UriKind.Relative));
            }
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            pswdBox.Focus();
        }

        

        private void CheckboxStateChange(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            // this is inverted coz after this method finishes the ui element is updated
            if (showPass.IsChecked == false)
            {
                pswdBox.Opacity = 0;
                txtBox.Opacity = 100;
                txtBox.Text = pswdBox.Password;
                txtBox.Select(txtBox.Text.Length, 0);
            }
            else {
                pswdBox.Opacity = 100;
                txtBox.Opacity = 0;
                pswdBox.Password = txtBox.Text;
                pswdBox.SelectAll();
            }
        }

        private void passTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (showPass.IsChecked == true)
            {
                txtBox.Focus();
            }
            else {
                pswdBox.Focus();
            }
        }
        
    }
}