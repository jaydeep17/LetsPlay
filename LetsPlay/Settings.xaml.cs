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

namespace LetsPlay
{
    public partial class Settings : PhoneApplicationPage
    {
        private ApplicationBarIconButton done;
        private IsolatedStorageSettings appSettings;
        private string oldPass, fakeOldPass;
        public Settings()
        {
            InitializeComponent();
            BuildLocalizedApplicationBar();
            appSettings = IsolatedStorageSettings.ApplicationSettings;
            oldPass = (string)appSettings["pass"];
            fakeOldPass = (string)appSettings["fake"];
        }

        

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            oldPassBox.Focus();
        }

        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            done = new ApplicationBarIconButton(new Uri("/Assets/check.png", UriKind.Relative));
            done.Text = AppResources.AppBarDone;
            done.Click += done_Click;
            done.IsEnabled = false;

            ApplicationBarIconButton cancel = new ApplicationBarIconButton(new Uri("/Assets/cancel.png", UriKind.Relative));
            cancel.Text = AppResources.AppBarCancel;
            cancel.Click += cancel_Click;

            ApplicationBar.Buttons.Add(done);
            ApplicationBar.Buttons.Add(cancel);
        }

        private void done_Click(object sender, EventArgs e)
        {
            if (panorama.SelectedIndex == 0)
            {
                appSettings["pass"] = newPassBox.Password;
            }
            else {
                appSettings["fake"] = fakeNewPassBox.Password;
            }
            NavigationService.GoBack();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void PanSwitch(object sender, SelectionChangedEventArgs e)
        {
            oldPassBox.Password = "";
            fakeOldPassBox.Password = "";
            newPassBox.Password = "";
            fakeNewPassBox.Password = "";

            oldPassTxt.Text = "";
            fakeOldPassTxt.Text = "";
            newPassTxt.Text = "";
            fakeNewPassTxt.Text = "";

            newPassBox.IsEnabled = false;
            fakeNewPassBox.IsEnabled = false;

            showNewPass.IsEnabled = false;
            showNewFake.IsEnabled = false;

            done.IsEnabled = false;
            int i = panorama.SelectedIndex;
            
        }

        private void checkOldPass(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (oldPassBox.Password == oldPass) {
                newPassBox.IsEnabled = true;
                newPassBox.Focus();
                showNewPass.IsEnabled = true;
            }
        }

        private void checkFakeOldPass(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (fakeOldPassBox.Password == fakeOldPass) {
                fakeNewPassBox.IsEnabled = true;
                fakeNewPassBox.Focus();
                showNewFake.IsEnabled = true;
            }
        }

        private void checkNewPass(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (newPassBox.Password.Length > 0)
                done.IsEnabled = true;
            else
                done.IsEnabled = false;
        }

        private void checkFakeNewPass(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (fakeNewPassBox.Password.Length > 0)
                done.IsEnabled = true;
            else
                done.IsEnabled = false;
        }



        /* All usability methods */

        private void OldPassCheckBoxChange(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            if (showOldPass.IsChecked == false)
            {
                oldPassBox.Opacity = 0;
                oldPassTxt.Opacity = 100;
                oldPassTxt.Text = oldPassBox.Password;
                oldPassTxt.Select(oldPassTxt.Text.Length, 0);
            }
            else {
                oldPassBox.Opacity = 100;
                oldPassTxt.Opacity = 0;
                oldPassBox.Password = oldPassTxt.Text;
                oldPassBox.SelectAll();
            }
        }

        private void NewPassCheckBoxChange(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            if (showNewPass.IsChecked == false)
            {
                newPassBox.Opacity = 0;
                newPassTxt.Opacity = 100;
                newPassTxt.Text = newPassBox.Password;
                newPassTxt.Select(newPassTxt.Text.Length, 0);
            }
            else
            {
                newPassBox.Opacity = 100;
                newPassTxt.Opacity = 0;
                newPassBox.Password = newPassTxt.Text;
                newPassBox.SelectAll();
            }
        }

        private void FakeOldPassCheckBoxChange(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            if (showOldFake.IsChecked == false)
            {
                fakeOldPassBox.Opacity = 0;
                fakeOldPassTxt.Opacity = 100;
                fakeOldPassTxt.Text = fakeOldPassBox.Password;
                fakeOldPassTxt.Select(fakeOldPassTxt.Text.Length, 0);
            }
            else {
                fakeOldPassBox.Opacity = 100;
                fakeOldPassTxt.Opacity = 0;
                fakeOldPassBox.Password = fakeOldPassTxt.Text;
                fakeOldPassBox.SelectAll();
            }
        }

        private void FakeNewPassCheckBoxChange(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            if (showNewFake.IsChecked == false)
            {
                fakeNewPassBox.Opacity = 0;
                fakeNewPassTxt.Opacity = 100;
                fakeNewPassTxt.Text = fakeNewPassBox.Password;
                fakeNewPassTxt.Select(fakeNewPassTxt.Text.Length, 0);
            }
            else
            {
                fakeNewPassBox.Opacity = 100;
                fakeNewPassTxt.Opacity = 0;
                fakeNewPassBox.Password = fakeNewPassTxt.Text;
                fakeNewPassBox.SelectAll();
            }
        }

        private void OldPassTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (showOldPass.IsChecked == true)
                oldPassTxt.Focus();
            else
                oldPassBox.Focus();
        }

        private void NewPassTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (newPassBox.IsEnabled == false)
                return;
            if (showNewPass.IsChecked == true)
                newPassTxt.Focus();
            else
                newPassBox.Focus();
        }

        private void FakeOldTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (showOldFake.IsChecked == true)
                fakeOldPassTxt.Focus();
            else
                fakeOldPassBox.Focus();
        }

        private void FakeNewTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (fakeNewPassBox.IsEnabled == false)
                return;
            if (showNewFake.IsChecked == true)
                fakeNewPassTxt.Focus();
            else
                fakeNewPassBox.Focus();
        }

        private void checkOldPassTxt(object sender, TextChangedEventArgs e)
        {
            if (oldPassTxt.Text == oldPass)
            {
                newPassBox.IsEnabled = true;
                newPassBox.Focus();
                showNewPass.IsEnabled = true;
            }
        }

        private void checkFakeOldPassTxt(object sender, TextChangedEventArgs e)
        {
            if (fakeOldPassTxt.Text == fakeOldPass)
            {
                fakeNewPassBox.IsEnabled = true;
                fakeNewPassBox.Focus();
                showNewFake.IsEnabled = true;
            }
        }

        private void checkNewPassTxt(object sender, TextChangedEventArgs e)
        {
            if (newPassTxt.Text.Length > 0)
                done.IsEnabled = true;
            else
                done.IsEnabled = false;
        }

        private void checkFakeNewPassTxt(object sender, TextChangedEventArgs e)
        {
            if (fakeNewPassTxt.Text.Length > 0)
                done.IsEnabled = true;
            else
                done.IsEnabled = false;
        }
    }
}