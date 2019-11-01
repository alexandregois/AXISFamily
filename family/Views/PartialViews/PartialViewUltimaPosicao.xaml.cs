using family.CustomElements;
using family.ViewModels;
using family.Views.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace family.Views.PartialViews
{
#pragma warning disable CS4014
#pragma warning disable RECS0022
#pragma warning disable CS1998
	public partial class PartialViewUltimaPosicao : ContentView, IPartialView
	{
		public ViewModelMapa _viewModelMapa { get; set; }
        protected App _app => (Application.Current as App);
        public ViewModelUltimaPosicao _viewModel { get; set; }

        public PartialViewUltimaPosicao(
			ViewModelMapa paramViewModelMapa
		)
		{
			InitializeComponent();

            _viewModelMapa = paramViewModelMapa;

			_viewModel = new ViewModelUltimaPosicao(
				_viewModelMapa
			);

			_viewModel.ActualView = this as IPartialView;


            this.BindingContext = _viewModel;

            _viewModelMapa.ActualView.ExibirLoad();
            
            MontaPainel();

            _viewModelMapa.ActualView.EscondeLoad();

            _app._countClick = 0;

            

        }

		public void MontaPainel()
		{
			CustomLabel labelEndereco = new CustomLabel()
			{
				MaxLines = 2,
				TextColor = Color.White,
				FontSize = 15,
				HorizontalTextAlignment = TextAlignment.Start,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Margin = new Thickness(10,0,5,0)
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
            
            CustomLabel labelDataPosicao = new CustomLabel()
            {
                MaxLines = 1,
                TextColor = Color.White,
                FontSize = 15,
                HorizontalTextAlignment = TextAlignment.Start,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(10, 0, 0, 0)
            };

            labelDataPosicao.PropertyChanged += CustomLabelHeight;

            labelDataPosicao.SetBinding(
                Label.TextProperty
                , new Binding(
                    "LabelDataPosicaoText"
                    , BindingMode.Default
                    , null
                    , null
                    , null
                    , this.BindingContext
                )
            );

            PanelLabel.Children.Add(labelEndereco);
            //PanelLabel.Children.Add(labelDataPosicao);
        }

        public void OnAppearing()
		{
            try
            {

			    _viewModel.OnAppearing();

            }
            catch (System.Exception ex)
            {

            }
        }

		public void OnDisappearing()
		{
			_viewModel.OnDisappearing();
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
	#pragma warning restore CS1998
	#pragma warning restore RECS0022
	#pragma warning restore CS4014
}