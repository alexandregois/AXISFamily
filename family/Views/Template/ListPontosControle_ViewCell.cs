using System;
using System.Collections.Generic;
using family.Resx;
using family.ViewModels;
using family.ViewModels.InterfaceServices;
using Xamarin.Forms;
using family.Domain;

namespace family.Views.Template
{
	public class ListPontosControle_ViewCell : ViewCellBase
	{
		private readonly IMessageService _messageService;
		private readonly INavigationService _navigationService;

		private ViewModelPontoControle _viewModel;

		public ListPontosControle_ViewCell(
			Object paramContext
		) : base(Color.Orange)
		{

			_viewModel = (ViewModelPontoControle)paramContext;

			this._messageService =
				    DependencyService.Get<IMessageService>();

			this._navigationService =
				    DependencyService.Get<INavigationService>();

		}

		private void MontaAction(
			PontoControle paramPontoControle
		)
		{
			MenuItem edit = new MenuItem()
			{
				Text = AppResources.Edit,
				Command = _viewModel.ListPontosControleOnEditCommand,
				CommandParameter = paramPontoControle
			};
			ContextActions.Add(edit);

			MenuItem delete = new MenuItem()
			{
				Text = AppResources.Delete,
				Command = _viewModel.ListPontosControleOnDeleteCommand,
				CommandParameter = paramPontoControle,
				IsDestructive = true
			};
			ContextActions.Add(delete);
		}

		protected override void OnBindingContextChanged()
		{
			try
			{
				base.OnBindingContextChanged();

				PontoControle pontoControle 
				= (PontoControle)BindingContext;

				if(pontoControle != null)
				{
					MontaAction(pontoControle);
					Grid _grid = new Grid()
					{
						Margin = new Thickness(0, 10),
						ColumnSpacing = 0,
						RowSpacing = 0,
						MinimumHeightRequest=53
					};
					_grid.ColumnDefinitions = new ColumnDefinitionCollection();
					_grid.ColumnDefinitions.Add(new ColumnDefinition()
					{
						Width = GridLength.Star
					});
					_grid.ColumnDefinitions.Add(new ColumnDefinition()
					{
						Width = 75
					});

					_grid.RowDefinitions = new RowDefinitionCollection();
					_grid.RowDefinitions.Add(new RowDefinition()
					{
						Height = GridLength.Auto
					});

					Label collum0 = new Label()
					{
						TextColor = Color.White,
						FontSize = 16,
						VerticalOptions = LayoutOptions.Center,
						Margin = new Thickness(10, 0, 0, 0),
						Text = pontoControle.NomePonto
					};

					Label collum1 = new Label()
					{
						TextColor = Color.White,
						FontSize = 16,
						VerticalOptions = LayoutOptions.Center,
						Margin = new Thickness(10, 0),
						Text = pontoControle.StringTolerancia
					};

					_grid.Children.Add(collum0, 0, 0);
					_grid.Children.Add(collum1, 1, 0);

					View = _grid;
				}

			}
			catch (Exception)
			{

			}
		}
	}
}
