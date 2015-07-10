using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Coding4Fun.Toolkit.Controls;
using Microsoft.Phone.Controls;
using PsychicTest.Controls;
using PsychicTest.Helper;
using PsychicTest.Model;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace PsychicTest.View
{
    public partial class ChooseCards : PhoneApplicationPage
    {
        #region Private member

        private PopupMsg _toast;
        private ObservableCollection<CardItem> _listItems;

        private string _cardId;
        private int _limit;
        private int _selectedId;
        private int _hitCount;
        private int _totalCount;
        private int _countDown;
        private bool _isProcessing;
        private bool _isShowMenu;
        private bool _enableSound;
        private bool _isTrickMode;

        private const double Rate = 0.2;

        private readonly MediaElement _audioElement = new MediaElement();

        public int Limit
        {
            get { return _limit; }
        }

        #endregion

        #region Contructor

        public ChooseCards()
        {
            InitializeComponent();

            Loaded += ChooseCards_Loaded;
        }

        private void ChooseCards_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //Default setting
                _limit = Convert.ToInt32(IsolatedStorageSettings.ApplicationSettings["CountPredict"].ToString());
                _enableSound = IsolatedStorageSettings.ApplicationSettings["EnableSound"].ToString().Equals("1");

                //Add media element
                LayoutRoot.Children.Add(_audioElement);

                ResetGame();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private void ResetGame()
        {
            _countDown = 30;
            _hitCount = 0;
            _totalCount = 0;
            _isProcessing = false;
            _isShowMenu = false;
            _isTrickMode = false;

            SetCount();
            SetTimer("30");
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);

            if (LayoutRoot.Children.Contains(_audioElement))
                LayoutRoot.Children.Remove(_audioElement);

            if (ScoreGrid.Visibility == Visibility.Visible)
            {
                e.Cancel = true;

                GameGrid.Visibility = Visibility.Visible;
                ImgQuickMenu.Visibility = Visibility.Visible;
                ScoreGrid.Visibility = Visibility.Collapsed;

                TbPlayername.Text = string.Empty;
            }
            else
            {
                if(_toast != null)
                    _toast.Close();
                PopupManager.Instance.Hide();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.NavigationMode == NavigationMode.New)
            {
                string type;

                if (NavigationContext.QueryString.TryGetValue("type", out type))
                {
                    if (type.Equals("zener"))
                    {
                        _listItems = App.ListCards;
                    }
                    else if (type.Equals("color"))
                    {
                        _listItems = App.ListColors;
                    }

                    ListBoxCards.ItemsSource = _listItems;
                }

                //_timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 1, 0) };
                //_timer.Tick += mytime_Tick;
            }
        }

        private void SetCount()
        {
            TxtCountTime.Text = _hitCount + "/" + _totalCount;
        }

        private void SetTimer(string tik)
        {
            TxtTimer.Text = ":" + tik;
        }

        #endregion

        #region Image Tap

        private void ImgCardOnTap(object sender, GestureEventArgs e)
        {
            try
            {
                if (_isShowMenu)
                {
                    _isShowMenu = false;
                    CloseMenuStoryboard.Begin();
                }

                //If total select card < 10, continue
                if (_totalCount < Limit)
                {
                    var image = sender as Image;

                    if (image != null && !_isProcessing)
                    {
                        //Mark processing
                        _isProcessing = true;
                        _cardId = image.Tag.ToString();

                        //Init Random
                        var rand = new Random();
                        _selectedId = !_isTrickMode ? rand.Next(0, 5) : Convert.ToInt32(_cardId);

                        if (_listItems != null && _listItems.Count > 0)
                        {
                            //Set image to Front Card
                            var lagerImg = _listItems[_selectedId].CardThumbLager;

                            FrontCard.Source = new BitmapImage(new Uri(lagerImg, UriKind.RelativeOrAbsolute));

                            FlipCardStoryboard.Begin();
                            FlipCardStoryboard.Completed += FlipCardStoryboard_Completed;
                        }
                    }
                }
                else
                {
                    Debug.WriteLine("End game");

                    var defaultRatio = string.Empty;
                    var ratio = Formula(_hitCount, _totalCount);

                    if (ratio <= 1.96)
                        defaultRatio = "not very significant";
                    else if (1.96 < ratio && ratio <= 2.58)
                        defaultRatio = "not significant";
                    else if (2.58 < ratio && ratio <= 3)
                        defaultRatio = "significant";
                    else if (3 < ratio)
                        defaultRatio = "very significant";

                    GameGrid.Visibility = Visibility.Collapsed;
                    ImgQuickMenu.Visibility = Visibility.Collapsed;
                    ScoreGrid.Visibility = Visibility.Visible;

                    TxtResult.Text = _hitCount + " Out of " + _totalCount;
                    TxtRatio.Text = "Ratio: " + ratio + " " + defaultRatio;
                    TbPlayername.Focus();
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private void mytime_Tick(object sender, EventArgs e)
        {
            try
            {
                _countDown--;
                SetTimer(_countDown.ToString());
            }
            catch (Exception exception)
            {
               Debug.WriteLine(exception.Message);
            }
        }

        private void FlipCardStoryboard_Completed(object sender, EventArgs e)
        {
            try
            {
                Debug.WriteLine("FlipCard");

                _totalCount++;

                if (!_cardId.Equals(_selectedId.ToString()))
                {
                    if (_enableSound)
                    {
                        //Play sound effect
                        _audioElement.Source = new Uri("/Assets/Sound/Fail_sound_effect_1.mp3", UriKind.Relative);
                        _audioElement.MediaEnded += (o, args) => _audioElement.Stop();
                        _audioElement.Play();
                    }

                    _toast = new PopupMsg("Wrong", false);
                    _toast.SelectCompleted += delegate(string result)
                    {
                        if (result.Equals("ok") && !_isProcessing)
                        {
                            Debug.WriteLine("CloseCard");

                            _isProcessing = true;

                            CloseCardStoryboard.Begin();
                            CloseCardStoryboard.Completed += CloseCardStoryboard_Completed;
                        }
                    };
                    _toast.Show();
                }
                else
                {
                    _hitCount++;

                    if (_enableSound)
                    {
                        //Play sound effect
                        _audioElement.Source = new Uri("/Assets/Sound/Ding_Sound_Effect.mp3", UriKind.Relative);
                        _audioElement.MediaEnded += (o, args) => _audioElement.Stop();
                        _audioElement.Play();
                    }

                    _toast = new PopupMsg("Hit", true);
                    _toast.SelectCompleted += delegate(string result)
                    {
                        Debug.WriteLine("CloseCard");

                        if (result.Equals("ok") && !_isProcessing)
                        {
                            _isProcessing = true;

                            CloseCardStoryboard.Begin();
                            CloseCardStoryboard.Completed += CloseCardStoryboard_Completed;
                        }
                    };
                    _toast.Show();
                }

                _isProcessing = false;
                SetCount();

                FlipCardStoryboard.Completed -= FlipCardStoryboard_Completed;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private void CloseCardStoryboard_Completed(object sender, EventArgs e)
        {
            try
            {
                FlipCardStoryboard.Stop();
                CloseCardStoryboard.Stop();

                _toast.Close();

                _isProcessing = false;

                if (Limit.Equals(_totalCount))
                {
                    Debug.WriteLine("End game");

                    var defaultRatio = string.Empty;
                    var ratio = Formula(_hitCount, _totalCount);

                    if (ratio <= 1.96)
                        defaultRatio = "not very significant";
                    else if (1.96 < ratio && ratio <= 2.58)
                        defaultRatio = "not significant";
                    else if (2.58 < ratio && ratio <= 3)
                        defaultRatio = "significant";
                    else if (3 < ratio)
                        defaultRatio = "very significant";

                    GameGrid.Visibility = Visibility.Collapsed;
                    ImgQuickMenu.Visibility = Visibility.Collapsed;
                    ScoreGrid.Visibility = Visibility.Visible;

                    TxtResult.Text = _hitCount + " Out of " + _totalCount;
                    TxtRatio.Text = "Ratio: " + ratio + " " + defaultRatio;
                    TbPlayername.Focus();
                }

                CloseCardStoryboard.Completed -= CloseCardStoryboard_Completed;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private void cardBack_Tap(object sender, GestureEventArgs e)
        {
            if (_isShowMenu)
            {
                _isShowMenu = false;
                CloseMenuStoryboard.Begin();
            }
        }

        private void FrontCard_OnTap(object sender, GestureEventArgs e)
        {
            try
            {
                //CloseCardStoryboard.Begin();
                //CloseCardStoryboard.Completed += delegate(object o, EventArgs args)
                //{

                //};
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        #endregion

        #region Quick menu

        private void OpenQuickMenu(object sender, GestureEventArgs e)
        {
            try
            {
                if (!_isShowMenu)
                {
                    _isShowMenu = true;
                    QuickMenuStoryboard.Begin();
                }
                else
                {
                    _isShowMenu = false;
                    CloseMenuStoryboard.Begin();
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private void ResetGameTap(object sender, GestureEventArgs e)
        {
            try
            {
                FlipCardStoryboard.Stop();
                CloseCardStoryboard.Stop();

                ResetGame();

                CloseMenuStoryboard.Begin();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private void NewTestTap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/StartPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void TopPsychicTap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/Score.xaml", UriKind.RelativeOrAbsolute));
        }

        #endregion

        #region Save Score

        private void TbPlayername_OnKeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    if (!string.IsNullOrEmpty(TxtResult.Text.Trim()))
                    {
                        var listScore =
                            IsolatedStorageSettings.ApplicationSettings["Score"] as ObservableCollection<ScoreItem>;

                        if (listScore != null)
                        {
                            if (listScore.Count > 0)
                            {
                                var lastIndex = listScore[listScore.Count - 1].ScoreId;

                                var currentScore = new ScoreItem()
                                {
                                    ScoreId = lastIndex + 1,
                                    PlayerName = TbPlayername.Text,
                                    Score = _hitCount + "/" + _totalCount,
                                    RatioValue = Formula(_hitCount, _totalCount),
                                    Time = DateTime.Now
                                };

                                listScore.Add(currentScore);
                            }
                            else
                            {
                                var currentScore = new ScoreItem()
                                {
                                    ScoreId = 1,
                                    PlayerName = TbPlayername.Text,
                                    Score = _hitCount + "/" + _totalCount,
                                    RatioValue = Formula(_hitCount, _totalCount),
                                    Time = DateTime.Now
                                };

                                listScore.Add(currentScore);
                            }

                            IsolatedStorageSettings.ApplicationSettings["Score"] = listScore;
                            IsolatedStorageSettings.ApplicationSettings.Save();
                        }
                    }
                    else
                    {
                        ToastPrompt toast = new ToastPrompt();
                        toast.Message = "Enter your name to continue!";
                        toast.Show();
                    }

                    NavigationService.Navigate(new Uri("/View/Score.xaml", UriKind.RelativeOrAbsolute));
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        //TrickMode
        private void TrickModeTap(object sender, GestureEventArgs e)
        {
            try
            {
                if (_isTrickMode)
                    _isTrickMode = false;
                else _isTrickMode = true;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private double Formula(int r, int g)
        {
            try
            {
                var a = g*Rate;
                var b = 0.16;
                var c = b*g;
                var s = Math.Sqrt(c);
                var d = r - a;
                var m = d/s;

                return Math.Round(m, 2, MidpointRounding.AwayFromZero);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
                return 0;
            }
        }

        #endregion
    }
}