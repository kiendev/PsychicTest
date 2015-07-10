using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using PsychicTest.Helper;

namespace PsychicTest.Controls
{
    public delegate void TapCompletedHandler(string result);

    public partial class PopupMsg : UserControl
    {
        public event TapCompletedHandler SelectCompleted;
        private Popup _popup = new Popup();

        public PopupMsg(string title, bool isTrue)
        {
            InitializeComponent();

            Height = Application.Current.Host.Content.ActualHeight;
            Width = Application.Current.Host.Content.ActualWidth;

            TxtTitle.Text = title;
            //TxtWarning.Text = warning;

            ImgWarning.Source = isTrue ? new BitmapImage(new Uri("/Assets/Images/checkmark.png", UriKind.Relative)) : new BitmapImage(new Uri("/Assets/Images/cancel.png", UriKind.Relative));
        }

        public void Show()
        {
            _popup.Child = this;
            _popup.IsOpen = true;
        }

        public void Close()
        {
            _popup.IsOpen = false;
        }

        private void BackgroundTap(object sender, GestureEventArgs e)
        {
            try
            {
                //AnimationHide.Begin();
                //AnimationHide.Completed += (o, args) =>
                //{
                //    PopupManager.Instance.Hide();
                //    SelectCompleted("ok");
                //};
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private void GridWarningTap(object sender, GestureEventArgs e)
        {
            try
            {
                AnimationHide.Begin();
                AnimationHide.Completed += (o, args) =>
                {
                    PopupManager.Instance.Hide();
                    SelectCompleted("ok");
                };

            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }
    }
}
