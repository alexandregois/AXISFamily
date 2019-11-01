using System;
using family.CustomElements;
using family.Domain.Dto;
using family.Domain.Enum;
using family.ViewModels;
using family.Views.Interfaces;
using family.Views.Template;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using System.Threading.Tasks;


namespace family.Views
{
    public partial class ViewMapa : ContentPage, IMapa
    {
        public App _app => (Application.Current as App);

        private ViewModelMapa _viewModel { get; set; }

        private int _contador { get; set; }


        private CustomDialogAlert _dialogAlert = null;
        public CustomDialogAlert DialogAlert
        {
            get
            {
                if (_dialogAlert == null)
                {
                    _dialogAlert = new CustomDialogAlert(
                        PanelViewMapa
                        , Color.FromHex("#80000000")
                        , false
                    );
                }

                return _dialogAlert;
            }
            set
            {
                _dialogAlert = value;
            }
        }

        private ActivityIndicator _loader = null;
        private ActivityIndicator Loader
        {
            get
            {
                if (_loader == null)
                {
                    _loader = DialogAlert.RequireActivityIndicator();
                    _loader.Color = Color.FromHex("#7ff3ff");
                }
                return _loader;
            }
            set
            {
                _loader = value;
            }
        }

        public ViewModelStreetView _viewModelStreetView { get; set; }

        private ViewModelMapaGoogle _modelMapaGoogle;
        public ViewModelMapaGoogle ModelMapaGoogle
        {
            get
            {
                if (_modelMapaGoogle == null)
                    ModelMapaGoogle = new ViewModelMapaGoogle();
                return _modelMapaGoogle;
            }
            set
            {
                _modelMapaGoogle = value;
            }
        }

        public Map mapaPosicao { get; set; }

        public ContentView paramContentViewAtual { get; set; }

        public ViewMapa(
            EnumPage paramOpenPage
            , Int32 paramIdRastreador
        )
        {
            InitializeComponent();

            PanelGeral.ControlTemplate =
                new ControlTemplate(typeof(DefaultTemplate));

            _viewModel = new ViewModelMapa(
                paramOpenPage
                , paramIdRastreador
            );
            _viewModel.ActualView = this as IMapa;
            this.BindingContext = _viewModel;
            SizeBox();
            ExibirLoad();

            _contador = 0;


        }

        protected override void OnAppearing()
        {
            PainelMapa.Children.Clear();
            mapaPosicao = new Map();
            PainelMapa.Children.Add(mapaPosicao);

            ModelMapaGoogle._mapa = mapaPosicao;

            _viewModel.OnAppearing();

            try
            {
                if (_app.selectPinUltimaPosicao != null)
                    mapaPosicao.SelectedPin = _app.selectPinUltimaPosicao;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }


        }

        protected override void OnDisappearing()
        {
            _viewModel.OnDisappearing();
        }

        public void EscondeLoad()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    DialogAlert.HideAndCleanAlert();
                    DialogAlert.Destroy();
                    DialogAlert = null;

