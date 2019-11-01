using System;
using family.Domain.Enum;
using family.Services.ServiceRealm;
using family.ViewModels.InterfaceServices;
using family.Views;
using family.Views.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(NavigationService))]
namespace family.Views.Services
{
#pragma warning disable CS4014
#pragma warning disable RECS0022
    public class NavigationService : INavigationService
    {

        private App _app => (Application.Current as App);

        public void Voltar()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Application.Current.MainPage.Navigation.PopAsync();
            });
        }

        public void NavigateToLogin()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                _app.MainPage = new NavigationPage(new ViewLogin(_app.isPersonalizado, _app.nameProject));
            });
        }

        public void NavigateToListaUnidadeRastreadaPage()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                TokenDataStore token = new TokenDataStore();
                Boolean? existIsLocator = token.ExistIsLocator();
                _app.Util.TrackService(existIsLocator.Value);

                _app.MainPage = new NavigationPage(new ViewListaUnidadeRastreada());
            });
        }

        public void NavigateToCadastro()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Application.Current.MainPage.Navigation.PushAsync(new ViewCadastro());
            });
        }

        public void NavigateToConfiguracao()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Application.Current.MainPage.Navigation.PushAsync(new ViewConfiguracao());
            });
        }

        public void NavigateToTelefone()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Application.Current.MainPage.Navigation.PushAsync(new ViewListaTelefone());
            });
        }

        public void NavigateToStreetView(Double paramLatitude, Double paramLongitude, Boolean paramExibe)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Application.Current.MainPage.Navigation.PushAsync(new ViewStreet(paramLatitude, paramLongitude, paramExibe));
            });
        }

        public void NavigateToMapa(
            EnumPage paramPartialPage
            , int paramIdRastreador
        )
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Application.Current.MainPage.Navigation.PushAsync(
                new ViewMapa(
                    paramPartialPage
                    , paramIdRastreador
                )
                );
            });
        }
    }
#pragma warning restore CS4014
#pragma warning restore RECS0022
}

