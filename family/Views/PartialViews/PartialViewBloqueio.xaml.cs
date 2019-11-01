using System;
using System.Collections.Generic;
using family.ViewModels;
using family.Views.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace family.Views.PartialViews
{
	#pragma warning disable CS4014
	#pragma warning disable RECS0022
	#pragma warning disable CS1998
	public partial class PartialViewBloqueio : ContentView, IPartialView
	{
		public ViewModelMapa _viewModelMapa { get; set; }

		public ViewModelBloqueio _viewModel { get; set; }

		public PartialViewBloqueio(
			ViewModelMapa paramViewModelMapa
		)
		{
			InitializeComponent();

			_viewModelMapa = paramViewModelMapa;

			_viewModel = new ViewModelBloqueio(
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

		public void OnDisappearing()
		{
			_viewModel.OnDisappearing();
		}

		public void MontaPainel()
		{
		}

    }
	#pragma warning restore CS1998
	#pragma warning restore RECS0022
	#pragma warning restore CS4014
}