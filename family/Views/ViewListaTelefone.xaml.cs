using System;
using family.CustomElements;
using family.ViewModels;
using family.Views.Interfaces;
using family.Views.Template;
using Xamarin.Forms;
using family.Views.Services;
using family.Domain.Dto;
using family.Domain.Enum;
using family.Domain.Realm;
using family.ViewModels.InterfaceServices;
using family.CrossPlataform;
using Microsoft.AppCenter.Crashes;
using family.Services.ServiceRealm;

namespace family.Views
{
#pragma warning disable CS4014
#pragma warning disable RECS0022
#pragma warning disable CS1998
    public partial class ViewListaTelefone : ContentPage, IListaUnidadeRastreada
    {
        private readonly IMessageService _messageService;

        private App _app => (Application.Current as App);

        private ICrossPlataformUtil _util;
        public ICrossPlataformUtil Util
        {
            get
            {
                if (_util == null)
                    _util = DependencyService.Get<ICrossPlataformUtil>();

                return _util;
            }
            set
            {
                _util = value;
            }
        }

        private ViewModelListaTelefone _viewModel { get; set; }

        private Boolean _boolStartPeriodic { get; set; }

        //private readonly INetworkProvider _networkProvider;

        private CustomDialogAlert _dialogAlert = null;
        public CustomDialogAlert DialogAlert
        {
            get
            {
                if (_dialogAlert == null)
                {
                    _dialogAlert = new CustomDialogAlert(Panel, Color.FromHex("#80000000"));
                    _dialogAlert.ShadowBox_TapDelegate += (sender, args) =>
                    {
                        //AtivaBotoes();
                    };
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

        public ViewListaTelefone()
        {
            InitializeComponent();


            PanelGeral.ControlTemplate =
                new ControlTemplate(typeof(DefaultTemplate));

            _viewModel = new ViewModelListaTelefone();
            _viewModel._view = this as IListaUnidadeRastreada;
            this.BindingContext = _viewModel;

            BuildTemplatePage();

            PanelLista.ItemTemplate = new DataTemplate(() =>
            {
                return new PanelListaTelefone_ViewCell(this.BindingContext);
            });


            this._messageService =
                    DependencyService.Get<IMessageService>();


            ViewModelConfiguracao viewModelConfiguracao = new ViewModelConfiguracao();

            viewModelConfiguracao.CheckKeepAlive();


        }

        protected override void OnAppearing()
        {
            Color statusColor = Color.FromHex("#a3455d");
            _app.Util.changeColorStatusBar(statusColor, this);

            _viewModel.OnAppearing();

            BuildTemplatePage();

            ServiceStart();

            //EscondeLoad();
        }

        protected override void OnDisappearing()
        {
            _viewModel.OnDisappearing();
        }

        public void BuildTemplatePage()
        {
            
        }

        private async void ListUnidades_ItemTapped(
            object sender
            , ItemTappedEventArgs e
        )
        {

            PosicaoUnidadeRastreadaRealm dataItem = e.Item as PosicaoUnidadeRastreadaRealm;

            _viewModel.NavegateMapa(
                EnumPage.UltimaPosicao
                , dataItem.IdRastreador
            );

        }

        #region Interface
        public void EscondeLoad()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                DialogAlert.HideAndCleanAlert();
                DialogAlert.Destroy();
                DialogAlert = null;

            });
        }

        public void ExibirLoad()
        {
            Device.BeginInvokeOnMainThread(() => ShowAlert(Loader));

        }

        public void BeginRefresh()
        {
            Boolean boolismessage = false;

            if (Application.Current.Properties.ContainsKey("IsMessage"))
            {
                if (Application.Current.Properties["IsMessage"] != null)
                    boolismessage = (Boolean)Application.Current.Properties["IsMessage"];
            }

            if (boolismessage == false)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    PanelLista.BeginRefresh();

                    BuildTemplatePage();

                    ServiceStart();

                });
            }
        }

        public void ServiceStart()
        {

            TokenDataStore tokenDataSource = new TokenDataStore();
            TokenRealm _tokenRealm = null;

            Int32 tempoTransmissao = _app.Configuracao.TempoTransmissaoPadrao;

            Boolean retornoTimer = false;

            if (_boolStartPeriodic == false)
            {

                Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(tempoTransmissao), () =>
                {

                    if (tokenDataSource.Get(1) != null)
                    {
                        _tokenRealm = tokenDataSource.Get(1);

                        if (_tokenRealm.TempoTransmissao > 0)
                        {
                            retornoTimer = false;
                            _boolStartPeriodic = true;

                        }

                    }
                    else
                    {
                        retornoTimer = true;
                    }

                    return retornoTimer;

                });
            }

        }        

        public void EndRefresh()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                PanelLista.EndRefresh();
                EscondeLoad();
            });
        }


        public void MakeFrameDefault(
            ref Frame paramFrame
            , ref StackLayout paramContent
        )
        {

            paramFrame = DialogAlert.RequireFramePadrao();
            paramContent.Margin = new Thickness(3);
        }

        public void ShowAlert(View paramView)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (DialogAlert != null)
                {
                    DialogAlert.HideAndCleanAlert();
                    DialogAlert.Destroy();
                    DialogAlert = null;
                }
                DialogAlert.ShowAlert(paramView);
            });
        }

        public void Is_True()
        {
            throw new NotImplementedException();
        }

        public void Is_False()
        {
            throw new NotImplementedException();
        }

        #endregion

        public void OpenStreetview(Boolean paramExibe) { }
        public void CloseStreetview(Boolean paramOnAppear) { }

    }
#pragma warning restore CS1998
#pragma warning restore RECS0022
#pragma warning restore CS4014
}