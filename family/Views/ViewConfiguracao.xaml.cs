
using family.CustomElements;
using family.ViewModels;
using family.Views.Template;
using Xamarin.Forms;
using family.Views.Interfaces;
using System;

namespace family.Views
{
    public partial class ViewConfiguracao : ContentPage, ILoader
    {
        private App _app => (Application.Current as App);

        private ViewModelConfiguracao _viewModel { get; set; }

        private Color statusColor = Color.FromHex("#f09356");

        private CustomDialogAlert _dialogAlert = null;
        private CustomDialogAlert DialogAlert
        {
            get
            {
                if (_dialogAlert == null)
                {
                    _dialogAlert = new CustomDialogAlert(
                        Panel
                        , Color.FromHex("#80000000")
                        , false
                    );
                }

                return _dialogAlert;
            }
            set
            {
                _dialogAlert = value;
            }
        }

        public ViewConfiguracao()
        {
            InitializeComponent();

            PanelGeral.ControlTemplate =
                new ControlTemplate(typeof(DefaultTemplate));


            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                PanelContratar.IsVisible = false;
                PanelDesconectar.Margin = new Thickness(0, -57, 0, 0);

            });

                _viewModel = new ViewModelConfiguracao();
            _viewModel._view = this as ILoader;
            this.BindingContext = _viewModel;

            _app.Util.changeColorStatusBar(statusColor, this);


            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {

                if (Device.RuntimePlatform == Device.iOS)
                {
                    _viewModel.ObterInformacaoProdutoLojaAsync();

                    PanelContratar.SetBinding(IsVisibleProperty, "AtivaPainelComprar");
                    //PanelContratar.IsVisible = true;
                    //PanelDesconectar.Margin = new Thickness(0, -25, 0, 0);
                    PanelDesconectar.SetBinding(StackLayout.MarginProperty, "MargemPainelDesconectar");
                    PanelDesconectar.SetBinding(IsVisibleProperty, "AtivaPainelDesconectar");

                }
                else
                {
                    //PanelContratar.IsVisible = false;
                    //PanelDesconectar.Margin = new Thickness(0, -57, 0, 0);
                    PanelContratar.SetBinding(IsVisibleProperty, "AtivaPainelComprar");
                    PanelDesconectar.SetBinding(StackLayout.MarginProperty, "MargemPainelDesconectar");
                    PanelDesconectar.SetBinding(IsVisibleProperty, "AtivaPainelDesconectar");
                }

            });


        }

        protected override void OnAppearing()
        {
            _viewModel.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            _viewModel.OnDisappearing();
        }

        private ActivityIndicator Loader()
        {
            ActivityIndicator tempActivity = DialogAlert.RequireActivityIndicator();
            tempActivity.Color = Color.FromHex("#FF2F2645");
            return tempActivity;
        }

        public void EscondeLoad()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                DialogAlert.HideAndCleanAlert();
                DialogAlert.Destroy();
                DialogAlert = null;
            });
        }

        public void ExibirLoad()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (DialogAlert != null)
                {
                    DialogAlert.HideAndCleanAlert();
                    DialogAlert.Destroy();
                    DialogAlert = null;
                }
                DialogAlert.ShowAlert(Loader());
            });
        }

        public void Is_True()
        {
            throw new System.NotImplementedException();
        }

        public void Is_False()
        {
            throw new System.NotImplementedException();


        }

        public void OpenStreetview(Boolean paramExibe) { }
        public void CloseStreetview(Boolean paramOnAppear) { }
    }
}
