using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Coding4Fun.Toolkit.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace PsychicTest.View
{
    public partial class Settings : PhoneApplicationPage
    {
        private bool _isFirst;

        public Settings()
        {
            InitializeComponent();

            Loaded += Settings_Loaded;

            while (App.RootFrame.RemoveBackEntry() != null)
            {

            }

            _isFirst = true;
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            base.OnBackKeyPress(e);

            var selected = LspNumberGuesses.SelectedItem as GuessesItem;
            if (selected != null)
            {
                if (SoundSwitch.IsChecked != null && (bool) SoundSwitch.IsChecked)
                    IsolatedStorageSettings.ApplicationSettings["EnableSound"] = 1;
                else IsolatedStorageSettings.ApplicationSettings["EnableSound"] = 0;

                var value = selected.Name;
                IsolatedStorageSettings.ApplicationSettings["CountPredict"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();

                Debug.WriteLine(value);

                var toast = new ToastPrompt { Message = "setting saved" };
                toast.Show();
            }
        }

        private void Settings_Loaded(object sender, RoutedEventArgs e)
        {
            var source = new List<GuessesItem>
            {
                new GuessesItem() {Name = "10"},
                new GuessesItem() {Name = "15"},
                new GuessesItem() {Name = "20"},
                new GuessesItem() {Name = "25"},
                new GuessesItem() {Name = "30"}
            };

            LspNumberGuesses.ItemsSource = source;

            var limitVal = IsolatedStorageSettings.ApplicationSettings["CountPredict"].ToString();
            var enableSound = IsolatedStorageSettings.ApplicationSettings["EnableSound"].ToString();

            SoundSwitch.IsChecked = enableSound.Equals("1");
            LspNumberGuesses.SelectedItem = LspNumberGuesses.Items.OfType<GuessesItem>().First(i => i.Name == limitVal);
        }

        private void LspNumberGuesses_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (_isFirst)
                    _isFirst = false;
                else
                {
                    var listPicker = sender as ListPicker;
                    if (listPicker != null)
                    {
                        var selected = listPicker.SelectedItem as GuessesItem;
                        if (selected != null)
                        {
                            var value = selected.Name;

                            Debug.WriteLine(value);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private void SendMailClick(object sender, RoutedEventArgs e)
        {
            var task = new EmailComposeTask { To = "daniel@9malls.com", Cc = "ziacstudio@gmail.com" };
            task.Show(); 
        }

        private void RateAppClick(object sender, RoutedEventArgs e)
        {
            MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();
            marketplaceReviewTask.Show();
        }

        private void ShareClick(object sender, RoutedEventArgs e)
        {
            //Show an application, using the default ContentType.
            ShareLinkTask share = new ShareLinkTask();
            share.LinkUri = new Uri("https://www.windowsphone.com/en-us/store/app/psychic-testing/c2dc457a-6b01-46fb-a6a3-c4829129ef69", UriKind.RelativeOrAbsolute);
            share.Title = "Are you psychic? Play Psychic Testing on Windows Phone";
            share.Message = "Psychic Testing";
            share.Show();

            //MarketplaceDetailTask marketplaceDetailTask = new MarketplaceDetailTask();

            //marketplaceDetailTask.ContentIdentifier = "fa0ea25c-f43d-4455-9243-48ec40c42abf";
            //marketplaceDetailTask.ContentType = MarketplaceContentType.Applications;

            //marketplaceDetailTask.Show();
        }
    }

    public class GuessesItem
    {
        public string Name { get; set; }
    }
}