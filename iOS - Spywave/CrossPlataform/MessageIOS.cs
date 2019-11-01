using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AudioToolbox;
using CoreGraphics;
using family.CrossPlataform;
using family.iOS.CrossPlataform;
using Foundation;
using UIKit;


[assembly: Xamarin.Forms.Dependency(typeof(MessageIOS))]
namespace family.iOS.CrossPlataform
{
    public class MessageIOS : IMessage
    {

        public void ShowToast(string toastString)
        {
            Vibration();
            var toast = new XToast();
            toast.Show(UIApplication.SharedApplication.KeyWindow.RootViewController.View, toastString);
        }
        /// <summary>
        /// Vibrate device with default length
        /// </summary>
        /// <param name="milliseconds">Ignored (iOS doesn't expose)</param>
        public void Vibration(int milliseconds = 500)
        {
            SystemSound.Vibrate.PlaySystemSound();
        }
        private class XToast
        {
            private readonly UIView _view;
            private readonly UILabel _label;
            private const int Margin = 30;
            private const int Height = 40;
            private const int Width = 150;

            private NSTimer _timer;

            public XToast()
            {
                _view = new UIView(new CGRect(0, 0, 0, 0))
                {
                    BackgroundColor = UIColor.FromRGB(0, 175, 240)
                };
                _view.Layer.CornerRadius = (nfloat)20.0;

                _label = new UILabel(new CGRect(0, 0, 0, 0))
                {
                    TextAlignment = UITextAlignment.Center,
                    TextColor = UIColor.White
                };
                _view.AddSubview(_label);

            }

            public void Show(UIView parent, string message)
            {
                if (_timer != null)
                {
                    _timer.Invalidate();
                    _view.RemoveFromSuperview();
                }

                _view.Alpha = (nfloat)0.7;

                _view.Frame = new CGRect(
                    (parent.Bounds.Width - Width) / 2,
                    parent.Bounds.Height - Height - Margin,
                    Width,
                    Height);

                _label.Frame = new CGRect(0, 0, Width, Height);
                _label.Text = message;

                parent.AddSubview(_view);

                var wait = 10;
                _timer = NSTimer.CreateRepeatingScheduledTimer(TimeSpan.FromMilliseconds(100), delegate {
                    if (_view.Alpha <= 0)
                    {
                        _timer.Invalidate();
                        _view.RemoveFromSuperview();
                    }
                    else
                    {
                        if (wait > 0)
                        {
                            wait--;
                        }
                        else
                        {
                            _view.Alpha -= (nfloat)0.05;
                        }
                    }
                });
            }
        }

    }
}