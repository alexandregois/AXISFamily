using family.CustomElements;
using family.ViewModels;
using family.Views.Interfaces;
using System;
using Xamarin.Forms;

namespace family.Views
{
	public partial class ViewCadastro : ContentPage, ILoader
	{
		private ViewModelCadastro _viewModelCadastro { get; set; }
		private App _app => (Application.Current as App);

		private CustomDialogAlert _painelTopLoad { get; set; }

		public ViewCadastro()
		{
			InitializeComponent();

			_viewModelCadastro = new ViewModelCadastro();
			_viewModelCadastro._view = this as ILoader;
			this.BindingContext = _viewModelCadastro;

			MontaLoad();
		}

		protected override void OnAppearing ()
		{
			var lifecycleHandler = (ViewModelCadastro) this.BindingContext;
			lifecycleHandler.OnAppearing();

			_app.Util.changeColorStatusBar(Color.FromHex("#08001e"), this);
		}

		protected override void OnDisappearing ()
		{
			var lifecycleHandler = (ViewModelCadastro) this.BindingContext;
			lifecycleHandler.OnDisappearing();
		}

		private void MontaLoad()
		{
			_painelTopLoad = new CustomDialogAlert(
				Panel
				, Color.FromHex("#80000000")
				, false
			);
			ActivityIndicator activity = _painelTopLoad.RequireActivityIndicator();
			activity.Color = Color.FromHex("#FF2F2645");
			_painelTopLoad.AddChildren(activity);

		}

		public void EscondeLoad()
		{
			Device.BeginInvokeOnMainThread(() => {
				_painelTopLoad.ShowAlert();
			});
		}

		public void ExibirLoad()
		{
			Device.BeginInvokeOnMainThread(() => {
				_painelTopLoad.HideAlert();
			});
		}

        public void Is_True()
        {
            throw new System.NotImplementedException();
        }

        public void Is_False()
        {
            throw new System.NotImplementedException();
        }

		public void OpenStreetview(Boolean paramExibe) { }
		public void CloseStreetview(Boolean paramOnAppear) { }
	}
}
