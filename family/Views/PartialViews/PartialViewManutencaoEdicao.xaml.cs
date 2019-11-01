using System;
using System.Collections.Generic;
using family.ViewModels;
using family.Views.Interfaces;
using Xamarin.Forms;
using family.Domain;
using family.Views.Template;
using family.CustomElements;
using family.Resx;
using Xamarin.Forms.GoogleMaps;

namespace family.Views.PartialViews
{
    public partial class PartialViewManutencaoEdicao : ContentView, IPartialView
    {
        public ViewModelMapa _viewModelMapa { get; set; }

        public ViewModelManutencao _viewModel { get; set; }

        public PartialViewManutencaoEdicao(
            ViewModelMapa paramViewModelMapa
        )
        {
            InitializeComponent();
            _viewModelMapa = paramViewModelMapa;

            _viewModel = new ViewModelManutencao(
                _viewModelMapa
            );
            _viewModel.ActualView = this as IPartialView;
            this.BindingContext = _viewModel;

            //_viewModel.VisualizacaoIsVisible = true;
            //_viewModel.EdicaoIsVisible = false;

            //MontaPainel();

            //_viewModelMapa.ActualView.ExibirLoad();
            //_viewModelMapa.ActualView.EscondeLoad();
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
                //PainelDadosEdicao.IsVisible = _viewModel.EdicaoIsVisible;
            });
        }

    }
}
