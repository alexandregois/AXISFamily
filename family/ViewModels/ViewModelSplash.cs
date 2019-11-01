using family.Domain.Realm;
using family.Model;
using family.Services.ServiceRealm;
using family.ViewModels.Base;
using System;
using Xamarin.Forms;

namespace family.ViewModels
{
    public class ViewModelSplash : ViewModelBase
    {
        protected App _app => (Application.Current as App);


        private ImageSource _powerBySource;
        public ImageSource PowerBySource
        {
            get
            {
                return _powerBySource;
            }
            set
            {
                _powerBySource = value;
                this.Notify("PowerBySource");
            }
        }

        private Double _gridRowTop;
        public Double GridRowTop
        {
            get
            {
                return _gridRowTop;
            }
            set
            {
                _gridRowTop = value;
                this.Notify("GridRowTop");
            }
        }

        private Color _corFundoLogin;
        public Color CorFundoLogin
        {
            get
            {
                return _corFundoLogin;
            }
            set
            {
                _corFundoLogin = value;
                this.Notify("CorFundoLogin");
            }
        }

        private Double _gridRowBotton;
        public Double GridRowBotton
        {
            get
            {
                return _gridRowBotton;
            }
            set
            {
                _gridRowBotton = value;
                this.Notify("GridRowBotton");
            }
        }

        private Double _gridRowCenter;
        public Double GridRowCenter
        {
            get
            {
                return _gridRowCenter;
            }
            set
            {
                _gridRowCenter = value;
                this.Notify("GridRowCenter");
            }
        }

        private ImageSource _splashImage;
        public ImageSource SplashImage
        {
            get
            {
                return _splashImage;
            }
            set
            {
                _splashImage = value;
                this.Notify("SplashImage");
            }
        }

        public ViewModelSplash()
        {

            Device.BeginInvokeOnMainThread(() =>
            {

                if (!_app.isPersonalizado)
                {
                    if (Application.Current.Properties.ContainsKey("UrlLogo"))
                    {
                        String strUrl = (string)Application.Current.Properties["UrlLogo"];

                        if (strUrl != null && strUrl.IndexOf("http") > -1)
                            SplashImage = ImageSource.FromUri(new Uri(strUrl));
                        else
                            SplashImage = ImageSource.FromFile("family_splash.png");
                    }
                    else
                        SplashImage = ImageSource.FromFile("family_splash.png");
                }
                else
                {
                    SplashImage = ImageSource.FromFile("splash.png");                    
                    PowerBySource = ImageSource.FromFile("systemsatPoweredOff.png");

                    if (_app.nameProject == "atm")
                        CorFundoLogin = Color.FromHex("#440103");
                }


                GridRowTop = (_app.ScreenHeight / 100) * 40;
                GridRowCenter = (_app.ScreenHeight / 100) * 75;
                GridRowBotton = (_app.ScreenHeight / 100) * 10;

            });

        }

        public override void OnAppearing()
        {

        }

        public override void OnDisappearing()
        {

        }

        public override void OnLayoutChanged()
        {

        }
    }
}
