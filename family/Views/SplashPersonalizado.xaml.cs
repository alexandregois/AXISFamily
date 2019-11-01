using family.Services.ServiceRealm;
using family.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace family.Views
{
    public partial class SplashPersonalizado : ContentPage
    {
        public App _app => (Application.Current as App);

        private ViewModelSplash _viewModel { get; set; }

        public SplashPersonalizado()
        {
            InitializeComponent();


            Device.BeginInvokeOnMainThread(() =>
            {
                //imageLogo.Source = ImageSource.FromFile("family_splash.png");

                imageLogo.SetBinding(Image.SourceProperty, "SplashImage");


                if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS)
                {
                    imageLogo.HeightRequest = (_app.ScreenHeight / 100) * 10;
                    Task.Delay(7000);
                }
                else
                {
                    imageLogo.HeightRequest = (_app.ScreenHeight / 100) * 20;  // 15;
                    Task.Delay(5000);
                }

            });


            _viewModel = new ViewModelSplash();
            this.BindingContext = _viewModel;


            TokenDataStore token = new TokenDataStore();
            Boolean? existIsLocator = token.ExistIsLocator();

            if (existIsLocator.HasValue)
            {
                _app.Util.TrackService(existIsLocator.Value);

                Device.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage.Navigation.PushAsync(new ViewListaUnidadeRastreada());
                });

                return;
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                Application.Current.MainPage.Navigation.PushAsync(new ViewLogin(_app.isPersonalizado, _app.nameProject));
            });

        }
    }
}