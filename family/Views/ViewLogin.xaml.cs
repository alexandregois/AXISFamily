using family.CustomElements;
using family.ViewModels;
using family.Views.Interfaces;
using System;
using Xamarin.Forms;

namespace family.Views
{
    public partial class ViewLogin : ContentPage, ILoader
    {
        private ViewModelLogin _viewModelLogin { get; set; }
        private App _app => (Application.Current as App);


        private CustomDialogAlert _painelTopLoad { get; set; }

        public ViewLogin(Boolean paramIsPersonalizado, String paramNameProject)
        {
            InitializeComponent();
             
            if (_app.nameProject == "agility")
            {
                ButtonLogin.BackgroundColor = Color.FromHex("#FF8833");
                PageLogin.BackgroundColor = Color.FromHex("#810A9A");
            }

            _viewModelLogin = new ViewModelLogin(paramIsPersonalizado, paramNameProject);
            _viewModelLogin._view = this as ILoader;
            this.BindingContext = _viewModelLogin;

            GetPreferences();

            MontaLoad();
        }

        protected override void OnAppearing()
        {
            var lifecycleHandler = (ViewModelLogin)this.BindingContext;
            lifecycleHandler.OnAppearing();

            Color statusBarColor = Color.FromHex("#00000b");
            _app.Util.changeColorStatusBar(statusBarColor, this);
        }

        protected override void OnDisappearing()
        {
            var lifecycleHandler = (ViewModelLogin)this.BindingContext;
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
            activity.Color = Color.FromHex("#FF49435c");
            _painelTopLoad.AddChildren(activity);

        }

        private void GetPreferences()
        {
            if (Application.Current.Properties.ContainsKey("User"))
                if (Application.Current.Properties["User"] != null)
                    TxtEmail.Text = (string)Application.Current.Properties["User"];

            if (Application.Current.Properties.ContainsKey("Pass"))
                if (Application.Current.Properties["Pass"] != null)
                    TxtSenha.Text = (string)Application.Current.Properties["Pass"];
        }

        public void EscondeLoad()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                _painelTopLoad.HideAlert();
            });
        }

        public void ExibirLoad()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                _painelTopLoad.ShowAlert();
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