                    if (_app.selectPinUltimaPosicao != null)
                    {
                        if (mapaPosicao != null)
                        {
                            mapaPosicao.SelectedPin = _app.selectPinUltimaPosicao;
                        }
                    }

                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }

            });
        }

        public void OpenStreetview(Boolean paramExibe)
        {

            ExibirLoad();

            Device.BeginInvokeOnMainThread(async () =>
            {
                WebView WVStreet = new WebView();

                if (_app.ActualPosition == null)
                    _viewModelStreetView = new ViewModelStreetView(0, 0, paramExibe);
                else
                    _viewModelStreetView = new ViewModelStreetView(_app.ActualPosition.Latitude, _app.ActualPosition.Longitude, paramExibe);


                WVStreet = _viewModelStreetView.WVStreet;

                WVStreet.VerticalOptions = LayoutOptions.FillAndExpand;
                WVStreet.HorizontalOptions = LayoutOptions.FillAndExpand;


                if (paramExibe)
                {

                    //if (_contador < 2)
                    //{

                    PainelMapa.Children.Clear();
                    PainelMapa.Children.Add(WVStreet);

                    WVStreet.IsVisible = paramExibe;

                    if (_app.ActualPosition == null)
                        _viewModelStreetView = new ViewModelStreetView(0, 0, paramExibe);
                    else
                        _viewModelStreetView = new ViewModelStreetView(_app.ActualPosition.Latitude, _app.ActualPosition.Longitude, paramExibe);


                    WVStreet = _viewModelStreetView.WVStreet;

                    WVStreet.VerticalOptions = LayoutOptions.FillAndExpand;
                    WVStreet.HorizontalOptions = LayoutOptions.FillAndExpand;

                    await Task.Delay(50000);

                    //    _contador += 1;

                    //}

                }

                EscondeLoad();

            });


        }

        public void CloseStreetview(Boolean paramOnAppear)
        {

            PainelMapa.Children.Clear();
            mapaPosicao = new Map();
            PainelMapa.Children.Add(mapaPosicao);

            ModelMapaGoogle._mapa = mapaPosicao;

            _viewModel.OnAppearing();

            try
            {
                if (_app.selectPinUltimaPosicao != null)
                    mapaPosicao.SelectedPin = _app.selectPinUltimaPosicao;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

        }

        public void ExibirLoad()
        {
            ShowAlert(Loader);
        }

        public void ShowAlert(View paramView)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                DialogAlert.ShowAlert(paramView);
            });
        }

        public void AddPartialPage(
            ContentView paramContentView
        )
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                ContentLoad.Children.Clear();
                Task.Delay(3000);
                ContentLoad.Children.Add(paramContentView);
            });
        }

        public void ChangeColor(
            Color paramColorStatusBar
            , Color paramColorLoad
        )
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                _app.Util.changeColorStatusBar(paramColorStatusBar, this);
                Loader.Color = paramColorLoad;
            });
        }

        public ViewModelMapaGoogle GetModelMapaGoogle()
        {
            return ModelMapaGoogle;
        }

        public void SizeBox()
        {

            Device.BeginInvokeOnMainThread(() =>
            {
                Double menuBoxPositionY = 0;
                Double contentLoadPositionY = _viewModel.MenuHeight;

                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        contentLoadPositionY = menuBoxPositionY;
                        menuBoxPositionY = _viewModel.ContentLoadBoxHeight;
                        break;
                }

                Point pointContentLoad = new Point()
                {
                    X = 0,
                    Y = contentLoadPositionY
                };

                Size sizeContentLoad = new Size()
                {
                    Width = _app.ScreenWidth,
                    Height = _viewModel.ContentLoadBoxHeight
                };

                Point pointMenuBox = new Point()
                {
                    X = 0,
                    Y = menuBoxPositionY
                };

                Size sizeMenuBox = new Size()
                {
                    Width = _app.ScreenWidth,
                    Height = _viewModel.MenuHeight
                };

                AbsoluteLayout.SetLayoutFlags(
                    MenuBox
                    , AbsoluteLayoutFlags.None
                );

                AbsoluteLayout.SetLayoutBounds(
                    MenuBox
                    , new Rectangle(
                        pointMenuBox
                        , sizeMenuBox
                    )
                );

                AbsoluteLayout.SetLayoutFlags(
                    ContentLoadBox
                    , AbsoluteLayoutFlags.None
                );

                AbsoluteLayout.SetLayoutBounds(
                    ContentLoadBox
                    , new Rectangle(
                        pointContentLoad
                        , sizeContentLoad
                    )
                );
            });

        }

        private Button MontaButtonMenu(
            MenuMapa paramMenuMapa
            , String paramDef
        )
        {
            Button tempButton = new Button()
            {
                HeightRequest = _viewModel.MenuHeight,
                WidthRequest = _viewModel.MenuItemWidth,
                StyleId = "stack" + paramMenuMapa.Pagina.ToString(),
                Margin = new Thickness(1, 0),
                Image = paramMenuMapa.Icone,
                BorderColor = Color.Transparent,
                BorderWidth = 0,
                BorderRadius = 0,
                BackgroundColor = Color.Aquamarine
            };

            tempButton.SetBinding(
                Button.BackgroundColorProperty
                , new Binding(
                    paramDef + "BackgroundColor"
                    , BindingMode.Default
                    , null
                    , null
                    , null
                    , this.BindingContext
                )
            );

            tempButton.SetBinding(
                Button.OpacityProperty
                , new Binding(
                    paramDef + "Opacity"
                    , BindingMode.Default
                    , null
                    , null
                    , null
                    , this.BindingContext
                )
            );

            tempButton.SetBinding(
                Button.IsEnabledProperty
                , new Binding(
                    paramDef + "Enabled"
                    , BindingMode.Default
                    , null
                    , null
                    , null
                    , this.BindingContext
                )
            );

            tempButton.SetBinding(
                Button.OpacityProperty
                , new Binding(
                    "BoxMenuCommand"
                    , BindingMode.Default
                    , null
                    , paramMenuMapa
                    , null
                    , this.BindingContext
                )
            );

            return tempButton;
        }

    }
}
