using System;
using System.Collections.Generic;
using family.ViewModels;
using family.Views.Interfaces;
using Xamarin.Forms;
using family.Domain;
using family.Views.Template;
using family.CustomElements;
using Xamarin.Forms.GoogleMaps;
using family.Resx;

namespace family.Views.PartialViews
{
	public partial class PartialViewPontoControle : ContentView, IPartialViewPontoControle
	{
		public ViewModelMapa _viewModelMapa { get; set; }

		public ViewModelPontoControle _viewModel { get; set; }

		public PartialViewPontoControle(
			ViewModelMapa paramViewModelMapa
		)
		{
			InitializeComponent();
			_viewModelMapa = paramViewModelMapa;

			_viewModel = new ViewModelPontoControle(
				_viewModelMapa
			);
			_viewModel.ActualView = this as IPartialViewPontoControle;
			this.BindingContext = _viewModel;

			ListPontosControle.ItemTemplate = new DataTemplate(() => {
				return new ListPontosControle_ViewCell(this.BindingContext);
			});


            labelCalendar.Text = AppResources.DayTime;
            labelCalendarConfigurar.Text = AppResources.Configurar;

            labelPainelCalendar.Text = AppResources.DayTime;
            labelFechar.Text = AppResources.Close;

            labelCalendarConfigurar.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = _viewModel.OpenSemanaCommand,
                NumberOfTapsRequired = 1
            });
                       

            labelFechar.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = _viewModel.CloseSemanaCommand,
                NumberOfTapsRequired = 1
            });


            ImageCheck.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(ImageCheckedChanged),
                NumberOfTapsRequired = 1
            });



            MontaPainel();
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

        #region Lista Ponto de Controle
        public void BeginRefresh()
		{
			Device.BeginInvokeOnMainThread(() => {
				ListPontosControle.BeginRefresh();
			});
		}

		public void EndRefresh()
		{
			Device.BeginInvokeOnMainThread(() => {
				ListPontosControle.EndRefresh();
			});
		}

		public void ScrollTop(PontoControle paramPosicao)
		{
			Device.BeginInvokeOnMainThread(() => {
				ListPontosControle.ScrollTo(
					paramPosicao
					, ScrollToPosition.MakeVisible
					, false
				);
			});
		}

		public void SelectedItem(PontoControle paramPosicao)
		{
			Device.BeginInvokeOnMainThread(() => {
				ListPontosControle.SelectedItem = paramPosicao;
			});
		}

		private void ListPontosControleItemSelected(
			object sender
			, SelectedItemChangedEventArgs e
		)
		{

			_viewModel.ListPontosControleItemSelected(e.SelectedItem as PontoControle);

		}
		#endregion

		public void MontaPainel()
		{

			CustomSlider sliderPontoControle = new CustomSlider()
			{
				HeightRequest = 15,
				Maximum = 500,
				Minimum = 50,
				Color = Color.White
			};

			sliderPontoControle.ValueChanged += _viewModel.SliderPontoControle_ValueChanged;
			sliderPontoControle.Released += _viewModel.SliderPontoControle_ButtonPress;
			sliderPontoControle.ButtonPress +=  _viewModel.SliderPontoControle_ButtonPress;

			sliderPontoControle.SetBinding(
				Slider.ValueProperty
				, new Binding(
					"SliderValue"
					, BindingMode.Default
					, null
					, null
					, null
					, this.BindingContext
				)
			);

			sliderPontoControle.SetBinding(
				Slider.IsEnabledProperty
				, new Binding(
					"SliderEnable"
					, BindingMode.Default
					, null
					, null
					, null
					, this.BindingContext
				)
			);

			RaioAncoraBox.Children.Add(sliderPontoControle);

		}

        private void ImageCheckedChanged()
        {
            var selectedImage = ImageCheck.Source as FileImageSource;

            if (selectedImage.File == "checkbox_off.png")
            {
                // perform action checkbox is checked;
                ImageCheck.Source = "checkbox_on.png";
                _viewModel.GetCheck(true);
            }

            if (selectedImage.File == "checkbox_on.png")
            {
                // perform action when checkbox is unchecked
                ImageCheck.Source = "checkbox_off.png";
                _viewModel.GetCheck(false);
            }

            
        }

    }
}
