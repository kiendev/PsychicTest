using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace PsychicTest.View
{
    public partial class StartPage : PhoneApplicationPage
    {
        public StartPage()
        {
            InitializeComponent();
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            base.OnBackKeyPress(e);

            Application.Current.Terminate();
        }

        private void ZenerSelect(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/ChooseCards.xaml?type=zener", UriKind.Relative));
        }

        private void ColorSelect(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/ChooseCards.xaml?type=color", UriKind.Relative));
        }

        private void SettingSelect(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/Settings.xaml", UriKind.Relative));
        }

        private void HighScoreSelect(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/Score.xaml", UriKind.Relative));
        }
    }
}