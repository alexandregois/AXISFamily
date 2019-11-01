using System;
using family.CustomElements;
using family.ViewModels;
using family.Views.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace family.Views.PartialViews
{
	public partial class PartialViewAncora : ContentView, IPartialView
	{
		public ViewModelMapa _viewModelMapa { get; set; }

		public ViewModelAncora _viewModel { get; set; }

		public PartialViewAncora(
			ViewModelMapa paramViewModelMapa
		)
		{
			InitializeComponent();

			_viewModelMapa = paramViewModelMapa;

			_viewModel = new ViewModelAncora(
				_viewModelMapa
			);
			_viewModel.ActualView = this as IPartialView;
			this.BindingContext = _viewModel;

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

        public void SelectPinMapa(Pin PinMapa) { }

        public void OnDisappearing()
		{
			_viewModel.OnDisappearing();
		}

		public void MontaPainel()
		{
			CustomLabel labelEndereco = new CustomLabel()
			{
				MaxLines = 2,
				TextColor = Color.White,
				FontSize = 16,
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Margin = new Thickness(13,5,5,5)
			};

			labelEndereco.PropertyChanged += CustomLabelHeight;

			labelEndereco.SetBinding(
				Label.TextProperty
				, new Binding(
					"LabelEnderecoText"
					, BindingMode.Default
					, null
					, null
					, null
					, this.BindingContext
				)
			);

			PanelLabel.Children.Add(labelEndereco);

			CustomSlider sliderPontoControle = new CustomSlider()
			{
				HeightRequest = 40,
				Maximum = 500,
				Minimum = 50,
				Color = Color.White,
                WidthRequest = _viewModel.DefaultWidth
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

		public void CustomLabelHeight(
			object sender, 
			System.ComponentModel.PropertyChangedEventArgs e
		)
		{
			if(e.PropertyName == "Height")
			{
				Label temp = ((Label)sender);
				double altura = temp.Height + temp.Margin.Bottom + temp.Margin.Top;

				if(altura > PanelLabel.HeightRequest)
				{
					PanelLabel.HeightRequest = altura;
				}
			}
		}

	}
}
