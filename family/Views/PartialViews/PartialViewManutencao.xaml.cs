using System;
using System.Collections.Generic;
using family.ViewModels;
using family.Views.Interfaces;
using Xamarin.Forms;
using family.Domain;
using family.Views.Template;
using family.CustomElements;
using family.Resx;
using family.ViewModels.InterfaceServices;
using Xamarin.Forms.GoogleMaps;

namespace family.Views.PartialViews
{
    public partial class PartialViewManutencao : ContentView, IPartialView
    {
        public ViewModelMapa _viewModelMapa { get; set; }

        protected readonly IMessageService _messageService;

        public ViewModelManutencao _viewModel { get; set; }

        public PartialViewManutencao(
            ViewModelMapa paramViewModelMapa
        )
        {
            InitializeComponent();
            _viewModelMapa = paramViewModelMapa;

            this._messageService =
                    DependencyService.Get<IMessageService>();

            _viewModel = new ViewModelManutencao(
                _viewModelMapa
            );
            _viewModel.ActualView = this as IPartialView;
            this.BindingContext = _viewModel;

            _viewModel.VisualizacaoIsVisible = true;
            _viewModel.EdicaoIsVisible = false;


            imgDeleteData.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(LimpaData),
                NumberOfTapsRequired = 1
            });
                        
            //MontaPainel();

            //_viewModelMapa.ActualView.ExibirLoad();
            //_viewModelMapa.ActualView.EscondeLoad();
        }

        private void AtivaData()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                txtData.IsVisible = false;
                dtpSeguro.Date = DateTime.Now.ToLocalTime();
                dtpSeguro.IsVisible = true;
                imgDeleteData.IsVisible = true;
            });
        }

        private void LimpaData()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                txtData.IsVisible = true;
                dtpSeguro.Date = DateTime.Now.ToLocalTime();
                dtpSeguro.IsVisible = false;
                imgDeleteData.IsVisible = false;
            });
        }

        public void OnAppearing()
        {
            try
            {
                _viewModel.VisualizacaoIsVisible = true;
                _viewModel.EdicaoIsVisible = false;

                _viewModel.OnAppearing();
            }
            catch (Exception ex)
            {

            }
            
        }

        public void OnDisappearing()
        {
            _viewModel.OnDisappearing();
        }

        public void MontaPainel()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                PainelDados.IsVisible = _viewModel.VisualizacaoIsVisible;
                PainelDadosEdicao.IsVisible = _viewModel.EdicaoIsVisible;
            });
        }

        private void EntryOnlyNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            string newValue = e.NewTextValue;
            double value;

            if (!String.IsNullOrEmpty(newValue))
            {
                if (!double.TryParse(newValue, out value))
                {
                    _messageService.ShowAlertAsync(
                 AppResources.SomenteNumero
                 , AppResources.Error);

                }

            }
        }

        private void txtData_Focused(object sender, FocusEventArgs e)
        {
            AtivaData();
        }
    }
}
