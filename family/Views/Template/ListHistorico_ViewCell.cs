using System;
using family.Domain.Dto;
using family.Resx;
using family.ViewModels;
using family.ViewModels.InterfaceServices;
using Xamarin.Forms;

namespace family.Views.Template
{
	public class ListHistorico_ViewCell : ViewCellBase
	{
		private readonly IMessageService _messageService;
		private readonly INavigationService _navigationService;
		private Thickness _gridPadding;
		private Double _gridMinHeight;
		private Grid _grid;
		private Label collum0;
		private Label collum1;

		private ViewModelHistorico _viewModel;

		public ListHistorico_ViewCell(
			Object paramContext
		) : base(Color.Orange) //(Color.White)
		{

			_viewModel = (ViewModelHistorico)paramContext;

			_gridPadding = new Thickness(10);
			_gridMinHeight = 40;

			this._messageService =
				    DependencyService.Get<IMessageService>();

			this._navigationService =
				    DependencyService.Get<INavigationService>();

		}

		private void MontaAction(
			PosicaoHistorico paramPontoControle
		)
		{
			MenuItem edit = new MenuItem()
			{
				Text = AppResources.Details,
				Command = _viewModel.ListViewPosicoesDetalhesCommand,
				CommandParameter = paramPontoControle
			};
			ContextActions.Add(edit);
		}

		protected override void OnBindingContextChanged()
		{
			try
			{
				base.OnBindingContextChanged();

				PosicaoHistorico pontoControle 
				= (PosicaoHistorico)BindingContext;

				if(pontoControle != null)
				{
					//MontaAction(pontoControle);
					_grid = new Grid()
					{
						ColumnSpacing = 10,
						RowSpacing = 0,
						Margin = _gridPadding
					};
					_grid.ColumnDefinitions = new ColumnDefinitionCollection();
					_grid.ColumnDefinitions.Add(new ColumnDefinition()
					{
						Width = 85
					});
					_grid.ColumnDefinitions.Add(new ColumnDefinition()
					{
						Width = GridLength.Star
					});

					_grid.RowDefinitions = new RowDefinitionCollection();
					_grid.RowDefinitions.Add(new RowDefinition()
					{
						Height = GridLength.Auto
					});

					collum0 = new Label()
					{
						TextColor = Color.White,
						FontSize = 14,
						Margin = new Thickness(0),
						Text = pontoControle.StringVelocidade,
						VerticalTextAlignment = TextAlignment.Center
					};

					collum1 = new Label()
					{
						TextColor = Color.White,
						FontSize = 14,
						Margin = new Thickness(0),
						Text = pontoControle.StringDataEvento,
						VerticalTextAlignment = TextAlignment.Center
					};

					_grid.Children.Add(collum0, 0, 0);
					_grid.Children.Add(collum1, 1, 0);


                    _grid.GestureRecognizers.Add(new TapGestureRecognizer
                    {
                        Command = _viewModel.ListViewPosicoesDetalhesCommand,
                        //Command = new Command(OpenDetalhesAsync),
                        CommandParameter = pontoControle,
                        NumberOfTapsRequired = 1
                    });

                    View = _grid;
				}

			}
			catch (Exception)
			{

			}
		}

        protected void OpenDetalhesAsync()
        {
            Application.Current.MainPage.Navigation.PushModalAsync(new ViewHistoricoDetalhes());
        }

	}
}
