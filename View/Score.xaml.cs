using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Controls;
using PsychicTest.Model;

namespace PsychicTest.View
{
    public partial class Score : PhoneApplicationPage
    {
        public Score()
        {
            InitializeComponent();

            Loaded += Score_Loaded;
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            base.OnBackKeyPress(e);

            e.Cancel = true;

            NavigationService.Navigate(new Uri("/View/StartPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Score_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var listScore = IsolatedStorageSettings.ApplicationSettings["Score"] as ObservableCollection<ScoreItem>;
                if (listScore != null)
                {
                    listScore = new ObservableCollection<ScoreItem>(listScore.OrderByDescending(x => x.RatioValue));

                    for (int i = 0; i < listScore.Count; i++)
                    {
                        listScore[i].ScoreId = i + 1;
                    }
                }

                LstHighScore.ItemsSource = listScore;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }
    }
}