using System;
using family.CustomElements;
using family.Domain.Dto;
using family.Resx;
using family.ViewModels;
using family.Views.Interfaces;
using family.Views.Template;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace family.Views.PartialViews
{
    public partial class PartialViewHistorico : ContentView, IPartialViewHistorico
    {
        public ViewModelMapa _viewModelMapa { get; set; }

        public ViewModelHistorico _viewModel { get; set; }

        public CustomDatePicker DataFiltro { get; set; }

        public App _app => (Application.Current as App);

        public PartialViewHistorico(
            ViewModelMapa paramViewModelMapa
        )
        {
            InitializeComponent();

            try
            {

                _viewModelMapa = paramViewModelMapa;

                _viewModel = new ViewModelHistorico(
                    _viewModelMapa
                );
                _viewModel.ActualView = this as IPartialViewHistorico;
                this.BindingContext = _viewModel;

                ListViewPosicoes.ItemTemplate = new DataTemplate(() =>
                {
                    return new ListHistorico_ViewCell(this.BindingContext);
                });

                MontaPainel();

            }
            catch (Exception)
            {

            }

        }

        public void OnAppearing()
        {
            try
            {
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

        public void SelectPinMapa(Pin PinMapa) { }

        private void MontaPainel()
        {
            DataFiltro = new CustomDatePicker()
            {
                Date = _viewModelMapa.ActualPosition.DataEvento.LocalDateTime,
                IsVisible = false
            };
            DataFiltro.MaximumDate = DateTime.UtcNow;
            DataFiltro.DonePress += _viewModel.DataFiltro_DateSelected;
            DataFiltro.Unfocused += _viewModel.DataFiltro_Unfocused;

            DataFiltro.SetBinding(
                DatePicker.DateProperty
                , new Binding(
                    "DataFiltroDate"
                    , BindingMode.Default
                    , null
                    , null
                    , null
                    , this.BindingContext
                )
            );

            PanelSearch.Children.Add(DataFiltro);
        }

        public void BeginRefresh()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                ListViewPosicoes.BeginRefresh();
            });
        }

        public void EndRefresh()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                ListViewPosicoes.EndRefresh();
            });
        }

        public void DataFiltroFocus()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                DataFiltro.Focus();
            });
        }

        public void ScrollTop(PosicaoHistorico paramPosicao)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                ListViewPosicoes.ScrollTo(
                    paramPosicao
                    , ScrollToPosition.MakeVisible
                    , false
                );
            });
        }

        public void SelectedItem(PosicaoHistorico paramPosicao)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                ListViewPosicoes.SelectedItem = paramPosicao;
            });
        }

        private void ListViewPosicoes_ItemSelected(
            object sender
            , SelectedItemChangedEventArgs e
        )
        {
            ListView lista = ((ListView)sender);

            _viewModel.ListViewPosicoesItemSelected(e.SelectedItem as PosicaoHistorico);

        }

    }
}
