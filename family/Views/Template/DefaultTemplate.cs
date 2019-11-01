using Xamarin.Forms;
using System;
using family.ViewModels.Base;

namespace family.Views.Template
{
	public class DefaultTemplate : Grid
	{
		public ViewModelBase _viewModel;
		protected App _app => (Application.Current as App);



		public DefaultTemplate()
		{
			ColumnSpacing = 0;
			RowSpacing = 0;
			VerticalOptions = LayoutOptions.FillAndExpand;
			BackgroundColor = Color.FromHex("#FF363636");
			Margin = _app.DefaultTemplateMargin;
			WidthRequest = _app.ScreenWidth;

			GridColumnDefinitions();
			GridRowDefinition();
			GridAddChildren();
		}

		private void GridColumnDefinitions()
		{
			ColumnDefinitions = new ColumnDefinitionCollection();

			ColumnDefinition column = new ColumnDefinition()
			{
				Width = GridLength.Star
			};

			ColumnDefinitions.Add(column);
		}

		private void GridRowDefinition()
		{
			RowDefinition row01 = new RowDefinition()
			{
				Height = _app.DefaultTemplateHeightNavegationBar
			};

			RowDefinition row02 = new RowDefinition()
			{
				Height = _app.DefaultTemplateNavegationLine
			};

			RowDefinition row03 = new RowDefinition()
			{
				Height = _app.DefaultTemplateHeightContent
			};

			RowDefinitions = new RowDefinitionCollection();
			RowDefinitions.Add(row01);
			RowDefinitions.Add(row02);
			RowDefinitions.Add(row03);
		}

		private void GridAddChildren()
		{

			Children.Add(MontaGridCabecalho(), 0, 0);

			StackLayout PanelBarra = new StackLayout()
			{
				Margin = new Thickness(0),
				HeightRequest = _app.DefaultTemplateNavegationLine,
				Spacing = 0
			};

			PanelBarra.SetBinding(
				StackLayout.BackgroundColorProperty
				, new TemplateBinding( "Parent.BindingContext.PageColor" )
			);
			Children.Add(PanelBarra, 0, 1);

			ContentPresenter contentPresenter = new ContentPresenter()
			{
				Margin = new Thickness(0),
				WidthRequest = this.WidthRequest,
				HeightRequest = _app.DefaultTemplateHeightContent
			};

			Children.Add(contentPresenter, 0, 2);
			
		}

		public Grid MontaGridCabecalho()
		{
			Grid collum0 = new Grid()
			{
				RowSpacing = 0,
				ColumnSpacing = 0,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				HeightRequest = _app.DefaultTemplateHeightNavegationBar
			};

			RowDefinition row01 = new RowDefinition()
			{
				Height = GridLength.Star
			};

			collum0.RowDefinitions = new RowDefinitionCollection();
			collum0.RowDefinitions.Add(row01);

			collum0.ColumnDefinitions = new ColumnDefinitionCollection();

			collum0.ColumnDefinitions.Add(new ColumnDefinition()
			{
				Width = 30
			});

			collum0.ColumnDefinitions.Add(new ColumnDefinition()
			{
				Width = 65
			});

			collum0.ColumnDefinitions.Add(new ColumnDefinition()
			{
				Width = GridLength.Star
			});

			collum0.ColumnDefinitions.Add(new ColumnDefinition()
			{
				Width = GridLength.Auto
			});

			Button voltarBox = new Button()
			{
				Margin = new Thickness(0, 0, 0, 0),
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Image = "seta_esquerda.png",
				BackgroundColor = Color.Transparent,
				BorderRadius = 0,
				BorderWidth = 0,
				HeightRequest = collum0.HeightRequest
			};

			voltarBox.SetBinding(
				Button.CommandProperty
				, new TemplateBinding ("Parent.BindingContext.VoltarCommand")
			);

			voltarBox.SetBinding(
				Button.IsVisibleProperty
				, new TemplateBinding ("Parent.BindingContext.VoltarVisible")
			);

			collum0.Children.Add(voltarBox, 0, 0);

			StackLayout panelTituloImagemBox = new StackLayout()
			{
				Margin = new Thickness(10, 0, 0, 0),
				Spacing = 0,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.Center,
				HeightRequest = collum0.HeightRequest
			};

			Image imageTemp = new Image()
			{
				Margin = new Thickness(0, 11.5, 0, 0),
				HeightRequest = 30
			};

			panelTituloImagemBox.SetBinding(
				StackLayout.BackgroundColorProperty
				, new TemplateBinding ("Parent.BindingContext.PageColor")
			);

			imageTemp.SetBinding(
				Image.SourceProperty
				, new TemplateBinding ("Parent.BindingContext.ImageSourceProperty")
			);

			imageTemp.SetBinding(
				Image.WidthProperty
				, new TemplateBinding ("Parent.BindingContext.ImageWidthProperty")
			);

			panelTituloImagemBox.Children.Add(imageTemp);
			collum0.Children.Add(panelTituloImagemBox, 1, 0);

			Frame boxMiddle = new Frame()
			{
				HasShadow = false,
				Margin = new Thickness(10, 0),
				Padding = 0,
				HorizontalOptions = LayoutOptions.Start,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				BackgroundColor = Color.Transparent,
				HeightRequest = collum0.HeightRequest,
				CornerRadius = 0,
				Opacity = 1
			};

			boxMiddle.SetBinding(
				Frame.ContentProperty
				, new TemplateBinding("Parent.BindingContext.BoxMiddleContent")
			);
			collum0.Children.Add(boxMiddle, 2, 0);

			Frame boxRight = new Frame()
			{
				Margin = 0,
				Padding = 0,
				HasShadow = false,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				BackgroundColor = Color.Transparent,
				HeightRequest = collum0.HeightRequest,
				CornerRadius = 0,
				Opacity = 1
			};

			boxRight.SetBinding(
				Frame.ContentProperty
				, new TemplateBinding("Parent.BindingContext.BoxRightContent")
			);
			collum0.Children.Add(boxRight, 3, 0);

			return collum0;
		}

	}
}

